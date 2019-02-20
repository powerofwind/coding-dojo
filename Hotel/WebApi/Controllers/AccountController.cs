using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static List<AccountInfo> accounts = new List<AccountInfo>();

        // POST api/account
        [HttpPost]
        public RegisterResult Post([FromBody] AccountInfo req)
        {
            if (req.Username.Length < 4 || req.Password.Length < 4)
            {
                return new RegisterResult
                {
                    IsSuccess = false,
                    Message = "Username หรือ Password ต้องไม่ต่ำกว่า 4 ตัวอักษร"
                };
            }

            var isDupplicate = accounts
                .Where(it => it.Username == req.Username)
                .Any();
            if (isDupplicate)
            {
                return new RegisterResult
                {
                    IsSuccess = false,
                    Message = "Username มีอยู่ในระบบแล้ว"
                };
            }

            req.Id = Guid.NewGuid().ToString();
            accounts.Add(req);
            return new RegisterResult
            {
                IsSuccess = true,
                Message = "Register เสร็จสิ้น"
            };
        }

        // POST api/account/login
        [HttpPost("login")]
        public LoginResult Login([FromBody] AccountInfo req)
        {
            // var isFound = accounts
            //     .Where(it => it.Username == req.Username)
            //     .Where(it => it.Password == req.Password)
            //     .Any();

            var isFound = accounts
                .Any(it => it.Username == req.Username && it.Password == req.Password);

            return new LoginResult
            {
                IsSuccess = isFound,
                Message = isFound ? "Login สำเร็จ" : "Username หรือ Password ไม่ถูก"
            };
        }
    }
}
