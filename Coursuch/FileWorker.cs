using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursuch
{
    class FileWorker
    {
        public static bool ckeckFile(string filename)
        {
            return File.Exists(filename);
        }

        public static void creadeDirrectory(string directory) {
            Directory.CreateDirectory(directory);
        }

        public static void write(string filename, object data)
        {
            string text = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filename, text);
        }

        public static void append(string filename, object data)
        {
            string text = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.AppendAllText(filename, text);
        }

        public static T read<T>(string filename)
        {
            string text = File.ReadAllText(filename);

            T result = JsonConvert.DeserializeObject<T>(text);

            return result;
        }
    }
}