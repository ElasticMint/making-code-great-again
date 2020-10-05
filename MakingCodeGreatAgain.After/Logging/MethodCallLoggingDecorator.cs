using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace MakingCodeGreatAgain.After.Logging
{
    public class MethodCallLoggingDecorator<TDecorated> : DispatchProxy where TDecorated : class
    {
        private TDecorated _decorated;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (!Log.IsEnabled(LogEventLevel.Verbose))
            {
                return targetMethod.Invoke(_decorated, args);
            }

            var targetType = _decorated.GetType();
            var logger = Log.ForContext(targetType);

            try
            {
                var serialisedArgs = new List<string>();

                for (var i = 0; i < args.Length; ++i)
                {
                    var argument = args[i];

                    var parameterInfo = targetMethod.GetParameters()[i];
                    var parameterName = parameterInfo.Name;
                    var parameterValue = JsonSerializer.Serialize(argument);
                    if (parameterInfo.CustomAttributes.Any(x => x.AttributeType == typeof(DoNotLogAttribute)) ||
                        parameterInfo.ParameterType == typeof(Stream))
                    {
                        parameterValue = "*****";
                    }
                    serialisedArgs.Add($"{parameterName}: {parameterValue}");
                }

                logger.Verbose(
                    "Invoking {class}.{methodName} with args {args}",
                    targetType,
                    targetMethod.Name,
                    serialisedArgs);

                var result = targetMethod.Invoke(_decorated, args);

                if (result is Task resultTask)
                {
                    resultTask.ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            logger.Error(
                                task.Exception,
                                "Error during invocation of {class}.{methodName}",
                                targetType,
                                targetMethod.Name);
                        }
                        else
                        {
                            if (targetMethod.ReturnType.IsGenericType)
                            {
                                var resultProperty = result.GetType().GetProperty("Result");
                                if (resultProperty != null)
                                {
                                    logger.Verbose(
                                        "{returnValue} returned from {class}.{methodName}",
                                        JsonSerializer.Serialize(resultProperty.GetValue(result)),
                                        targetType,
                                        targetMethod.Name);
                                }
                            }

                            logger.Verbose(
                                "{class}.{methodName} completed",
                                targetType,
                                targetMethod.Name);
                        }
                    });
                }
                else
                {
                    if (targetMethod.ReturnType != typeof(void) && targetMethod.ReturnType != typeof(Stream))
                    {
                        logger.Verbose(
                            "{returnValue} returned from {class}.{methodName}",
                            JsonSerializer.Serialize(result),
                            targetType,
                            targetMethod.Name);
                    }

                    logger.Verbose(
                        "{class}.{methodName} completed",
                        targetType,
                        targetMethod.Name);
                }

                return result;
            }
            catch (TargetInvocationException ex)
            {
                logger.Error(
                    ex.InnerException ?? ex,
                    "Error during invocation of {class}.{methodName}",
                    targetType,
                    targetMethod.Name);
                throw ex.InnerException ?? ex;
            }
        }

        public static TDecorated Create(TDecorated decorated)
        {
            object proxy = Create<TDecorated, MethodCallLoggingDecorator<TDecorated>>();
            ((MethodCallLoggingDecorator<TDecorated>)proxy)._decorated =
                decorated ??
                throw new ArgumentNullException(nameof(decorated));

            return (TDecorated)proxy;
        }
    }
}