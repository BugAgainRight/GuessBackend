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

                // 检查竞猜是否已经结算
                if (match.Winner != -1)
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
}


