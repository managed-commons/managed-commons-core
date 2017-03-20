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
using System.Collections.Generic;
using System.Linq;

namespace Commons.Translation
{
    public class DictionaryTranslator : ITranslator
    {
        public void AddLocale(string locale, Dictionary<string, string> dictionary)
        {
            _AddLocale(locale, dictionary);
        }

        public string Translate(string locale, string textToTranslate) => _Translate(locale, textToTranslate);

        public string TranslatePlural(string locale, int quantity, string none, string singular, params string[] plurals)
        => TranslationService.DefaultPlural(quantity, _Translate(locale, none), _Translate(locale, singular), _Translate(locale, plurals));

        private readonly Dictionary<string, Dictionary<string, string>> _ = new Dictionary<string, Dictionary<string, string>>();

        private static string LanguageFrom(string locale) => locale.Split('-')[0];

        private static string ValidateAndNormalizeLocale(string locale) => string.IsNullOrWhiteSpace(locale) ? throw new ArgumentNullException(nameof(locale)) : locale.ToLower();

        private void _AddLocale(string locale, Dictionary<string, string> dictionary)
        {
            locale = ValidateAndNormalizeLocale(locale);
            if (_.ContainsKey(locale))
                throw new ArgumentException("Locale already added!!!", nameof(locale));
            _[locale] = dictionary;
            if (locale.Contains("-"))
            {
                locale = LanguageFrom(locale);
                if (!_.ContainsKey(locale))
                    _[locale] = dictionary;
            }
        }

        private string[] _Translate(string locale, string[] plurals) => plurals.Select(s => _Translate(locale, s)).ToArray();

        private string _Translate(string locale, string textToTranslate)
        {
            locale = ValidateAndNormalizeLocale(locale);
            if (_.ContainsKey(locale))
            {
                var dic = _[locale];
                if (dic.ContainsKey(textToTranslate))
                    return dic[textToTranslate];
            }
            if (locale.Contains("-"))
                return _Translate(LanguageFrom(locale), textToTranslate);
            return null;
        }
    }
}
