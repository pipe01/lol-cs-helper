using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2
{
    public class Configuration
    {
        public int WindowSyncInterval { get; set; } = 500;
        public int MinimumRefreshInterval { get; set; } = 2000;
        public bool DebugDraw { get; set; } = false;

        public void SaveToFile(string path)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented,
                new JsonConverter[] { new StringEnumConverter() });

            File.WriteAllText(path, json);
        }

        public static Configuration LoadFromFile(string path)
        {
            if (!File.Exists(path))
                return null;

            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
        }
    }
}
