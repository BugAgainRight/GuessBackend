using System.Web.Http;

namespace BackendDemo;

// 请求结构体，由前端发送，后端接收
public class GetRandomRequest
{
    public int MinValue, MaxValue;
}

// 响应结构体，由后端发回给前端
public class GetRandomResponse
{
    public int Result;
}

// 继承ApiController，这个类将提供 http://localhost:7595/api/test/xxx 的内容的响应
// 如果类名是 BarController，则就是 http://localhost:7595/api/bar/xxx
public class TestController : ApiController
{
    // 定义一个 HTTP 请求的响应函数
    [HttpPost]  // 声明这个请求必须是 POST 请求，如果是 GET 请求就用 HttpGet
    // 参数的 [FromBody] 表示从HTTP请求的Body解析参数（也就是前端发送请求的时候要把这个参数的序列化成json放在Body里）
    public GetRandomResponse Random([FromBody] GetRandomRequest request) 
    {
        var response = new GetRandomResponse()
        {
            Result = new Random().Next(request.MinValue, request.MaxValue)
        };
        // 这里的response就是发回给前端的内容，前端看到的内容就是 这个 东西 序列化成 json 以后的文本
        return response;
    }
}