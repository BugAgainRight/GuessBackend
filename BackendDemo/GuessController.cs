using Newtonsoft.Json;
using System.Web.Http;

namespace BackendDemo;

    public class GuessController : ApiController
    {
        [HttpGet]
        public StatusData Confirm([FromUri] string account, [FromUri] string id, [FromUri] int winner)
        {
            var response = new StatusData();

            try
            {
                // 查找发起竞猜的用户
                var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "用户不存在。";
                    return response;
                }

                // 查找比赛
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchData.json");
                var eventList = JsonConvert.DeserializeObject<EventList>(File.ReadAllText(path))!;
                var match = eventList.Events.FirstOrDefault(e => e.ID == id);
                if (match == null)
                {
                    response.Success = false;
                    response.Message = "比赛不存在。";
                    return response;
                }

                if (Storage.Instance.SimulatedTime < match.StartGuessTime)
                {
                    response.Success = false;
                    response.Message = "竞猜尚未开始";
                    return response;
                }

                // 检查竞猜是否已经结算
                if (Storage.Instance.SimulatedTime >= match.EventTime)
                {
                    response.Success = false;
                    response.Message = "竞猜已经结束";
                    return response;
                }

                // 记录用户的竞猜
                var guess = new Storage.Guess
                {
                    EventID = id,
                    GuessWinner = winner,
                    IsSettled = false // 初始为未结算
                };
                user.Guesses.Add(guess);
                Storage.SaveChanges();
                response.Success = true;
                response.Message = "竞猜成功。";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"内部错误: {ex.Message}";
            }

            return response;
        }

    [HttpGet]
    public GuessList List([FromUri] string userID)
    {
        var guessList = new GuessList();
        var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == userID);
        if (user == null)
        {
                return guessList;
        }
        guessList.Guesses = user.Guesses.Select(g => new GuessData
        {
            EventID = g.EventID,
            GuessWinner = g.GuessWinner,
            IsSettled = g.IsSettled
        }).ToList();
        
        return guessList;
    }

    [HttpGet]
    public EventGuessData Event([FromUri] string account, [FromUri] string eventID)
    {
        var eventGuessData = new EventGuessData
        {
            GuessCount = new int[2] { 0, 0 }, // [猜A人数, 猜B人数]
            UserGuess = -1 // 默认值表示未猜测
        };

        var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
        if (user != null)
        {
            var userGuess = user.Guesses.FirstOrDefault(g => g.EventID == eventID);
            if (userGuess != null)
            {
                eventGuessData.UserGuess = userGuess.GuessWinner; 
            }
        }

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchData.json");
        var json = File.ReadAllText(path);
        var eventList = JsonConvert.DeserializeObject<EventList>(json)!;

        // 统计猜A和猜B的人数
        var match = eventList.Events.FirstOrDefault(e => e.ID == eventID);
        if (match != null)
        {
            foreach (var  users in Storage.Instance.Users)
            {
                foreach (var guess in users.Guesses)
                {
                    if (guess.EventID == eventID)
                    {
                        if (guess.GuessWinner == 0) // 猜A
                        {
                            eventGuessData.GuessCount[0]++;
                        }
                        else if (guess.GuessWinner == 1) // 猜B
                        {
                            eventGuessData.GuessCount[1]++;
                        }
                    }
                }
            }
        }

        return eventGuessData;
    }


}


