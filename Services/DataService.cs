using passwordmanager.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace passwordmanager.Services
{
    public class DataService
    {
        private string path = Path.Combine(
     AppDomain.CurrentDomain.BaseDirectory,
     "Data",
     "data.json");

        public List<PasswordItem> Load()
        {
            if (!File.Exists(path))
                return new List<PasswordItem>();

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<PasswordItem>();

            return JsonSerializer.Deserialize<List<PasswordItem>>(json)
                   ?? new List<PasswordItem>();
        }

        public void Save(List<PasswordItem> items)
        {
            Directory.CreateDirectory(
    Path.GetDirectoryName(path));
            string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}
