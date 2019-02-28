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

        // GET api/account
        [HttpGet]
        public IEnumerable<GetAccountsResult> Get()
        {
            //ใช้ select เพราะต้องการเปลี่ยนของใน account ซึ่งมีโมเดลเป็น AccountInfo ให้เป็น GetAccountsResult
            return accounts.Select(it => new GetAccountsResult
            {
                Id = it.Id,
                Username = it.Username
            });
        }

        // PUT api/account
        [HttpPut]
        public EditResult Put([FromBody] AccountInfo data)
        {
            //หาไอดีว่ามีไอดีนั้นมั้ย
            var findID = accounts.FirstOrDefault(it => it.Id == data.Id);
            if (findID == null)
            {
                return new EditResult
                {
                    IsSuccess = false,
                    Message = "ไม่พบบัญชีที่เลือก"
                };
            }

            findID.Password = data.Password;
            findID.Username = data.Username;
            return new EditResult
            {
                IsSuccess = true,
                Message = "ข้อมูลได้ถูกแก้ไขแล้ว"
            };
        }

        // Delete api/account/{id}
        [HttpDelete("{id}")]
        public DeleteResult Delete(string id)
        {
            //เช็คid ว่ามีidที่ต้องการลบข้อมูลไหม
            var findAccount = accounts.FirstOrDefault(it => it.Id == id);

            if (findAccount == null)
            {
                return new DeleteResult
                {
                    IsSuccess = false,
                    Message = "ไม่พบบัญชีที่เลือก"
                };
            }

            //ลบข้อมูลจาก id
            accounts.Remove(findAccount);
            return new DeleteResult
            {
                IsSuccess = true,
                Message = "บัญชีของคุณถูกลบออกจากระบบเรียบร้อยแล้ว"
                //  Message = string.Empty
            };

        }
    }
}

