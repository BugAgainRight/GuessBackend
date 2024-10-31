using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static BackendDemo.Storage;
namespace BackendDemo
{
    public partial class UserController : ApiController
    {
        [HttpPost]
        [Route("api/user/register")]
        public IHttpActionResult Register([FromBody] RegisterData registerData)
        {
            if(registerData == null)
            {
                return BadRequest("无效数据");
            }

            if (Storage.Instance.Users.Any(u => u.Account == registerData.Account))
            {
                return Ok(new StatusData
                {
                    Success = false,
                    Message = "已有其他用户使用这个用户名了，换个用户名吧。"
                });
            }

            if (Storage.Instance.Users.Any(u => u.IDNumber == registerData.IDNumber))
            {
                return Ok(new StatusData
                {
                    Success = false,
                    Message = "当前身份证已被注册！"
                });
            }
            //注册新用户
            var newUser = new Storage.User
            {
                Account = registerData.Account,
                Password = registerData.Password,
                IDNumber = registerData.IDNumber,
                Points = 100.0,
                Name = "用户" + registerData.Account // 默认名称
            };

            Storage.Instance.Users.Add(newUser);
            Storage.SaveChanges();//保存更改

            return Ok(new StatusData
            {
                Success = true,
                Message = "注册成功！"
            });
        }
        [HttpPost]
        [Route("api/user/login")]
        public IHttpActionResult Login([FromBody] LoginData loginData)
        {
            if (loginData == null)
            {
                return BadRequest("无效数据");
            }

            //检查用户信息是否匹配
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == loginData.Account && u.Password == loginData.Password);

            if (user == null)
            {
                return Ok(new LoginResponse
                {
                    Success = false,
                    Message = "账号或密码错误！"
                });
            }

            return Ok(new LoginResponse
            {
                ID = user.IDNumber,
                Success = true,
                Message = "登录成功！"
            });
        }
        [HttpGet]
        public MessageList Messages([FromUri] string account)
        {
            var response= new MessageList();                
            var user = Storage.Instance.Users.FirstOrDefault(u => u.Account == account);
            if (user != null)
            {
                response.Messages = user.Messages;
            }
            else
            {
                response.Messages = new List<MessageData>();
            }

            return response;
        }
    }
}
