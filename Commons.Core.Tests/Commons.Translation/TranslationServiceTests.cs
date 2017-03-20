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
using NUnit.Framework;
using static Commons.Translation.TranslationService;

namespace Commons
{
    [TestFixture]
    public class TranslationServiceTests
    {
        static TranslationServiceTests()
        {
            var translator = new DictionaryTranslator();
            translator.AddLocale("pt-BR", new Dictionary<string, string>
            {
                ["Hello {0}!!!"] = "Alô {0}!!!",
                ["See {0}"] = "Mais detalhes em {0}",
                ["Value {0:000} at {1:f}!!!"] = "Valor {0:000} em {1:f}!!!",
                ["Nothing"] = "Nada",
                ["One"] = "Um",
                ["Two"] = "Dois",
                ["Many"] = "Muitos"
            });
            translator.AddLocale("es", new Dictionary<string, string>
            {
                ["Hello {0}!!!"] = "Hola {0}!!!",
                ["See {0}"] = "Siga {0}",
                ["Value {0:000} at {1:f}!!!"] = "Valor {0:000} en {1:f}!!!",
                ["Nothing"] = "Nada",
                ["One"] = "Un",
                ["Two"] = "Dos",
                ["Many"] = "Muchos"
            });
            RegisterTranslator(translator, Context);
        }

        [Test]
        public void TestFailInterpolatedStringArgentinianSpanishTranslation()
        {
            TestFailDavidFowler("es-AR");
        }

        [Test]
        public void TestFailInterpolatedStringNonTranslation()
        {
            TestFailDavidFowler("en-US");
        }

        [Test]
        public void TestFailInterpolatedStringPortugueseBrazilianTranslation()
        {
            TestFailDavidFowler("pt-BR");
        }

        [Test]
        public void TestFailPluralsNonTranslation()
        {
            TestFailPlurals("en-US");
        }

        [Test]
        public void TestFailPluralsPortugueseBrazilianTranslation()
        {
            TestFailPlurals("pt-BR");
        }

        [Test]
        public void TestFailPluralsSpanishTranslation()
        {
            TestFailPlurals("es");
        }

        [Test]
        public void TestInterpolatedStringArgentinianSpanishTranslation()
        {
            TestHelloDavidFowler("es-AR", "Hola David Fowler!!!");
        }

        [Test]
        public void TestInterpolatedStringNonTranslation()
        {
            TestHelloDavidFowler("en-US", "Hello David Fowler!!!");
        }

        [Test]
        public void TestInterpolatedStringPortugueseBrazilianTranslation()
        {
            TestHelloDavidFowler("pt-BR", "Alô David Fowler!!!");
        }

        [Test]
        public void TestInterpolatedStringSpanishTranslation()
        {
            TestHelloDavidFowler("es", "Hola David Fowler!!!");
        }

        [Test]
        public void TestInterpolatedStringWithFormatSpecifierNonTranslation()
        {
            TestInterpolatedStringWithFormatSpecifier("en-US", "Value 013 at Friday, October 30, 2015 7:40 PM!!!");
        }

        [Test]
        public void TestInterpolatedStringWithFormatSpecifierPortugueseBrazilianTranslation()
        {
            TestInterpolatedStringWithFormatSpecifier("pt-BR", "Valor 013 em sexta-feira, 30 de outubro de 2015 19:40!!!");
        }

        [Test]
        public void TestInterpolatedStringWithFormatSpecifierSpanishTranslation()
        {
            TestInterpolatedStringWithFormatSpecifier("es", "Valor 013 en viernes, 30 de octubre de 2015 19:40!!!");
        }

        [Test]
        public void TestLicenseAttributeTranslation()
        {
            using (new TranslationServiceLocaleLock("pt-BR")) {
                var license = new LicenseAttribute(LicenseType.MIT);
                Assert.AreEqual("MIT License - Mais detalhes em https://opensource.org/licenses/MIT", license.ToString());
            }
        }

        [Test]
        public void TestPluralsNonTranslation()
        {
            TestPlurals("en-US", "Nothing", "One", "Two", "Many");
        }

        [Test]
        public void TestPluralsPortugueseBrazilianTranslation()
        {
            TestPlurals("pt-BR", "Nada", "Um", "Dois", "Muitos");
        }

        [Test]
        public void TestPluralsSpanishTranslation()
        {
            TestPlurals("es", "Nada", "Un", "Dos", "Muchos");
        }

        const string Context = "Unit.Commons.Core";

        static void TestFailDavidFowler(string locale)
        {
            using (new TranslationServiceLocaleLock(locale)) {
                var catched = 0;
                LocalizationWarningEventHandler handler = (object sender, LocalizationWarningEventArgs e) => {
                    catched++;
                    Assert.AreEqual(locale, e.LanguageName);
                    Assert.AreEqual("Fail {0}!!!", e.MissingText);
                    if (e.Context != "*")
                        Assert.AreEqual(Context, e.Context);
                };
                ProblemDetected += handler;
                try {
                    const string name = "David Fowler";
                    Assert.IsNotEmpty(__($"Fail {name}!!!", Context));
                    Assert.AreEqual(2, catched);
                } finally {
                    ProblemDetected -= handler;
                }
            }
        }

        static void TestFailPlurals(string locale)
        {
            using (new TranslationServiceLocaleLock(locale)) {
                var catched = 0;
                LocalizationWarningEventHandler handler = (object sender, LocalizationWarningEventArgs e) => {
                    catched++;
                    Assert.AreEqual(locale, e.LanguageName);
                    Assert.AreEqual("No thing|One|Two|Many", e.MissingText);
                    if (e.Context != "*")
                        Assert.AreEqual(Context, e.Context);
                };
                ProblemDetected += handler;
                try {
                    Assert.IsNotEmpty(_s(Context, 0, "No thing", "One", "Two", "Many"));
                    Assert.AreEqual(2, catched);
                } finally {
                    ProblemDetected -= handler;
                }
            }
        }

        static void TestHelloDavidFowler(string locale, string expected)
        {
            using (new TranslationServiceLocaleLock(locale)) {
                const string name = "David Fowler";
                Assert.AreEqual(expected, __($"Hello {name}!!!"));
            }
        }

        static void TestInterpolatedStringWithFormatSpecifier(string locale, string expected)
        {
            using (new TranslationServiceLocaleLock(locale)) {
                const int value = 13;
                var datetime = new DateTime(2015, 10, 30, 19, 40, 0);
                Assert.AreEqual(expected, __($"Value {value:000} at {datetime:f}!!!"));
            }
        }

        static void TestPlurals(string locale, string expectedNone, string expectedOne, string expectedTwo, string expectedMany)
        {
            using (new TranslationServiceLocaleLock(locale)) {
                Assert.AreEqual(expectedNone, _s(Context, 0, "Nothing", "One", "Two", "Many"));
                Assert.AreEqual(expectedOne, _s(Context, 1, "Nothing", "One", "Two", "Many"));
                Assert.AreEqual(expectedTwo, _s(Context, 2, "Nothing", "One", "Two", "Many"));
                Assert.AreEqual(expectedMany, _s(Context, 3, "Nothing", "One", "Two", "Many"));
                Assert.AreEqual(expectedMany, _s(Context, 2, "Nothing", "One", "Many"));
                Assert.AreEqual("Three", _s(Context, 3, "Nothing", "One", "Two", "Three", "Many"));
                Assert.AreEqual(expectedMany, _s(Context, 4, "Nothing", "One", "Two", "Three", "Many"));
            }
        }
    }
}
