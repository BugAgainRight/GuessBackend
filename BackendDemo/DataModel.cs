using System;
using System.Collections.Generic;
using System.Drawing;

[Serializable]
public class RegisterData{
    public string Account;
    public string Password;
    public string IDNumber; // 身份证ID
}

[Serializable]
public class StatusData{
    public bool Success;
    public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}

[Serializable]
public class LoginData{
    public string Account;
    public string Password;
}

[Serializable]
public class LoginResponse{
    public string ID; // 注册时后端给用户分配ID，登陆时返回
    public bool Success;
    public string Message;
}

[Serializable]
public class UserInfo{
    public string Name;
    public string PhoneNumber;
    public string Address;
    public double Points;
}

[Serializable]
public class UpdateInfoRequest{
    public string Account;
    public string Name;
    public string PhoneNumber;
    public string Address;
}

[Serializable]
public class EventList{
    public List<Event> Events = new List<Event>();
}

[Serializable]
public class Event{
    public string ID;
    public string Name;	// 比赛名称
    public DateTime EventTime, StartGuessTime, EndGuessTime;
    public string[] PartyANames, PartyBNames; // 队伍A的所有成员的名字，队伍B的所有成员的名字（如果是单人则单人）
    public string PartyACountry, PartyBCountry; // 国家A的名称，国家B的名称
    public int Winner; // 获胜方，如果在竞赛结束之前，则返回-1，竞猜结束后返回0或者1
}

[Serializable]
public class GuessList{
    public List<GuessData> Guesses = new();
}

[Serializable]
public class GuessData{
    public string EventID;
    public int GuessWinner;
    public bool IsSettled;
}

[Serializable]
public class EventGuessData
{
    public int[] GuessCount;    // 当前比赛全服 猜A赢的总人数 和 猜B赢的总人数 （数组，2个元素）
    public int UserGuess;       // 当前用户的猜测选择，未猜测=-1，猜A=0，猜B=1
}

[Serializable]
public class TimeData
{
    public DateTime ServerTime;
}

[Serializable]
public class PrizeList
{
    public List<Prize> Prizes = new();
}

[Serializable]
public class Prize
{
    public string ID;
    public string Name;
    public int Stock;//剩余库存数量
    public double PointsRequired;//兑换一件所需要的积分
    public bool Redeemed;//true表示用户已兑换，false表示用户未兑换
}

[Serializable]
public class PrizeStatusData
{
    public bool Success;
    public string Message;//积分是否足够，地址是否填写，兑换物是否已兑换，库存是否充足
}

[Serializable]
public class MessageList
{
    public List<MessageData> Messages = new();
}
[Serializable]
public class MessageData
{
    public string ID = Guid.NewGuid().ToString();
    public string Content;
    public DateTime Time;
}
