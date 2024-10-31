using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using static BackendDemo.Storage;

namespace BackendDemo
{

    public partial class UserController : ApiController
    {

        [HttpGet]
        public UserInfo Info([FromUri] string account)
        {
            try
            {
                var user = Storage.Instance.Users.First(x => x.Account == account);
                var response = new UserInfo()
                {
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Points = user.Points
                };

                return response;
            }
            catch(Exception)
            {
                var Erro = new UserInfo();
                Erro.Name = "NO FOUND!";
                Erro.PhoneNumber = "NO FOUND!";
                Erro.Address = "NO FOUND!";
                return Erro;
            }
        }
        [HttpPost]
        public StatusData Updateinfo([FromBody] UpdateInfoRequest Info)
        {
            var response = new StatusData();
            try
            {
                var user = Storage.Instance.Users.First(x => x.Account == Info.Account);
                user.Name=Info.Name;
                user.Address=Info.Address;
                user.PhoneNumber=Info.PhoneNumber;

            }
            catch(Exception)
            {
                response.Success = false;
                response.Message = "未查找到用户";
                return response;

            }
            Storage.SaveChanges();

            response.Success = true;
            response.Message = "更新成功";

            return response;
        }

    }

}

