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
using Commons.Translation;
using static Commons.Translation.TranslationService;

namespace Commons
{
    public enum LicenseType
    {
        Proprietary,
        AGPL3,
        Apache2,
        BSD3Clause,
        BSD2Clause,
        GPL2,
        GPL3,
        LGPL21,
        LGPL3,
        MIT,
        Mozilla2,
        CDDL,
        Eclipse,
        Other
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class LicenseAttribute : Attribute
    {
        public LicenseAttribute(LicenseType licenseType) {
            if (licenseType == LicenseType.Other)
                throw new ArgumentOutOfRangeException(nameof(licenseType), "The license type should not be 'Other', use the alternate constructor");
            var license = _licenses[licenseType];
            LicenseType = licenseType;
            Name = license.Name;
            Details = license.Details;
            DetailsUrl = license.Url;
            IsProprietary = licenseType == LicenseType.Proprietary;
        }

        public LicenseAttribute(string name, string detailsUrl, [Translatable] string details, bool isProprietary) {
            LicenseType = isProprietary ? LicenseType.Proprietary : LicenseType.Other;
            Name = name;
            DetailsUrl = detailsUrl;
            Details = details;
            IsProprietary = isProprietary;
        }

        public string Details { get; }

        public string DetailsUrl { get; }

        public bool IsProprietary { get; }

        public LicenseType LicenseType { get; }

        public string Name { get; }

        public string TranslatedDetails => __($"See {DetailsUrl}") + _(Details);

        public override string ToString() => string.IsNullOrWhiteSpace(DetailsUrl) ? Name : (Name + " - " + TranslatedDetails);

        private static readonly Dictionary<LicenseType, LicenseDescriptor> _licenses = new Dictionary<LicenseType, LicenseDescriptor> {
            [LicenseType.Proprietary] = new LicenseDescriptor("Proprietary", null),
            [LicenseType.AGPL3] = new LicenseDescriptor("GNU Affero General Public License, Version 3 (AGPL-3.0)", "https://opensource.org/licenses/AGPL-3.0"),
            [LicenseType.Apache2] = new LicenseDescriptor("Apache License, Version 2.0", "https://opensource.org/licenses/Apache-2.0"),
            [LicenseType.BSD3Clause] = new LicenseDescriptor("BSD 3-Clause License", "https://opensource.org/licenses/BSD-3-Clause"),
            [LicenseType.BSD2Clause] = new LicenseDescriptor("BSD 2-Clause License", "https://opensource.org/licenses/BSD-2-Clause"),
            [LicenseType.GPL2] = new LicenseDescriptor("GNU General Public License, Version 2 (GPL-2.0)", "https://opensource.org/licenses/GPL-2.0"),
            [LicenseType.GPL3] = new LicenseDescriptor("GNU General Public License, Version 3 (GPL-3.0)", "https://opensource.org/licenses/GPL-3.0"),
            [LicenseType.LGPL21] = new LicenseDescriptor("GNU Lesser General Public License, Version 2.1 (LGPL-2.1)", "https://opensource.org/licenses/LGPL-2.1"),
            [LicenseType.LGPL3] = new LicenseDescriptor("GNU Lesser General Public License, Version 3.0 (LGPL-3.0)", "https://opensource.org/licenses/LGPL-3.0"),
            [LicenseType.MIT] = new LicenseDescriptor("MIT License", "https://opensource.org/licenses/MIT"),
            [LicenseType.Mozilla2] = new LicenseDescriptor("Mozilla Public License 2.0 (MPL-2.0)", "https://opensource.org/licenses/MPL-2.0"),
            [LicenseType.CDDL] = new LicenseDescriptor("COMMON DEVELOPMENT AND DISTRIBUTION LICENSE Version 1.0 (CDDL-1.0)", "https://opensource.org/licenses/CDDL-1.0"),
            [LicenseType.Eclipse] = new LicenseDescriptor("Eclipse Public License, Version 1.0 (EPL-1.0)", "https://opensource.org/licenses/EPL-1.0")
        };

        private struct LicenseDescriptor
        {
            public readonly string Details;
            public readonly string Name;
            public readonly string Url;

            public LicenseDescriptor(string name, string url, string details = null) {
                Name = name; Url = url; Details = details;
            }
        }
    }
}
