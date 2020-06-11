using System;
using System.Linq;

namespace Commons.Translation
{
    /// <summary>
    /// This marks that a string literal passed into the parameter or assigned to a property, will ultimately be
    /// translated and can be automatically extracted by a tool (normally the compiler)
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class TranslatableAttribute : Attribute
    {
    }
}
