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
            foreach (var match in eventList.Events) {
                if (Storage.Instance.SimulatedTime >= match.EndGuessTime && match.Winner != -1)
                {
                    List<Storage.User> correctUsers = new();
                    List<Storage.User> msgSent = new();
                    int incorrectGuessCount = 0;
                    //遍历所有用户,计算竞猜结果
                    Dictionary<Storage.User, double> initialPointsMap = new();
                    foreach (var user in Storage.Instance.Users)
                    {
                        initialPointsMap[user] = user.Points;
                        // 筛选当前用户中符合条件的竞猜
                        var unsettledGuess = user.Guesses.FirstOrDefault(g => g.EventID == match.ID && !g.IsSettled);
                        if (unsettledGuess == null)
                        {
                            continue;
                        }

                        if (unsettledGuess.GuessWinner == match.Winner)
                        {
                            correctUsers.Add(user); // 记录猜对的用户
                        }
                        else
                        {
                            user.Points -= 1;      // 猜错的用户扣 1 分
                            incorrectGuessCount++; // 记录总扣分
                        }
                        unsettledGuess.IsSettled = true;    // 标记竞猜已结算
                        msgSent.Add(user);
                    }
                    //平均分配给猜对的用户
                    if (correctUsers.Count > 0 && incorrectGuessCount > 0) {
                        double pointsPerCorrectUser = (double)incorrectGuessCount / correctUsers.Count;
                        foreach (var correctUser in correctUsers)
                        {
                            correctUser.Points += pointsPerCorrectUser;
                        }
                    }
                    // 添加消息内容
                    foreach (var user in msgSent)
                    {
                        double initialPoints = initialPointsMap[user];
                        double pointsChange = user.Points - initialPoints; // 计算积分变化

                        // 生成消息内容
                        string messageContent = $"{match.Name} 比赛已结算，您" +
                                                (user.Guesses.Any(g => g.EventID == match.ID && g.GuessWinner == match.Winner) ? "猜中了" : "猜错了") +
                                                $"，积分变化：{pointsChange:F2}";

                        if (pointsChange == 0)
                        {
                            messageContent += "，由于没有人猜错，本次竞猜无积分流动。";
                        }

                        user.Messages.Add(new MessageData
                        {
                            Content = messageContent,
                            Time = DateTime.Now,
                        });
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