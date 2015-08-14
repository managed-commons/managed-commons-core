﻿// Commons.Core
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
using Commons.Translation;
using NUnit.Framework;

namespace Commons
{
    [TestFixture]
    public class AssemblyInformationTests
    {
        [Test]
        public void ToStringOnSimpleAssemblyInfo()
        {
            using (new TranslationServiceLocaleLock("en-US"))
            {
                var info = new AssemblyInformation(typeof(AssemblyInformationTests).Assembly);
                var version = info.Version;
                var expected = $@"
Unit.Commons.Core  {version} - Copyright ©2002-2015 Rafael 'Monoman' Teixeira, Managed Commons Team
Unit tests to core meta-information library

License: MIT License - See https://opensource.org/licenses/MIT

Unit tests using NUnit
Authors: Rafael 'Monoman' Teixeira, Managed Commons Team

Additional info for testing purposes

Please report bugs at <https://github.com/managed-commons/managed-commons-core/issues>
";
                Assert.AreEqual(expected.TrimStart('\r', '\n'), info.ToString());
            }
        }
    }
}
