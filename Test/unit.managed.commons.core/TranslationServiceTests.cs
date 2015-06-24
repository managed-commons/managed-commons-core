using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Translation;
using Xunit;
using static Commons.Translation.TranslationService;

namespace Commons.Core
{
    public class TranslationServiceTests
    {
        static TranslationServiceTests()
        {
            var translator = new DictionaryTranslator();
            translator.AddLocale("pt-BR", new Dictionary<string, string> {["Hello {0}!!!"] = "Alô {0}!!!",["See {0}\n"] = "Mais detalhes em {0}\n" });
            translator.AddLocale("es", new Dictionary<string, string> {["Hello {0}!!!"] = "Hola {0}!!!",["See {0}\n"] = "Siga {0}\n" });
            RegisterTranslator(translator);
        }

        [Fact]
        public void TestInterpolatedStringNonTranslation()
        {
            Locale = "en-US";
            var name = "David";
            Assert.Equal("Hello David!!!", __($"Hello {name}!!!"));
        }

        [Fact]
        public void TestInterpolatedStringPortugueseBrazilianTranslation()
        {
            Locale = "pt-BR";
            var name = "David";
            Assert.Equal("Alô David!!!", __($"Hello {name}!!!"));
        }

        [Fact]
        public void TestInterpolatedStringSpanishTranslation()
        {
            Locale = "es";
            var name = "David";
            Assert.Equal("Hola David!!!", __($"Hello {name}!!!"));
            Locale = "es-AR";
            Assert.Equal("Hola David!!!", __($"Hello {name}!!!"));
        }

        [Fact]
        public void TestLicenseAttributeTranslation()
        {
            Locale = "pt-BR";
            var license = new LicenseAttribute(LicenseType.MIT);
            Assert.Equal("MIT License - Mais detalhes em https://opensource.org/licenses/MIT\n", license.ToString());
        }
    }
}
