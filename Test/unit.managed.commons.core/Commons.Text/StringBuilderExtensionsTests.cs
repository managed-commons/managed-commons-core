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
using System.Text;
using NUnit.Framework;

namespace Commons.Text.Tests
{
    [TestFixture]
    public class StringBuilderExtensionsTests
    {
        [Test]
        public void AppendLineIfNotEmpty()
        {
            var sb = new StringBuilder();
            sb.AppendLineIfNotEmpty(null);
            Assert.AreEqual(string.Empty, sb.ToString());
            sb.AppendLineIfNotEmpty("something");
            AssertLineIs("something", sb);
            sb.AppendLineIfNotEmpty(null);
            AssertLineIs("something", sb);
            sb.AppendLineIfNotEmpty(string.Empty);
            AssertLineIs("something", sb);
            sb.AppendLineIfNotEmpty("   ");
            AssertLineIs("something", sb);
        }

        [Test]
        public void AppendLineIfNotNull_1()
        {
            var sb = new StringBuilder();
            sb.AppendLineIfNotNull(1);
            AssertLineIs("1", sb);
        }

        [Test]
        public void AppendLineIfNotNull_Null()
        {
            var sb = new StringBuilder();
            sb.AppendLineIfNotNull(null);
            Assert.AreEqual(string.Empty, sb.ToString());
        }

        [Test]
        public void AppendLineIfNotNull_Text_1()
        {
            var sb = new StringBuilder();
            sb.AppendLineIfNotNull(1, "Text");
            AssertLineIs("Text", sb);
        }

        [Test]
        public void AppendLinesWithIndentAndSeparator()
        {
            AssertAppendedLines((sb, items, indent, separator) => sb.AppendLinesWithIndentAndSeparator(items, indent), ',');
            AssertAppendedLines((sb, items, indent, separator) => sb.AppendLinesWithIndentAndSeparator(items, indent, ','), ',');
            AssertAppendedLines((sb, items, indent, separator) => sb.AppendLinesWithIndentAndSeparator(items, indent, ';'), ';');
        }

        [Test]
        public void TrimLastSeparatorPreservingWhitespace()
        {
            AssertTrim(sb => sb.TrimLastSeparatorPreservingWhitespace(), ',');
            AssertTrim(sb => sb.TrimLastSeparatorPreservingWhitespace(','), ',');
            AssertTrim(sb => sb.TrimLastSeparatorPreservingWhitespace(';'), ';');
        }

        [Test]
        public void TruncateAtEmpty_10_ExpectsEmpty()
        {
            TestTruncateAt(null, 10, string.Empty);
        }

        [Test]
        public void TruncateAtFilled_0_ExpectsEmpty()
        {
            TestTruncateAt(SOMETHING, 0, string.Empty);
        }

        [Test]
        public void TruncateAtFilled_24_ExpectsSomething()
        {
            TestTruncateAt(SOMETHING, 24, SOMETHING);
        }

        [Test]
        public void TruncateAtFilled_4_ExpectsSome()
        {
            TestTruncateAt(SOMETHING, 4, "Some");
        }

        const string SOMETHING = "Something";

        static void AssertAppendedLines(Action<StringBuilder, IEnumerable<string>, string, char> append, char separator)
        {
            var sb = new StringBuilder();
            var indent = ".";
            var items = new string[] { "one", "two", "three" };
            string expected = string.Format("{1}one{2}{0}{1}two{2}{0}{1}three{2}{0}", Environment.NewLine, indent, separator);
            append(sb, items, indent, separator);
            Assert.AreEqual(expected, sb.ToString());
        }

        static void AssertLineIs(string expected, StringBuilder sb)
        {
            Assert.AreEqual(expected + Environment.NewLine, sb.ToString());
        }

        static void AssertTrim(Action<StringBuilder> trim, char separator)
        {
            string expected = $"one,two,three\t  \t{Environment.NewLine}";
            var sb = new StringBuilder();
            sb.AppendLine($"one,two,three{separator}\t  \t");
            trim(sb);
            Assert.AreEqual(expected, sb.ToString());
        }

        static void TestTruncateAt(string append, int truncateAt, string expected)
        {
            var sb = new StringBuilder();
            sb.Append(append);
            sb.TruncateAt(truncateAt);
            Assert.AreEqual(expected, sb.ToString());
        }
    }
}
