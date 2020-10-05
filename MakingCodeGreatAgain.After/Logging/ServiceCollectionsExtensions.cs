using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MakingCodeGreatAgain.After.Logging
{
    public static class ServiceCollectionsExtensions
    {
        /// <summary>
        /// Call this method after all types whose public method calls need to be logged have been registered.
        /// It iterates across all types adding a Dynamic Proxy which will log the details of calls to public methods.
        /// In order for this to work with internal classes you need to add [assembly:InternalsVisibleTo("ProxyBuilder")]
        /// into the AssemblyInfo.cs.
        ///
        /// This is an issue in System.Reflection.DispatchProxy which is set to be addressed at some point.
        /// See https://github.com/dotnet/runtime/issues/30917.
        ///
        /// N.B. Types must implement an interface for their method calls to be logged.
        /// </summary>
        /// <param name="services">Service collection to add logging of method calls to.</param>
        /// <param name="namespace">Namespace containing types to add logging of public method calls to.</param>
        public static IServiceCollection LogMethodCalls(this IServiceCollection services, string @namespace)
        {
            foreach (var service in services.Where(
                    s =>
                        s.ServiceType.Namespace != null &&
                        s.ServiceType.Namespace.StartsWith(@namespace) &&
                        s.ServiceType.IsInterface)
                .ToArray())
            {
                DecorateWithMethodCallLoggingDispatchProxy(services, service.ServiceType);
            }

            return services;
        }

        private static void DecorateWithMethodCallLoggingDispatchProxy(IServiceCollection services, Type serviceType)
        {
            var createMethod =
                typeof(MethodCallLoggingDecorator<>)
                    .MakeGenericType(serviceType)
                    .GetMethod("Create");

            if (createMethod == null)
            {
                throw new InvalidOperationException("Could not find Create method on MethodCallLoggingDecorator");
            }

            var argInfos = createMethod.GetParameters();

            var descriptorsToDecorate = services
                .Where(s => s.ServiceType == serviceType)
                .ToList();

            foreach (var descriptor in descriptorsToDecorate)
            {
                var decorated = ServiceDescriptor.Describe(
                    serviceType,
                    sp =>
                    {
                        var decoratorInstance = createMethod.Invoke(
                            null,
                            argInfos.Select(
                                    info => info.ParameterType == (descriptor.ServiceType ?? descriptor.ImplementationType)
                                        ? sp.CreateInstance(descriptor)
                                        : sp.GetRequiredService(info.ParameterType))
                                .ToArray());

                        return decoratorInstance;
                    },
                    descriptor.Lifetime);

                services.Remove(descriptor);
                services.Add(decorated);
            }
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(services);
            }

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }
}