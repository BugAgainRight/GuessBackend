using Newtonsoft.Json;
using System.Web.Http;

namespace BackendDemo;



public class TimeController : ApiController
{
    [HttpGet]
    public StatusData Set([FromUri] DateTime time)
    {
        var response = new StatusData();

        try
        {
            Storage.Instance.SimulatedTime = time;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MatchData.json");
            string json = File.ReadAllText(path);
            var eventList = JsonConvert.DeserializeObject<EventList>(json)!;
            //遍历所有赛事,检查竞猜是否需要结算
            foreach (var match in eventList.events) {
                if (Storage.Instance.SimulatedTime >= match.EndGuessTime && match.Winner != -1)
                {
                    List<Storage.User> correctUsers = new();
                    int incorrectGuessCount = 0;
                    //遍历所有用户,计算竞猜结果
                    foreach (var user in Storage.Instance.Users)
                    {
                        // 筛选当前用户中符合条件的竞猜
                        var unsettledGuesses = user.Guesses.Where(g => g.EventID == match.ID && !g.IsSettled).ToList();

                        foreach (var guess in unsettledGuesses)
                        {
                            if (guess.GuessWinner == match.Winner)
                            {
                                correctUsers.Add(user); // 记录猜对的用户
                            }
                            else
                            {
                                user.Points -= 1;      // 猜错的用户扣 1 分
                                incorrectGuessCount++; // 记录总扣分
                            }
                            guess.IsSettled = true;    // 标记竞猜已结算
                        }
                    }
                    //平均分配给猜对的用户
                    if (correctUsers.Count > 0 && incorrectGuessCount > 0) {
                        double pointsPerCorrectUser = (double)incorrectGuessCount / correctUsers.Count;
                        foreach (var correctUser in correctUsers)
                        {
                            correctUser.Points += pointsPerCorrectUser;
                        }
                    }
                }
            }
            Storage.SaveChanges();
            response.Success = true;
            response.Message = "模拟时间已成功更新,竞猜已结算";
        }
        catch (Exception)
        {
            response.Success = false;
            response.Message = "内部错误"; 
        }

        return response; 
    }
    public TimeData Get() { 
        var response = new TimeData();
        response.ServerTime = Storage.Instance.SimulatedTime;
        return response;
    }
}