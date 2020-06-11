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

namespace Commons.Text
{
    public static class IEnumerableOfStringExtensions
    {
        public static IEnumerable<string> ExceptThese(this IEnumerable<string> list, params string[] exceptions) => list.Except(exceptions);

        public static string JoinWith(this IEnumerable<string> itens, string connector) => string.Join(connector ?? string.Empty, itens);

        public static IEnumerable<string> NoBlanks(this IEnumerable<string> list) => list.Where(s => !s.IsOnlyWhiteSpace());

        public static IEnumerable<string> ToLower(this IEnumerable<string> list) => list.Select(s => s == null ? string.Empty : s.ToLowerInvariant());

        public static IEnumerable<string> ToUpper(this IEnumerable<string> list) => list.Select(s => s == null ? string.Empty : s.ToUpperInvariant());

        public static IEnumerable<string> TrimAll(this IEnumerable<string> list) => list.Select(s => s == null ? string.Empty : s.Trim());
    }

    public static class IEnumerableOfTExtensions
    {
        public static IEnumerable<T> NoNulls<T>(this IEnumerable<T> list) => list.Safe().Where(it => !(it is null));

        public static IEnumerable<T> Safe<T>(this IEnumerable<T> list) => list is null ? Enumerable.Empty<T>() : list;

        public static bool None<T>(this IEnumerable<T> list) => !list.NoNulls().Any();
        public static IEnumerable<TS> SelectIfAny<T, TS>(this IEnumerable<T> list, Func<T, TS> selector)
            => list.None() ? null : list.Select(selector);
    }
}
