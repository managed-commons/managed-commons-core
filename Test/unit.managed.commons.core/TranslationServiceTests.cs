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
using Commons.Translation;
using Xunit;
using static Commons.Translation.TranslationService;

namespace Commons
{
    public class TranslationServiceTests
    {
        static TranslationServiceTests()
        {
            var translator = new DictionaryTranslator();
            translator.AddLocale("pt-BR", new Dictionary<string, string> {["Hello {0}!!!"] = "Alô {0}!!!",["See {0}"] = "Mais detalhes em {0}" });
            translator.AddLocale("es", new Dictionary<string, string> {["Hello {0}!!!"] = "Hola {0}!!!",["See {0}"] = "Siga {0}" });
            RegisterTranslator(translator);
        }

        [Fact]
        public void TestInterpolatedStringArgentinianSpanishTranslation()
        {
            TestHelloDavidFowler("es-AR", "Hola David Fowler!!!");
        }

        [Fact]
        public void TestInterpolatedStringNonTranslation()
        {
            TestHelloDavidFowler("en-US", "Hello David Fowler!!!");
        }

        [Fact]
        public void TestInterpolatedStringPortugueseBrazilianTranslation()
        {
            TestHelloDavidFowler("pt-BR", "Alô David Fowler!!!");
        }

        [Fact]
        public void TestInterpolatedStringSpanishTranslation()
        {
            TestHelloDavidFowler("es", "Hola David Fowler!!!");
        }

        [Fact]
        public void TestLicenseAttributeTranslation()
        {
            using (new TranslationServiceLocaleLock("pt-BR"))
            {
                var license = new LicenseAttribute(LicenseType.MIT);
                Assert.Equal("MIT License - Mais detalhes em https://opensource.org/licenses/MIT", license.ToString());
            }
        }

        private static void TestHelloDavidFowler(string locale, string expected)
        {
            using (new TranslationServiceLocaleLock(locale))
            {
                var name = "David Fowler";
                Assert.Equal(expected, __($"Hello {name}!!!"));
            }
        }
    }
}
