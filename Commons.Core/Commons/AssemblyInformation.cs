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
using static Commons.Translation.TranslationService;

namespace Commons
{
    public class AssemblyInformation
    {
        public AssemblyInformation(Assembly assembly)
        {
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
        }

        public string AboutDetails { get; set; }
        public string AdditionalBannerInfo { get; set; }
        public string AdditionalInfo { get; set; }
        public IEnumerable<string> Authors { get; set; }
        public string Copyright { get; set; }
        public string Description { get; set; }
        public string ExeName { get; set; }
        public static AssemblyInformation FromEntryAssembly => new AssemblyInformation(Assembly.GetEntryAssembly());
        public LicenseAttribute License { get; set; }

        public string Product { get; set; }

        public string ReportBugsTo { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public AssemblyInformation WithDefaults
        {
            get
            {
                AboutDetails = AboutDetails ?? "Add a [assembly: Commons.About(\"Here goes the short about details\")] to your assembly";
                Copyright = Copyright ?? $"Add a [assembly: AssemblyCopyright(\"(c){DateTimeOffset.Now.Year:0000} Here goes the copyright holder name\")] to your assembly";
                Description = Description ?? "Add a [assembly: AssemblyDescription(\"Here goes the short description\")] to your assembly";
                Title = Title ?? "Add a [assembly: AssemblyTitle(\"Here goes the application name\")] to your assembly";
                Product = Product ?? "Add a [assembly: AssemblyProduct(\"Here goes the product/parent project name\")] to your assembly";
                if (Authors == null) {
                    var authors = new String[1];
                    authors[0] = "Add [assembly: AssemblyCompany(\"Here goes the authors' names, separated by commas\")] to your assembly";
                    Authors = authors;
                }
                return this;
            }
        }

        public void ShowAbout()
        {
            ShowStringBuiltWith(AppendBanner, AppendDescription, AppendAuthors);
        }

        public void ShowBanner()
        {
            ShowStringBuiltWith(AppendBanner);
        }

        public void ShowFooter()
        {
            ShowStringBuiltWith(AppendFooter);
        }

        public void ShowTitleLines()
        {
            ShowStringBuiltWith(AppendBanner, AppendDescription);
        }

        public override string ToString() => BuildStringWith(AppendBanner, AppendDescription, AppendAuthors, AppendFooter);

        static string BuildStringWith(params Action<StringBuilder>[] appenders)
        {
            var sb = new StringBuilder();
            foreach (var builder in appenders)
                builder(sb);
            return sb.ToString();
        }

        static string ChooseConnector(string line) => (line?.IndexOf('@') > 0) ? _("to") : _("at");

        static IEnumerable<string> GetAuthors(Assembly assembly)
        {
            var company = assembly.GetAttributeValueAsString<AssemblyCompanyAttribute>(a => a.Company);
            if (!string.IsNullOrWhiteSpace(company))
                return company.Split(',').Select(s => s.Trim());
            return null;
        }

        static void ShowStringBuiltWith(params Action<StringBuilder>[] appenders)
        {
            Console.Write(BuildStringWith(appenders));
        }

        void AppendAuthors(StringBuilder sb)
        {
            sb.AppendLine(_(AboutDetails));
            sb.AppendLine(__($"Authors: {string.Join(", ", Authors)}"));
        }

        void AppendBanner(StringBuilder sb)
        {
            sb.AppendLine($"{Title}  {Version} - {Copyright}");
            sb.AppendLineIfNotEmpty(AdditionalBannerInfo);
        }

        void AppendDescription(StringBuilder sb)
        {
            sb.AppendLine(_(Description));
            sb.AppendLineIfNotNull(License, __($"\r\nLicense: {License}"));
            sb.AppendLine();
        }

        void AppendFooter(StringBuilder sb)
        {
            sb.AppendLineIfNotNull(AdditionalInfo, $"\r\n{_(AdditionalInfo)}");
            sb.AppendLineIfNotNull(ReportBugsTo, __($"\r\nPlease report bugs {ChooseConnector(ReportBugsTo)} <{_(ReportBugsTo)}>"));
        }
    }
}
