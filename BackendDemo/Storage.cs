using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendDemo
{
    internal static class Storage
    {
        [Serializable]
        public class Data
        {
            public List<User> Users = new();
            public DateTime SimulatedTime;
        }

        [Serializable]
        public class User
        {
            public string Account;
            public string Password;
            public string IDNumber;
            public string Name;
            public string PhoneNumber;
            public string Address;
            public double Points;
            public List<Guess> Guesses = new();
        }

        [Serializable]
        public class Guess
        {
            public string EventID;
            public int GuessWinner;
            public bool IsSettled;
        }

        public static Data Instance;

        public static void LoadFromFile()
        {
            if (File.Exists("userdata.json"))
            {
                Instance = JsonConvert.DeserializeObject<Data>(File.ReadAllText("userdata.json"))!;
            }
            else
            {
                Instance = new Data();
            }
        }

        public static void SaveChanges()
        {
            File.WriteAllText("userdata.json", JsonConvert.SerializeObject(Instance));
        }
    }
}
