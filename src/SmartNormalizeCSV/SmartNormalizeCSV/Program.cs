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
                List<string> lineList = new List<string>();
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

                    lineList.Add(line);
                }

                StringBuilder stringBuilder = new StringBuilder();

                foreach (string item in lineList)
                {
                    string column = string.Join(" ;", item.Split(';').Select((value, index) => value.PadRight(major[index], ' ')));

                    stringBuilder.AppendLine(column);
                }

                lineList.Clear();

                using (StreamWriter streamWriter = new StreamWriter(@"C:\tmp\normalized.txt"))
                {
                    streamWriter.Write(stringBuilder.ToString());
                }

                stringBuilder.Clear();
            }
        }
    }
}
