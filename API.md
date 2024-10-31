# POST /api/user/register

注册账号

body:

```Plaintext
public class RegisterData{
        public string Account;
        public string Password;
        public string IDNumber; // 身份证ID
}
```

返回：

```Plaintext
public class StatusData{
        public bool Success;
        public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}
```

# POST /api/user/login

登录账号

```Plaintext
public class LoginData{
        public string Account;
        public string Password;
}
```

返回：

```Plaintext
public class LoginResponse{
        public bool Success;
        public string Message;
}
```

# GET /api/user/info

获取用户信息

参数：string account：账号名

返回：

```C#
public class UserInfo{
        public string Name;
        public string PhoneNumber;
        public string Address;
        public double Points;    // 积分数量
}
```

# POST /api/user/updateinfo

更新用户信息

```C#
public class UpdateInfoRequest{
        public string Account;
        public string Name;
        public string PhoneNumber;
        public string Address;
}
```

返回：

```C#
public class StatusData{
        public bool Success;
        public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}
```

# GET /api/event/list

获取全部比赛信息

无参数。

返回：

```Plaintext
public class EventList{
        public List<Event> Events = new();
}

public class Event{
        public string ID;
        public string Name;        // 比赛名称
        public DateTime EventTime, StartGuessTime, EndGuessTime;
        public string[] PartyANames, PartyBNames; // 队伍A的所有成员的名字，队伍B的所有成员的名字（如果是单人则单人）
        public string PartyACountry, PartyBCountry; // 国家A的名称，国家B的名称
        public int Winner; // 获胜方，如果在竞赛结束之前，则返回-1，竞猜结束后返回0或者1
}
```

# GET /api/guess/confirm

竞猜

参数：string account：发起竞猜的账号，string id：比赛ID, int winner：猜测的获胜方（0=A方，1=B方）

返回：

```Plaintext
public class StatusData{
        public bool Success;
        public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}
```

# GET /api/guess/list

获取当前用户竞猜列表

参数：string userID：账户名

返回：

```Plaintext
public class GuessList{
        public List<GuessData> Guesses = new();
}

public class GuessDataa{
        public string EventID;
        public int GuessWinner;
}
```

# GET /api/guess/event

获取比赛的竞猜情况

参数：string account：账号，string eventID：比赛ID

返回：

```Plain
public class EventGuessData{
    public int[] GuessCount;    // 当前比赛全服 猜A赢的总人数 和 猜B赢的总人数 （数组，2个元素）
    public int UserGuess;       // 当前用户的猜测选择，未猜测=-1，猜A=0，猜B=1
}
```

# GET /api/time/set

更改服务器的“模拟时间”

参数：DateTime time：时间

返回：

```C++
public class StatusData{
        public bool Success;
        public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}
```

# GET /api/time/get

获取服务器的“模拟时间”

无参数。

返回：

```C++
public class TimeData{
        public DateTime ServerTime;
}
```

# GET /api/prizes/info

获取兑换物列表

参数：string account

返回

```C#
public class PrizeList{
    public List<Prize> Prizes = new();
}
public class Prize{
    public string ID;
    public string Name;
    public int Stock;//剩余库存数量
    public double PointsRequired;//兑换一件所需要的积分
    public bool Redeemed;//true表示用户已兑换，false表示用户未兑换
}
```

# GET /api/prizes/redeem

申请兑换奖品

参数：string account：用户名，string ID：想兑换的奖品ID

返回

```C#
public class PrizeStatusData{
    public bool Success;
    public string Message;//积分是否足够，地址是否填写，兑换物是否已兑换，库存是否充足
}
```

# GET /api/user/messages

获取用户的全部消息通知

参数：string account：用户名

返回

```C#
public class MessageList{
    public List<MessageData> Messages = new();
}
    
public class MessageData{
    public string ID = Guid.NewGuid().ToString();
    public string Content;
    public DateTime Time;
}
```
