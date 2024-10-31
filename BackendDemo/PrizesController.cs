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

        [HttpGet]
        [Route("api/prizes/redeem")]
        public IHttpActionResult RedeemPrize(string account, string id)
        {
            var prizeStatus = new PrizeStatusData();

            // 检查用户是否存在  
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
            if (user == null)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "用户未找到";
                return NotFound();
            }

            // 检查用户地址是否填写  
            if (string.IsNullOrEmpty(user.Address))
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "请填写地址以完成兑奖";
                return BadRequest("false");
            }

            // 根据 ID 查找奖品  
            var prize = Storage.PrizeData.Prizes.FirstOrDefault(p => p.ID == id);
            if (prize == null)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "奖品未找到";
                return NotFound();
            }

            // 检查库存  
            if (prize.Stock <= 0)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "该奖品库存不足";
                return BadRequest("false");
            }

            // 检查奖品是否已兑换  
            if (prize.Redeemed)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "该奖品已被兑换";
                return BadRequest("false");
            }

            // 检查用户积分是否足够  
            if (user.Points < prize.PointsRequired)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "积分不足，无法兑换该奖品";
                return BadRequest("false");
            }

            // 进行奖品的兑换  
            prize.Redeemed = true; // 标记为已兑换  
            prize.Stock--; // 库存减少  
            user.Points -= prize.PointsRequired; // 更新用户积分（减少相应积分）  

            // 返回成功状态  
            prizeStatus.Success = true;
            prizeStatus.Message = "奖品兑换成功！";
            return Ok();
        }
    }
}
