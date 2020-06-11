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
using System.Reflection;
using System.Text;
using Commons.Reflection;
using Commons.Text;
using Commons.Translation;
using static Commons.Translation.TranslationService;

namespace Commons
{
    public class AssemblyInformation
    {
        public AssemblyInformation(Assembly assembly) {
            ExeName = assembly.GetName().Name;
            Version = assembly.GetVersion();
            Title = assembly.GetAttributeValueAsString<AssemblyTitleAttribute>(a => a.Title);
            Copyright = assembly.GetAttributeValueAsString<AssemblyCopyrightAttribute>(a => a.Copyright);
            Description = assembly.GetAttributeValueAsString<AssemblyDescriptionAttribute>(a => a.Description);
            AboutDetails = assembly.AttributeToString<AboutAttribute>();
            AdditionalInfo = assembly.AttributeToString<AdditionalInfoAttribute>();
            ReportBugsTo = assembly.AttributeToString<ReportBugsToAttribute>();
            License = assembly.GetAttribute<LicenseAttribute>();
            Authors = GetAuthors(assembly);
            Company = assembly.GetAttributeValueAsString<AssemblyCompanyAttribute>(a => a.Company);
        }

        public string AboutDetails { get; set; }
        public string AdditionalBannerInfo { get; set; }
        public string AdditionalInfo { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public string Company { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string ExeName { get; set; }
        public LicenseAttribute License { get; set; }

        public string Product { get; set; }

        public string ReportBugsTo { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public AssemblyInformation WithDefaults {
            get {
                AboutDetails ??= "Add [assembly: Commons.About(\"Here goes the short about details\")] to your code";
                Copyright ??= $"Add [assembly: AssemblyCopyright(\"(c){DateTimeOffset.Now.Year:0000} Here goes the copyright holder name\")] to your code";
                Description ??= "Add [assembly: AssemblyDescription(\"Here goes the short description\")] to your code";
                Title ??= "Add [assembly: AssemblyTitle(\"Here goes the application name\")] to your code";
                Product ??= "Add [assembly: AssemblyProduct(\"Here goes the product/parent project name\")] to your code";
                Company ??= "Add [assembly: AssemblyCompany(\"Here goes the company name\")] to your code";
                Authors ??= (new string[] { "Add as many [assembly: Commons.Author(\"Here goes the author name\")] as authors to your code" });
                return this;
            }
        }

        public void ShowAbout([Translatable] string companyLabelOverride = null)
            => ShowStringBuiltWith(AppendBanner, AppendDescription, sb => AppendAuthors(sb, companyLabelOverride));

        public void ShowBanner() => ShowStringBuiltWith(AppendBanner);

        public void ShowFooter() => ShowStringBuiltWith(AppendFooter);

        public void ShowTitleLines() => ShowStringBuiltWith(AppendBanner, AppendDescription);

        public override string ToString() => BuildToString(sb => AppendAuthors(sb));

        public string ToString([Translatable] string companyLabelOverride) => BuildToString(sb => AppendAuthors(sb, companyLabelOverride));

        private static readonly string _nl = Environment.NewLine;

        private static string BuildStringWith(params Action<StringBuilder>[] appenders) {
            var sb = new StringBuilder();
            foreach (var builder in appenders)
                builder(sb);
            return sb.ToString();
        }

        private static string ChooseConnector(string line) => (line?.IndexOf('@') > 0) ? _("to") : _("at");

        private static IEnumerable<string> GetAuthors(Assembly assembly)
            => assembly.GetAttributes<AuthorAttribute>().Cast<AuthorAttribute>().SelectIfAny(a => a.Name);

        private static void ShowStringBuiltWith(params Action<StringBuilder>[] appenders) => Console.Write(BuildStringWith(appenders));

        private void AppendAuthors(StringBuilder sb, string companyLabelOverride = null) {
            sb.AppendLine(_(AboutDetails));
            sb.AppendLine(__($"Authors: {string.Join(", ", Authors)}"));
            sb.AppendLineIfNotNull(Company, $"{companyLabelOverride ?? _("Company")}: {Company}");
        }

        private void AppendBanner(StringBuilder sb) {
            sb.AppendLine($"{Title}  {Version} - {Copyright}");
            sb.AppendLineIfNotEmpty(AdditionalBannerInfo);
        }

        private void AppendDescription(StringBuilder sb) {
            sb.AppendLine(_(Description));
            sb.AppendLineIfNotNull(License, __($"{_nl}License: {License}"));
            sb.AppendLine();
        }

        private void AppendFooter(StringBuilder sb) {
            sb.AppendLineIfNotNull(AdditionalInfo, $"{_nl}{_(AdditionalInfo)}");
            sb.AppendLineIfNotNull(ReportBugsTo, __($"{_nl}Please report bugs {ChooseConnector(ReportBugsTo)} <{_(ReportBugsTo)}>"));
        }

        private string BuildToString(Action<StringBuilder> AuthorsAppender) => BuildStringWith(AppendBanner, AppendDescription, AuthorsAppender, AppendFooter);
    }
}
