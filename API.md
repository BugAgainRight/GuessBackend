# POST /api/register

注册账号

body:

```
public class RegisterData{
	public string Account;
	public string Password;
	public string IDNumber; // 身份证ID
}
```

返回：

```
public class StatusData{
	public bool Success;
	public string Message; // 错误信息，如果Success为false，则在这里返回原因，例如“当前身份证已被注册！”
}
```

# POST /api/login

登录账号

```
public class LoginData{
	public string Account;
	public string Password;
}
```

返回：

```
public class LoginResponse{
	public string ID; // 注册时后端给用户分配ID，登陆时返回
	public bool Success;
	public string Message;
}
```

# GET /api/userinfo

获取用户信息

参数：string id

返回：

```
public class UserInfo{
	public string Name;
	public string PhoneNumber;
	public string Address;
}
```

# POST /api/updateinfo

更新用户信息

```
public class UpdateInfoRequest{
	public string Name;
	public string PhoneNumber;
	public string Address;
}
```

返回：

```
public class StatusData{
	public bool Success;
}
```

# GET /api/eventlist

获取全部比赛信息

无参数。

返回：

```
public class EventList{
	public List<Event> Events = new();
}

public class Event{
	public string ID;
	public string Name;	// 比赛名称
	public DateTime EventTime, StartGuessTime, EndGuessTime;
	public string[] PartyANames, PartyBNames; // 队伍A的所有成员的名字，队伍B的所有成员的名字（如果是单人则单人）
	public string PartyACountry, PartyBCountry; // 国家A的名称，国家B的名称
	public int Winner; // 获胜方，如果在竞赛结束之前，则返回-1，竞猜结束后返回0或者1
}
```

# GET /api/guess

竞猜

参数：string id：比赛ID, int winner：猜测的获胜方（0=A方，1=B方）

返回：

```
public class StatusData{
	public bool Success;
}
```

# GET /api/guesslist

获取当前用户竞猜列表

参数：string userID：用户ID

返回：

```
public class GuessList{
	public List<GuessData> Guesses = new();
}

public class GuessDataa{
	public string EventID;
	public int GuessWinner;
}
```



