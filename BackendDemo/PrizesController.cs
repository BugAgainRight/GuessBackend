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
            // 加载奖品数据  
            Storage.LoadPrizes();
        }
 
        [HttpGet]
        [Route("api/prizes/info")]
        public IHttpActionResult Info([FromUri] string account)
        {
            // 检查用户是否存在  
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
            if (user == null)
            {
                return NotFound(); // 返回404
            }

            // 准备奖品列表  
            var prizeList = new PrizeList() 
            { 
                Prizes = Storage.PrizeData.Prizes.Select(x =>
                {
                    return new Prize()
                    {
                        Redeemed = user.RedeemedPrizes.Contains(x.ID),
                        ID = x.ID,
                        Name = x.Name,
                        PointsRequired = x.PointsRequired,
                        Stock = x.Stock
                    };
                }).ToList()
            };

            // 返回成功响应  
            return Ok(prizeList);
        }

        [HttpGet]
        [Route("api/prizes/redeem")]
        public IHttpActionResult Redeem([FromUri] string account, [FromUri] string id)
        {
            var prizeStatus = new PrizeStatusData();

            // 检查用户是否存在  
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
            if (user == null)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "用户未找到";
                return Ok(prizeStatus);
            }

            // 检查用户地址是否填写  
            if (string.IsNullOrEmpty(user.Address))
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "请填写地址以完成兑奖";
                return Ok(prizeStatus);
            }

            // 根据 ID 查找奖品  
            var prize = Storage.PrizeData.Prizes.FirstOrDefault(p => p.ID == id);
            if (prize == null)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "奖品未找到";
                return Ok(prizeStatus);
            }

            // 检查库存  
            if (prize.Stock <= 0)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "该奖品库存不足";
                return Ok(prizeStatus);
            }

            // 检查奖品是否已兑换  
            if (user.RedeemedPrizes.Contains(prize.ID))
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "该奖品已被兑换";
                return Ok(prizeStatus);
            }

            // 检查用户积分是否足够  
            if (user.Points < prize.PointsRequired)
            {
                prizeStatus.Success = false;
                prizeStatus.Message = "积分不足，无法兑换该奖品";
                return Ok(prizeStatus);
            }

            // 进行奖品的兑换  
            user.RedeemedPrizes.Add(prize.ID); // 标记为已兑换  
            prize.Stock--; // 库存减少  
            user.Points -= prize.PointsRequired; // 更新用户积分（减少相应积分）  
            
            Storage.SavePrizes();
            Storage.SaveChanges();

            // 返回成功状态  
            prizeStatus.Success = true;
            prizeStatus.Message = "奖品兑换成功！";
            return Ok(prizeStatus);
        }
    }
}
