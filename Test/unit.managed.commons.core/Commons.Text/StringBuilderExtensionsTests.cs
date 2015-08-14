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
    }
}