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
            public List<string> RedeemedPrizes = new();
            public List<MessageData> Messages = new();
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

        //增加对prize的读取写入
        [Serializable]
        public class Prize
        {
            public string ID;
            public string Name;
            public int Stock;
            public double PointsRequired;
        }

        [Serializable]
        public class PrizeList
        {
            public List<Prize> Prizes = new();
        }

        public static PrizeList PrizeData;

        public static void LoadPrizes()
        {
            if (File.Exists("PrizeData.json"))
            {
                PrizeData = JsonConvert.DeserializeObject<PrizeList>(File.ReadAllText("PrizeData.json"))!;
            }
            else
            {
                PrizeData = new PrizeList(); // 如果文件不存在，创建一个新的奖品列表  
            }
        }

        public static void SavePrizes()
        {
            File.WriteAllText("PrizeData.json", JsonConvert.SerializeObject(PrizeData));
        }
    }
}
