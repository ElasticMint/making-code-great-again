using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nest;
using Newtonsoft.Json;

namespace MakingCodeGreatAgain.After.ElasticSearch.Investors.Query
{
    internal static class InvestmentAmountUsdScript
    {
        public static ScriptFieldsDescriptor InvestmentAmountUsd(
            this ScriptFieldsDescriptor script)
        {
            const string fieldScript = @"
                def commitments = params['_source'].funds != null 
                    ? params['_source'].funds
                        .stream()
                        .map(m->m.investedAmount?.usdMn)
                        .collect(Collectors.toList())
                    : [];

                def amount = 0;
                for (def i = 0; i < commitments.length; ++i) {
                    amount += commitments[i];
                }

                return [
                   'investmentAmountUsd' : amount;
                ];
            "; 

            return script.ScriptField(
                "investmentAmountUsd",
                sc => sc
                    .Source(fieldScript));
        }

        public static decimal? FieldValue(IReadOnlyCollection<FieldValues> fields, int index)
        {
            var fieldValue = ((List<FieldValues>)fields)[index];
            var commitmentValue = fieldValue["investmentAmountUsd"];
            var myScriptProperty = commitmentValue.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Single(pi => pi.Name == "Token");
            var myScript = myScriptProperty.GetValue(commitmentValue, null);
            if (myScript == null)
            {
                return default;
            }

            // ReSharper disable PossibleNullReferenceException
            var commitment = ((Newtonsoft.Json.Linq.JContainer)JsonConvert.DeserializeObject(myScript.ToString())).First.First.First;
            var commitmentValueUsd = ((Newtonsoft.Json.Linq.JValue)commitment).Value;
            // ReSharper restore PossibleNullReferenceException
            return commitmentValueUsd != null ? Convert.ToDecimal(commitmentValueUsd) : default(decimal?);
        }
    }
}