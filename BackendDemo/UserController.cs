using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
namespace BackendDemo
{
    public class UserController : ApiController
    {
        static UserController()
        {
            Storage.LoadFromFile();
        }
        [HttpPost]
        [Route("api/user/register")]
        public IHttpActionResult Register([FromBody] RegisterData registerData)
        {
            if(registerData == null)
            {
                return BadRequest("无效数据");
            }
            if(Storage.Instance.Users.Any(u => u.IDNumber == registerData.IDNumber))
            {
                return Ok(new StatusData
                {
                    Success = false
                });
            }
            //注册新用户
            var newUser = new Storage.User
            {
                Account = registerData.Account,
                Password = registerData.Password,
                IDNumber = registerData.IDNumber
            };

            Storage.Instance.Users.Add(newUser);
            Storage.SaveChanges();//保存更改

            return Ok(new StatusData
            {
                Success = true
            });
        }

    }
}
