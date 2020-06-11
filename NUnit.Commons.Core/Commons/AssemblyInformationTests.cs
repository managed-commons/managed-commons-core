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
using System.Linq;
using System.Reflection;
using Commons;
using Commons.Text;
using Commons.Translation;
using NUnit.Framework;

[assembly: Commons.Author("Rafael 'Monoman' Teixeira")]
[assembly: Commons.Author("Managed Commons Team")]
[assembly: License(LicenseType.MIT)]
[assembly: About("Unit tests using NUnit")]
[assembly: ReportBugsTo("https://github.com/managed-commons/managed-commons-core/issues")]
[assembly: AdditionalInfo("Additional info for testing purposes")]
[assembly: IsPartOfPackage("Managed Commons")]

namespace Commons
{
    [TestFixture]
    public class AssemblyInformationTests
    {
        [Test]
        public void ToStringOnSimpleAssemblyInfo()
        {
            using (new TranslationServiceLocaleLock("en-US")) {
                var assembly = _thisAssembly;
                var info = new AssemblyInformation(assembly);
                var version = info.Version;
                var expected = $@"
NUnit.Commons.Core  {version} - Copyright ©2002-2020 Rafael 'Monoman' Teixeira, Managed Commons Team
Unit tests to core meta-information library

License: MIT License - See https://opensource.org/licenses/MIT

Unit tests using NUnit
Authors: Rafael 'Monoman' Teixeira, Managed Commons Team
Company: Managed Commons

Additional info for testing purposes

Please report bugs at <https://github.com/managed-commons/managed-commons-core/issues>
".WithEnvironmentNewLines();
                Assert.AreEqual(expected, info.ToString());
            }
        }


        static Assembly _thisAssembly =>
            typeof(AssemblyInformationTests).Assembly;
    }
}
