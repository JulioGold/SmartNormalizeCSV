using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SmartNormalizeCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(@"C:\tmp\source.csv"))
            {
                List<string> lista = new List<string>();
                Dictionary<int, int> major = new Dictionary<int, int>();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Dictionary<int, int> currentFields = line.Split(';')
                        .Select((value, index) => new { value, index })
                        .ToDictionary(pair => pair.index, pair => pair.value.Length);

                    if (major.Count < 1)
                    {
                        major = currentFields.ToDictionary(p => p.Key, p => p.Value);
                    }
                    else
                    {
                        foreach (var item in currentFields)
                        {
                            if (item.Value > major[item.Key])
                            {
                                major[item.Key] = item.Value;
                            }
                        }
                    }

                    lista.Add(line);
                }

                StringBuilder sb = new StringBuilder();

                foreach (var item in lista)
                {
                    var col = string.Join(" ;", item.Split(';').Select((value, index) => value.PadRight(major[index], ' ')));

                    sb.AppendLine(col);
                }

                lista.Clear();

                using (StreamWriter sw = new StreamWriter(@"C:\tmp\normalized.txt"))
                {
                    sw.Write(sb.ToString());
                }

                sb.Clear();
            }
        }
    }
}
