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

            response.Success = true;
            response.Message = "模拟时间已成功更新。";
        }
        catch (Exception)
        {
            response.Success = false;
            response.Message = "内部错误"; 
        }

        return response; 
    }
}