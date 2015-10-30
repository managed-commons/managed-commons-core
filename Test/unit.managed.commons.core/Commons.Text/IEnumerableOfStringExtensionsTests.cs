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
using NUnit.Framework;
using static Commons.Commons.Text.TestUtils;

namespace Commons.Text.Tests
{
    [TestFixture]
    public class IEnumerableOfStringExtensionsTests
    {
        [Test]
        public void TestExceptThese()
        {
            CompareStringEnumerable(ToArray("A", "B", "C").ExceptThese("B", "C"), "A");
            CompareStringEnumerable(ToArray("A", "B", "C", "D", "E", "F").ExceptThese("B", "C"), "A", "D", "E", "F");
            CompareStringEnumerable(ToArray("A", "B", "C", "D", "E", "F").ExceptThese("B", "E"), "A", "C", "D", "F");
            CompareStringEnumerable(ToArray("A", "B", "C", "D", "E", "F").ExceptThese("X", "Y", "Z"), "A", "B", "C", "D", "E", "F");
            CompareStringEnumerable(ToArray("A", "B", "C", "D", "E", "F").ExceptThese("a", "b", "c"), "A", "B", "C", "D", "E", "F");
            CompareStringEnumerable(ToArray("a", "b", "c", "d", "e", "f").ExceptThese("a", "b", "c"), "d", "e", "f");
        }

        [Test]
        public void TestJoinWith()
        {
            Assert.AreEqual("A_B_C", ToArray("A", "B", "C").JoinWith("_"));
            Assert.AreEqual("ABC", ToArray("A", "B", "C").JoinWith(null));
            Assert.AreEqual("ABC", ToArray("A", "B", "C").JoinWith(""));
            Assert.AreEqual("A B C", ToArray("A", "B", "C").JoinWith(" "));
        }

        [Test]
        public void TestNoBlanks()
        {
            CompareStringEnumerable(ToArray("A", "B", "C").NoBlanks(), "A", "B", "C");
            CompareStringEnumerable(ToArray("A", "   ", "C").NoBlanks(), "A", "C");
            CompareStringEnumerable(ToArray("A", null, "C").NoBlanks(), "A", "C");
        }

        [Test]
        public void TestToLower()
        {
            CompareStringEnumerable(ToArray("A", "B", "C").ToLower(), "a", "b", "c");
            CompareStringEnumerable(ToArray("A", "", "C").ToLower(), "a", "", "c");
            CompareStringEnumerable(ToArray("A", null, "C").ToLower(), "a", "", "c");
        }

        [Test]
        public void TestToUpper()
        {
            CompareStringEnumerable(ToArray("a", "b", "c").ToUpper(), "A", "B", "C");
            CompareStringEnumerable(ToArray("a", "", "c").ToUpper(), "A", "", "C");
            CompareStringEnumerable(ToArray("a", null, "c").ToUpper(), "A", "", "C");
        }

        [Test]
        public void TestTrimAll()
        {
            CompareStringEnumerable(ToArray("A", "B", "C").TrimAll(), "A", "B", "C");
            CompareStringEnumerable(ToArray(" A", "B ", "C").TrimAll(), "A", "B", "C");
            CompareStringEnumerable(ToArray("A   ", "B", "   C").TrimAll(), "A", "B", "C");
            CompareStringEnumerable(ToArray("A", "   ", "C").TrimAll(), "A", "", "C");
            CompareStringEnumerable(ToArray("A", null, "C").TrimAll(), "A", "", "C");
        }
    }
}
