using Newtonsoft.Json;
using passwordmanager.models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                return JsonConvert.DeserializeObject<List<User>>
                    (File.ReadAllText(path));
            }

            public void Save(List<User> users)
            {
                File.WriteAllText(path,
                    JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented));
            }
        }
}
