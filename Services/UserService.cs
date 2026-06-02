using Newtonsoft.Json;
using passwordmanager.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;


namespace passwordmanager.Services
{
    public class UserService
    {
        private readonly string path = "users.json";

        public List<User> Load()
        {
            if (!File.Exists(path))
                return new List<User>();

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<User>();

            return JsonConvert.DeserializeObject<List<User>>(json)
                   ?? new List<User>();
        }

        public void Save(List<User> users)
        {
            File.WriteAllText(
                path,
                JsonConvert.SerializeObject(
                    users,
                    Newtonsoft.Json.Formatting.Indented));
        }
    }
}
