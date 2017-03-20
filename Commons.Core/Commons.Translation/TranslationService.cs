// Commons.Core
//
// Copyright (c) 2002-2015 Rafael 'Monoman' Teixeira, Managed Commons Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Globalization;
using Commons.Text;

namespace Commons.Translation
{
    public static class TranslationService
    {
        public static event LocalizationWarningEventHandler ProblemDetected;

        static CultureInfo GetCultureInfo(string locale) =>
#if NET46
            CultureInfo.GetCultureInfo(locale);
#else
            new CultureInfo(locale);
#endif

        public static string Locale { get => _locale ?? CultureInfo.CurrentCulture.Name; set => _locale = value != null ? GetCultureInfo(value).Name : null; }

        public static string _([Translatable] string textToTranslate, [TranslationContext] string context = null)
            => InnerTranslate(Locale, context ?? "-", textToTranslate);

        public static string __([Translatable] FormattableString textToTranslate, [TranslationContext] string context = null)
            => InnerFormat(Locale, context ?? "-", textToTranslate.Format, textToTranslate.GetArguments());

        public static string _f([Translatable] string textToTranslate, params object[] args)
             => InnerFormat(_locale, "-", textToTranslate, args);

        public static string __f([TranslationContext] string context, [Translatable] string textToTranslate, params object[] args)
            => InnerFormat(_locale, context ?? "-", textToTranslate, args);

        public static string _s([TranslationContext] string context, int quantity, [Translatable] string none, [Translatable] string singular, [Translatable] params string[] plurals)
            => InnerTranslatePlural(Locale, context ?? "-", quantity, none, singular, plurals);

        public static string _s(int quantity, [Translatable] string none, [Translatable] string singular, [Translatable] params string[] plurals)
             => InnerTranslatePlural(Locale, "-", quantity, none, singular, plurals);

        public static string DefaultPlural(int quantity, string none, string singular, string[] plurals)
        {
            var pluralsLimit = plurals.Length;
            return (quantity == 0) ? none : ((quantity == 1) ? singular : (quantity > pluralsLimit ? plurals[pluralsLimit - 1] : plurals[quantity - 2]));
        }

        public static void RegisterTranslator(ITranslator translator, [TranslationContext] string context = null)
        {
            if (translator == null)
                throw new ArgumentNullException(nameof(translator));
            _chain = new TranslatorInChain(context ?? "-", translator, _chain);
        }

        public static string Translate(string locale, [Translatable] string textToTranslate, [TranslationContext] string context = null)
        {
            if (locale == null)
                throw new ArgumentNullException(nameof(locale));
            return InnerTranslate(locale, context ?? "-", textToTranslate);
        }

        public static string TranslateAndFormat(string locale, [TranslationContext] string context, [Translatable] string textToTranslate, params object[] args)
            => InnerFormat(locale, context ?? "-", textToTranslate, args);

        public static string TranslatePlural(string locale, [TranslationContext] string context, int quantity, [Translatable] string none, [Translatable] string singular, [Translatable] params string[] plurals)
        {
            if (locale == null)
                throw new ArgumentNullException(nameof(locale));
            return InnerTranslatePlural(locale, context ?? "-", quantity, none, singular, plurals);
        }

        static TranslatorInChain _chain;

        static string _locale;

        static string FindAndTranslate(Func<TranslatorInChain, string, string> use, string languageName, string context, string text)
        {
            var translator = _chain;
            while (translator != null && (context == "*" || translator.Context == context))
            {
                var result = use?.Invoke(translator, languageName);
                if (result != null)
                    return result;
                translator = translator.Next;
            }
            var translated = (context != "*") ? FindAndTranslate(use, languageName, "*", text) : null;
            if (translated == null)
                RaiseProblemDetectedEvent(languageName, context, text);
            return translated;
        }

        static string InnerFormat(string locale, string context, string textToTranslate, object[] args)
        {
            if (locale == null)
                throw new ArgumentNullException(nameof(locale));
            return textToTranslate.TransformOrNot(s => string.Format(GetCultureInfo(locale), InnerTranslate(locale, context, s), args));
        }

        static string InnerTranslate(string locale, string context, string textToTranslate)
            => textToTranslate.TransformOrNot(s => FindAndTranslate((tr, language) => tr.Translate(language, s), locale, context, textToTranslate));

        static string InnerTranslatePlural(string locale, string context, int quantity, string none, string singular, params string[] plurals)
        {
            if (singular == null)
                throw new ArgumentNullException(nameof(singular));
            if (plurals == null)
                throw new ArgumentNullException(nameof(plurals));
            if (quantity < 0 || (quantity == 0 && none.IsEmpty()))
                throw new ArgumentOutOfRangeException(nameof(quantity));
            return FindAndTranslate((tr, language) => tr.TranslatePlural(language, quantity, none, singular, plurals), locale, context, none + "|" + singular + "|" + plurals.JoinWith("|"))
                ?? DefaultPlural(quantity, none, singular, plurals);
        }

        static void RaiseProblemDetectedEvent(string languageName, string context, string missingText)
        {
            ProblemDetected?.Invoke(null, new LocalizationWarningEventArgs(languageName, context, missingText));
        }

        class TranslatorInChain : ITranslator
        {
            public readonly string Context;
            public readonly TranslatorInChain Next;

            public TranslatorInChain(string context, ITranslator translator, TranslatorInChain next)
            {
                Context = context;
                _translator = translator;
                Next = next;
            }

            public string Translate(string locale, string textToTranslate) => _translator.Translate(locale, textToTranslate);

            public string TranslatePlural(string locale, int quantity, string none, string singular, params string[] plurals)
                => _translator.TranslatePlural(locale, quantity, none, singular, plurals);

            readonly ITranslator _translator;
        }
    }
}
