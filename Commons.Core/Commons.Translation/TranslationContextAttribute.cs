using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Translation
{
    /// <summary>
    /// This marks that a string that should be passed (possibly inserted by the compiler) marking the context (assembly fullname) for matching translations
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class TranslationContextAttribute : Attribute
    {
    }
}
