using System;

namespace MakingCodeGreatAgain.After.Logging
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DoNotLogAttribute : Attribute
    {
    }
}