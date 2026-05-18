using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TechnologistWpf.Services
{
    public static class ExportHelper
    {
        public static void ExportToCsv<T>(IEnumerable<T> data, string filePath)
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties();
            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));
            foreach (var item in data)
            {
                sb.AppendLine(string.Join(",", props.Select(p => p.GetValue(item)?.ToString() ?? "")));
            }
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}