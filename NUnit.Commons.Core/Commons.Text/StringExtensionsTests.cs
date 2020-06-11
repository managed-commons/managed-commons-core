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
using NUnit.Framework;
using static Commons.Commons.Text.TestUtils;

namespace Commons.Text.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void TestAsBool() {
            Assert.That(((string)null).AsBool(), Is.EqualTo(false));
            Assert.That("".AsBool(), Is.EqualTo(false));
            Assert.That(".".AsBool(), Is.EqualTo(false));
            Assert.That("    \u00A0".AsBool(), Is.EqualTo(false));
            Assert.That("true".AsBool(), Is.EqualTo(true));
            Assert.That("True".AsBool(), Is.EqualTo(true));
            Assert.That("TRUE".AsBool(), Is.EqualTo(true));
            Assert.That("false".AsBool(), Is.EqualTo(false));
            Assert.That("-128".AsBool(), Is.EqualTo(false));
            Assert.That("Blah".AsBool(), Is.EqualTo(false));
        }

        [Test]
        public void TestAsEnum() {
            Assert.That(((string)null).AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.LastValue));
            Assert.That("".AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.LastValue));
            Assert.That("FirstValue".AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.FirstValue));
            Assert.That("secondvalue".AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.SecondValue));
            Assert.That("THIRDvalue".AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.ThirdValue));
            Assert.That("FourthValue".AsEnum(TestEnum.LastValue), Is.EqualTo(TestEnum.LastValue));
            Assert.That("FirstValue".AsEnum(0), Is.EqualTo(0));
            Assert.That("FirstValue".AsEnum(OtherTestEnum.LastValue), Is.EqualTo(OtherTestEnum.FirstValue));
            Assert.That("SecondValue".AsEnum(OtherTestEnum.LastValue), Is.EqualTo(OtherTestEnum.LastValue));
        }

        [Test]
        public void TestAsInt() {
            Assert.That(((string)null).AsInt(), Is.EqualTo(0));
            Assert.That("".AsInt(), Is.EqualTo(0));
            Assert.That(".".AsInt(), Is.EqualTo(0));
            Assert.That("    \u00A0".AsInt(), Is.EqualTo(0));
            Assert.That("1".AsInt(), Is.EqualTo(1));
            Assert.That("2.0".AsInt(), Is.EqualTo(0));
            Assert.That("-128".AsInt(), Is.EqualTo(-128));
            Assert.That("4294967296".AsInt(), Is.EqualTo(0));
        }

        [Test]
        public void TestAsLines() {
            CompareStringEnumerable(((string)null).AsLines());
            CompareStringEnumerable(("").AsLines());
            CompareStringEnumerable(("    ").AsLines(), string.Empty);
            CompareStringEnumerable(("OnlyLine").AsLines(), "OnlyLine");
            CompareStringEnumerable(("FirstLine\nSecondLine").AsLines(), "FirstLine", "SecondLine");
            CompareStringEnumerable(("FirstLine\n  \nThirdLine").AsLines(), "FirstLine", string.Empty, "ThirdLine");
        }

        [Test]
        public void TestAsLong() {
            Assert.That(((string)null).AsLong(), Is.EqualTo(0));
            Assert.That("".AsLong(), Is.EqualTo(0));
            Assert.That(".".AsLong(), Is.EqualTo(0));
            Assert.That("    \u00A0".AsLong(), Is.EqualTo(0));
            Assert.That("1".AsLong(), Is.EqualTo(1));
            Assert.That("2.0".AsLong(), Is.EqualTo(0));
            Assert.That("4294967296".AsLong(), Is.EqualTo(4294967296));
            Assert.That("42949672960".AsLong(), Is.EqualTo(42949672960));
        }

        [Test]
        public void TestAsUtf8Bytes() {
            Assert.That(((string)null).AsUtf8Bytes(), Is.Empty);
            Assert.That("".AsUtf8Bytes(), Is.Empty);
            var bytes = ".".AsUtf8Bytes();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(bytes.Length, Is.EqualTo(1));
            Assert.That(bytes[0], Is.EqualTo((byte)'.'));
        }

        [Test]
        public void TestAtMost() {
            Assert.That(((string)null).AtMost(10), Is.Null);
            Assert.That("".AtMost(10), Is.Empty);
            Assert.That(".".AtMost(10), Is.EqualTo("."));
            Assert.That("12345".AtMost(10), Is.EqualTo("12345"));
            Assert.That("1234567890".AtMost(10), Is.EqualTo("1234567890"));
            Assert.That("12345678901234567890".AtMost(10), Is.EqualTo("1234567890"));
        }

        [Test]
        [SetCulture("en-US")]
        public void TestFillUpTo() {
            Assert.That(((string)null).FillUpTo(8), Is.Empty);
            Assert.That(string.Empty.FillUpTo(8), Is.Empty);
            Assert.That("Something".FillUpTo(0), Is.Empty);
            Assert.That("Something".FillUpTo(-10), Is.Empty);
            Assert.That("Something".FillUpTo(4), Is.EqualTo("Some"));
            Assert.That("Something".FillUpTo(9), Is.EqualTo("Something"));
            Assert.That("Something".FillUpTo(14), Is.EqualTo("SomethingSomet"));
            Assert.That("Something".FillUpTo(24), Is.EqualTo("SomethingSomethingSometh"));
            Assert.That("XO".FillUpTo(24), Is.EqualTo("XOXOXOXOXOXOXOXOXOXOXOXO"));
        }

        [Test]
        public void TestForEachLine() {
            CountEachLineIn(null, 0);
            CountEachLineIn(string.Empty, 0);
            CountEachLineIn("oneline", 1);
            CountEachLineIn("FirstLine\nSecondLine", 2);
            CountEachLineIn("FirstLine\n  \nThirdLine", 3);
            CountEachLineIn(@"FirstLine
SecondLine", 2);
            CountEachLineIn("FirstLine\r\nSecondLine", 2);
        }

        [Test]
        public void TestIsEmpty() {
            Assert.That(((string)null).IsEmpty(), Is.True);
            Assert.That(string.Empty.IsEmpty(), Is.True);
            Assert.That("  ".IsEmpty(), Is.False);
            Assert.That("Something".IsEmpty(), Is.False);
        }

        [Test]
        public void TestIsNotEmpty() {
            Assert.That(((string)null).IsNotEmpty(), Is.False);
            Assert.That(string.Empty.IsNotEmpty(), Is.False);
            Assert.That("  ".IsNotEmpty(), Is.True);
            Assert.That("Something".IsNotEmpty(), Is.True);
        }

        [Test]
        public void TestIsNotJustWhiteSpace() {
            Assert.That(((string)null).IsNotJustWhiteSpace(), Is.False);
            Assert.That(string.Empty.IsNotJustWhiteSpace(), Is.False);
            Assert.That("  ".IsNotJustWhiteSpace(), Is.False);
            Assert.That("\n  ".IsNotJustWhiteSpace(), Is.False);
            Assert.That("Something".IsNotJustWhiteSpace(), Is.True);
        }

        [Test]
        public void TestIsOnlyWhiteSpace() {
            Assert.That(((string)null).IsOnlyWhiteSpace(), Is.True);
            Assert.That(string.Empty.IsOnlyWhiteSpace(), Is.True);
            Assert.That("  ".IsOnlyWhiteSpace(), Is.True);
            Assert.That("Something".IsOnlyWhiteSpace(), Is.False);
        }

        [Test]
        public void TestNormalizeWhitespaceToSingleSpaces() {
            Assert.That(((string)null).NormalizeWhitespaceToSingleSpaces(), Is.Empty);
            Assert.That(string.Empty.NormalizeWhitespaceToSingleSpaces(), Is.Empty);
            Assert.That("  ".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo(" "));
            Assert.That("Something".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo("Something"));
            Assert.That("Somewhere over the rainbow".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo("Somewhere over the rainbow"));
            Assert.That("Somewhere   over   the   rainbow".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo("Somewhere over the rainbow"));
            Assert.That("Somewhere\tover\tthe\trainbow".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo("Somewhere over the rainbow"));
            Assert.That("Somewhere \u00a0over the rainbow".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo("Somewhere over the rainbow"));
            Assert.That(" Somewhere over the rainbow ".NormalizeWhitespaceToSingleSpaces(), Is.EqualTo(" Somewhere over the rainbow "));
        }

        [Test]
        public void TestPrefixWith() {
            const string prefix = "Prefix_";
            Assert.That(((string)null).PrefixWith(prefix), Is.EqualTo(prefix));
            Assert.That(string.Empty.PrefixWith(prefix), Is.EqualTo(prefix));
            Assert.That("  ".PrefixWith(prefix), Is.EqualTo(prefix));
            Assert.That("Something".PrefixWith(prefix), Is.EqualTo(prefix + "Something"));
            Assert.That("  Something  ".PrefixWith(prefix), Is.EqualTo(prefix + "Something"));
        }

        [Test]
        public void TestRepeat() {
            Assert.That(((string)null).Repeat(3), Is.Empty);
            Assert.That(string.Empty.Repeat(3), Is.Empty);
            Assert.That("  ".Repeat(3), Is.EqualTo("      "));
            Assert.That("-".Repeat(3), Is.EqualTo("---"));
            Assert.That("Something".Repeat(1), Is.EqualTo("Something"));
            Assert.That("Something".Repeat(3), Is.EqualTo("SomethingSomethingSomething"));
            Assert.That("-".Repeat(0), Is.Empty);
            Assert.That("-".Repeat(-10), Is.Empty);
        }

        [Test]
        public void TestSafeSplit() {
            Assert.That(((string)null).SafeSplit(';'), Is.Empty);
            Assert.That(("").SafeSplit(';'), Is.Empty);
            Assert.That(("    ").SafeSplit(';'), Is.Empty);
            Assert.That(("").SafeSplit(), Is.Empty);
            CompareStringEnumerable(("OnlyValue").SafeSplit(), "OnlyValue");
            CompareStringEnumerable(("OnlyValue").SafeSplit(';'), "OnlyValue");
            CompareStringEnumerable(("FirstValue;SecondValue").SafeSplit(';'), "FirstValue", "SecondValue");
        }

        [Test]
        public void TestSafeSplitTrimmed() {
            Assert.That(((string)null).SafeSplitTrimmed(';'), Is.Empty);
            Assert.That(("").SafeSplitTrimmed(';'), Is.Empty);
            Assert.That(("    ").SafeSplitTrimmed(';'), Is.Empty);
            Assert.That((" ;   ").SafeSplitTrimmed(';'), Is.Empty);
            Assert.That(("").SafeSplitTrimmed(), Is.Empty);
            CompareStringEnumerable(("OnlyValue").SafeSplitTrimmed(), "OnlyValue");
            CompareStringEnumerable(("OnlyValue").SafeSplitTrimmed(';'), "OnlyValue");
            CompareStringEnumerable((" OnlyValue ").SafeSplitTrimmed(';'), "OnlyValue");
            CompareStringEnumerable(("FirstValue;SecondValue").SafeSplitTrimmed(';'), "FirstValue", "SecondValue");
            CompareStringEnumerable((" FirstValue; SecondValue ;").SafeSplitTrimmed(';'), "FirstValue", "SecondValue");
            CompareStringEnumerable(("FirstValue\r\nSecondValue").SafeSplitTrimmed('\r', '\n'), "FirstValue", "SecondValue");
            CompareStringEnumerable(("First Value\r\nSecond Value").SafeSplitTrimmed('\r', '\n'), "First Value", "Second Value");
        }

        [Test]
        public void TestSplitAt() {
            Assert.Throws<ArgumentOutOfRangeException>(() => "".SplitAt(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => "".SplitAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => "".SplitAt(int.MinValue));
            CompareStringEnumerable(((string)null).SplitAt(2), string.Empty);
            CompareStringEnumerable(string.Empty.SplitAt(2), string.Empty);
            CompareStringEnumerable("AB".SplitAt(1), "A", "B");
            CompareStringEnumerable("AB".SplitAt(2), "AB");
            CompareStringEnumerable("ABC".SplitAt(2), "AB", "C");
            CompareStringEnumerable("ABCDEFG".SplitAt(2), "AB", "CD", "EF", "G");
        }

        [Test]
        public void TestStripWhitespace() {
            Assert.That(((string)null).StripWhitespace(), Is.Empty);
            Assert.That(string.Empty.StripWhitespace(), Is.Empty);
            Assert.That("  ".StripWhitespace(), Is.Empty);
            Assert.That("Something".StripWhitespace(), Is.EqualTo("Something"));
            Assert.That("Somewhere over the rainbow".StripWhitespace(), Is.EqualTo("Somewhereovertherainbow"));
            Assert.That("Somewhere   over   the   rainbow".StripWhitespace(), Is.EqualTo("Somewhereovertherainbow"));
            Assert.That("Somewhere\tover\tthe\trainbow".StripWhitespace(), Is.EqualTo("Somewhereovertherainbow"));
            Assert.That("Somewhere \u00a0over the rainbow".StripWhitespace(), Is.EqualTo("Somewhereovertherainbow"));
            Assert.That(" Somewhere over the rainbow ".StripWhitespace(), Is.EqualTo("Somewhereovertherainbow"));
        }

        [Test]
        public void TestToCamelCase() {
            Assert.That("singleword".ToCamelCase(), Is.EqualTo("singleword"));
            Assert.That("two words".ToCamelCase(), Is.EqualTo("twoWords"));
            Assert.That("two  spaces  between  words".ToCamelCase(), Is.EqualTo("twoSpacesBetweenWords"));
            Assert.That(" space before first word".ToCamelCase(), Is.EqualTo("spaceBeforeFirstWord"));
            Assert.That("spaces at the end    ".ToCamelCase(), Is.EqualTo("spacesAtTheEnd"));
            Assert.That("some\ttabs\tand\u00A0hard\u00A0spaces".ToCamelCase(), Is.EqualTo("someTabsAndHardSpaces"));
        }

        [Test]
        public void TestToPascalCase() {
            Assert.That("singleword".ToPascalCase(), Is.EqualTo("Singleword"));
            Assert.That("two words".ToPascalCase(), Is.EqualTo("TwoWords"));
            Assert.That("two  spaces  between  words".ToPascalCase(), Is.EqualTo("TwoSpacesBetweenWords"));
            Assert.That(" space before first word".ToPascalCase(), Is.EqualTo("SpaceBeforeFirstWord"));
            Assert.That("spaces at the end    ".ToPascalCase(), Is.EqualTo("SpacesAtTheEnd"));
            Assert.That("some\ttabs\tand\u00A0hard\u00A0spaces".ToPascalCase(), Is.EqualTo("SomeTabsAndHardSpaces"));
        }

        [Test]
        public void TestTransform() {
            Assert.That(((string)null).Transform(s => "a" + s + "b"), Is.Empty);
            Assert.That(string.Empty.Transform(s => "a" + s + "b"), Is.Empty);
            Assert.That((" ").Transform(s => "a" + s + "b"), Is.EqualTo("a b"));
            Assert.That(("x").Transform(s => "a" + s + "b"), Is.EqualTo("axb"));
        }

        [Test]
        public void TestTransformDefaulting() {
            Assert.That(((string)null).TransformDefaulting(s => "a" + s + "b"), Is.Null);
            Assert.That(string.Empty.TransformDefaulting(s => "a" + s + "b"), Is.Null);
            Assert.That((" ").TransformDefaulting(s => "a" + s + "b"), Is.EqualTo("a b"));
            Assert.That(("x").TransformDefaulting(s => "a" + s + "b"), Is.EqualTo("axb"));
        }

        [Test]
        public void TestTruncate() {
            Assert.That(((string)null).Truncate(5), Is.Null);
            Assert.That(string.Empty.Truncate(5), Is.Empty);
            Assert.That(("123").Truncate(5), Is.EqualTo("123"));
            Assert.That(("12345").Truncate(5), Is.EqualTo("12345"));
            Assert.That(("1234567890").Truncate(5), Is.EqualTo("12345"));
        }

        [Test]
        public void TestTryParseOrDefault() {
            Assert.That(((string)null).TryParseOrDefault(5), Is.EqualTo(5));
            Assert.That(string.Empty.TryParseOrDefault(5), Is.EqualTo(5));
            Assert.That("A".TryParseOrDefault(5), Is.EqualTo(5));
            Assert.That("6".TryParseOrDefault(5), Is.EqualTo(6));
            Assert.That("false".TryParseOrDefault(true), Is.False);
            Assert.That("true".TryParseOrDefault(false), Is.True);
        }

        [Test]
        public void TestWithDefault() {
            Assert.That(((string)null).WithDefault("default"), Is.EqualTo("default"));
            Assert.That(string.Empty.WithDefault("default"), Is.EqualTo("default"));
            Assert.That("  ".WithDefault("default"), Is.EqualTo("default"));
            Assert.That("A".WithDefault("default"), Is.EqualTo("A"));
        }

        [Test]
        public void TestWithDefaultSingle() {
            Assert.That(((string)null).WithDefault(), Is.Empty);
            Assert.That(string.Empty.WithDefault(), Is.Empty);
            Assert.That("  ".WithDefault(), Is.Empty);
            Assert.That("A".WithDefault(), Is.EqualTo("A"));
        }

        private enum OtherTestEnum
        {
            FirstValue, LastValue
        }

        private enum TestEnum
        {
            FirstValue, SecondValue, ThirdValue, LastValue
        }

        private static void CountEachLineIn(string text, int expected) {
            var count = 0;
            text.ForEachLine(line => count++);
            Assert.That(count, Is.EqualTo(expected));
        }
    }
}
