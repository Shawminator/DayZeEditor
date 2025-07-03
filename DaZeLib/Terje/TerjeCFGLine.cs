using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DayZeLib
{
    public class TerjeCFGLine
    {
        public bool isComment { get; set; }
        public string comment { get; set; }
        public string cfgtype { get; set; }
        public string cfgvariablename { get; set; }
        public string cfgvariabletype { get; set; }
        public object cfgVariable { get; set; }

        public TerjeCFGLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }
            else if (line.TrimStart().StartsWith("//"))
            {
                isComment = true;
                comment = line;
            }
            else
            {
                isComment = false;
                ParseLine(line);
            }
        }

        private void ParseLine(string line)
        {
            // Regex to extract the config value, comment and metadata
            var regex = new Regex(@"^(.*?)=(.*?);[ \t]*//[ \t]*(.*)$");
            var match = regex.Match(line);

            if (match.Success)
            {
                cfgvariablename = match.Groups[1].Value.Trim().Split('.')[1];
                string valueStr = match.Groups[2].Value.Trim();
                comment = match.Groups[3].Value.Trim(); // Full comment including [type: ...]
                cfgtype = match.Groups[1].Value.Trim().Contains(".") ? match.Groups[1].Value.Trim().Split('.')[0] : "Unknown";

                // Optionally extract cfgvariabletype from the comment using a separate regex:
                var typeMatch = Regex.Match(comment, @"\[type:\s*(\w+)", RegexOptions.IgnoreCase);
                cfgvariabletype = typeMatch.Success ? typeMatch.Groups[1].Value.Trim() : "string";

                // Convert the value
                cfgVariable = ConvertValue(valueStr, cfgvariabletype);
            }
            else
            {
                throw new FormatException("Line does not match expected format: " + line);
            }
        }
        public string Formatline()
        {
            if (isComment)
            {
                return comment;
            }

            string valueStr = FormatValue(cfgVariable, cfgvariabletype);
            return $"{cfgtype}.{cfgvariablename} = {valueStr}; // {comment}";
        }
        private object ConvertValue(string value, string type)
        {
            switch (type.ToLower())
            {
                case "int":
                    return int.Parse(value);

                case "float":
                    return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

                case "bool":
                    return bool.Parse(value);

                case "string":
                    return value.Trim('"');

                default:
                    throw new NotSupportedException($"Unsupported config variable type: {type}");
            }
        }
        private string FormatValue(object value, string type)
        {
            switch (type.ToLower())
            {
                case "float":
                    return ((float)value).ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
                case "int":
                    return value.ToString();
                case "bool":
                    return value.ToString().ToLower(); // "true"/"false" lowercase
                case "string":
                    return $"\"{value}\"";
                default:
                    throw new NotSupportedException($"Unsupported config variable type: {type}");
            }
        }
    }
}
