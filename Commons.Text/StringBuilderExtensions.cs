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

namespace Commons.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb, string value)
        {
            if (sb != null && !string.IsNullOrWhiteSpace(value))
                sb.AppendLine(value);
            return sb;
        }

        public static StringBuilder AppendLineIfNotNull(this StringBuilder sb, object item, string text = null)
        {
            if (item != null)
                sb.AppendLineIfNotEmpty(text ?? item.ToString());
            return sb;
        }

        public static StringBuilder AppendLinesWithIndentAndSeparator(this StringBuilder sb, IEnumerable<string> items, string indent, char separator = ',')
        {
            foreach (var text in items)
                sb.AppendLine($"{indent}{text}{separator}");
            return sb;
        }

        public static StringBuilder TrimLastSeparatorPreservingWhitespace(this StringBuilder sb, char separator = ',')
        {
            for (int j = sb.Length - 1; j > 0; j--) {
                var c = sb[j];
                if (!char.IsWhiteSpace(c)) {
                    if (c == separator)
                        sb.Remove(j, 1);
                    break;
                }
            }
            return sb;
        }
    }
}
