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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons.Text
{
    public static class StringExtensions
    {
        public static bool AsBool(this string s) => TryParseOrDefault(s, false);

        public static byte[] AsBytes(this string s) => s.TransformDefaulting(Convert.FromBase64String) ?? _noBytes;

        public static T AsEnum<T>(this string s, T defaultValue) where T : struct, IConvertible => TryParseOrDefault(s, defaultValue);

        public static int AsInt(this string s) => TryParseOrDefault(s, (int)0);

        public static IEnumerable<string> AsLines(this string text) {
            if (text == null)
                yield break;
            using TextReader reader = new StringReader(text);
            foreach (string line in reader.Lines())
                yield return line;
        }

        public static long AsLong(this string s) => TryParseOrDefault(s, (long)0);

        public static byte[] AsUtf8Bytes(this string s) => s.TransformDefaulting(Encoding.UTF8.GetBytes) ?? _noBytes;

        public static string AtMost(this string s, int length) => s != null && length > 0 && s.Length > length ? s.Substring(0, length) : s;

        public static string FillUpTo(this string s, int width) {
            if (s.IsEmpty() || width <= 0)
                return string.Empty;
            var sb = new StringBuilder(width + s.Length);
            while (sb.Length < width) sb.Append(s);
            return sb.TruncateAt(width).ToString();
        }

        public static void ForEachLine(this string text, Action<string> process) {
            if (text.IsNotEmpty())
                new StringReader(text).ForEachLine(process);
        }

        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNotEmpty(this string s) => !string.IsNullOrEmpty(s);

        public static bool IsNotJustWhiteSpace(this string value) => !string.IsNullOrWhiteSpace(value);

        public static bool IsOnlyWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static string NormalizeWhitespaceToSingleSpaces(this string s) => Regex.Replace(s ?? string.Empty, @"\s+", " ");

        public static string PrefixWith(this string s, string prefix) => prefix + s.WithDefault().Trim();

        public static string Repeat(this string s, int repetitions) {
            if (s.IsEmpty() || repetitions <= 0)
                return string.Empty;
            var sb = new StringBuilder(s.Length * repetitions);
            while (repetitions-- > 0) sb.Append(s);
            return sb.ToString();
        }

        public static IEnumerable<string> SafeSplit(this string list, params char[] separators) {
            if (list.IsOnlyWhiteSpace())
                yield break;
            if (separators.Length == 0) {
                yield return list;
                yield break;
            }
            foreach (string item in list.Split(separators))
                yield return item;
        }

        public static IEnumerable<string> SafeSplitTrimmed(this string list, params char[] separators) => SafeSplit(list, separators).TrimAll().NoBlanks();

        public static IEnumerable<string> SplitAt(this string s, int length) {
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length));
            var list = new List<string>();
            if (s == null)
                s = string.Empty;
            else
                while (s.Length > length) {
                    list.Add(s.Substring(0, length));
                    s = s.Substring(length);
                }
            list.Add(s);
            return list;
        }

        public static string StripWhitespace(this string s) => Regex.Replace(s.WithDefault(), @"\s", string.Empty);

        public static string ToCamelCase(this string s) => FromSpacedWordsToCapitalizedWords(s, false);

        public static string ToPascalCase(this string s) => FromSpacedWordsToCapitalizedWords(s, true);

        public static string Transform(this string s, Func<string, string> transform) => s.IsEmpty() ? string.Empty : transform(s);

        public static T TransformDefaulting<T>(this string s, Func<string, T> transform) => s.IsEmpty() ? default : transform(s);

        public static string Truncate(this string s, int size) => SafeTruncate(s?.Trim(), size);

        public static T TryParseOrDefault<T>(this string s, T defaultValue) where T : IConvertible => s.IsOnlyWhiteSpace() ? defaultValue : ParseWithDefault<T>(s, defaultValue);

        public static string WithEnvironmentNewLines(this string verbatimString)
            => Regex.Replace(verbatimString.TrimStart('\r', '\n'), @"(\r\n|\r[^\n])", "\n").Replace("\n", Environment.NewLine);

        public static string WithDefault(this string s) => s.WithDefault(string.Empty);

        public static string WithDefault(this string s, string @default) => s.IsOnlyWhiteSpace() ? @default : s;

        internal static string TransformOrNot(this string s, Func<string, string> transform) => s.IsEmpty() ? s : (transform(s) ?? s);

        private static readonly byte[] _noBytes = new byte[0];

        private static string FromSpacedWordsToCapitalizedWords(string s, bool pascalCasing) => s.Transform(ss => ss.Rebuild(sb => sb.FromSpacedWordsToCapitalizedWords(ss.Trim(), pascalCasing)));

        private static bool IsEnum<T>() where T : IConvertible => typeof(T).IsEnum;

        private static T ParseEnumIgnoringCase<T>(string s, T defaultValue) where T : IConvertible => !Enum.GetNames(typeof(T)).Any(name => name.Equals(s, StringComparison.OrdinalIgnoreCase))
                ? defaultValue
                : (T)Enum.Parse(typeof(T), s, true);

        private static T ParseWithDefault<T>(string s, T defaultValue)
          where T : IConvertible {
            try {
                return IsEnum<T>() ? ParseEnumIgnoringCase(s, defaultValue) : (T)(Convert.ChangeType(s, typeof(T)));
            } catch (Exception) {
                return defaultValue;
            }
        }

        private static string Rebuild(this string s, Action<StringBuilder> magic) => new StringBuilder(s.Length).ToString(magic);

        private static string SafeTruncate(string s, int size) => s.IsEmpty() || s.Length <= size ? s : s.Substring(0, size);
    }
}
