using projectapp.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace projectapp.Services
{
    public class DataService
    {
        private string path = "Data/data.json";

        public List<PasswordItem> Load()
        {
            if (!File.Exists(path))
                return new List<PasswordItem>();

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<PasswordItem>>(json);
        }

        public void Save(List<PasswordItem> items)
        {
            Directory.CreateDirectory("Data");
            string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
