using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackendDemo
{
    public class PrizesController :ApiController
    {
        public PrizesController()
        {
            // 加载用户数据  
            Storage.LoadFromFile();
            // 加载奖品数据  
            Storage.LoadPrizes();
        }
 
        [HttpGet]
        [Route("api/prizes/info")]
        public IHttpActionResult GetPrizeList(string account)
        {
            // 检查用户是否存在  
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
            if (user == null)
            {
                return NotFound(); // 返回404
            }

            // 准备奖品列表  
            var prizeList = Storage.PrizeData;
            // 返回成功响应  
            return Ok(prizeList);
        }
    }
}
