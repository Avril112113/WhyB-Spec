using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GenerateSpec
{
    public static class Extensions
    {
        public static void AppendLine(this StringBuilder sb, string line, int indent_count)
        {
            sb.AppendLine($"{new string('\t', indent_count)}{line}");
        }

        public static string ToPascalCase(this string s)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(s).Replace(" ", string.Empty).Replace("_", string.Empty);
        }

        public static string ToCamelCase(this string s)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            string pc = myTI.ToTitleCase(s).Replace(" ", string.Empty).Replace("_", string.Empty);
            return char.ToLower(pc[0]) + pc[1..];
        }

        public static string ToSnakeCase(this string s)
        {
            return string.Concat(s.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string Indent(this string s, int indent, string suffix=null)
        {
            return Regex.Replace(s, "\r\n|\n\r?", $"{Environment.NewLine}{new string('\t', indent)}{suffix}");
        }
    }
}
