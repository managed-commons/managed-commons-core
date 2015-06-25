using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Commons.Reflection;

namespace Commons.Core
{
    public class AssemblyInformationTests
    {
        [Fact]
        public void ToStringOnSimpleAssemblyInfo()
        {
            Translation.TranslationService.Locale = "en-US";
            var info = new AssemblyInformation(typeof(AssemblyInformation).Assembly);
            Assert.Equal(
                "Commons.Core  2.0.0-Alpha - Copyright ©2002-2015 Rafael 'Monoman' Teixeira, Managed Commons Team\r\n"+
                "Core meta-information library\r\n\r\n"+
                "License: MIT License - Mais detalhes em https://opensource.org/licenses/MIT\n\r\n\r\n\r\n"+
                "Authors: Rafael 'Monoman' Teixeira, Managed Commons Team\r\n", 
                info.ToString());
        }
    }
}
