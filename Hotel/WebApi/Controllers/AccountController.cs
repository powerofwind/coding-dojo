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
        private static List<AllAccount> allAccount = new List<AllAccount>();


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
        public IEnumerable<AccountInfo> Get()
        {
            return accounts;
        }

        // PUT api/account
        [HttpPut]
        public EditResult Put([FromBody] AccountInfo data)
        {
            //เช็คid ว่ามีidที่ต้องการแก้ไขข้อมูลไหม
            var isEditAccount = accounts.Any(it => it.Id == data.Id);

            if (isEditAccount)
            {
                //เช็คก่อนว่า user/pass ที่กรอกเข้ามาใหม่ใช้ได้ไหม (ต้องไม่ตรงกับที่มีอยู่ และ มี.length มากกว่า4)
                if (data.Username.Length < 4 || data.Password.Length < 4)
                {
                    return new EditResult
                    {
                        IsSuccess = false,
                        Message = "Username หรือ Password ต้องไม่ต่ำกว่า 4 ตัวอักษร"
                    };
                }

                var isDuplicate = accounts.Any(it => it.Username == data.Username);
                if (isDuplicate)
                {
                    return new EditResult
                    {
                        IsSuccess = false,
                        Message = "Username มีอยู่ในระบบแล้ว"
                    };
                }

                //ลบข้อมูลจาก id
                var findAccount = accounts.FirstOrDefault(it => it.Id == data.Id);
                accounts.Remove(findAccount);

                //ใส่ค่าที่แก้ไข
                accounts.Add(data);
                return new EditResult
                {
                    IsSuccess = true,
                    Message = "ระบบได้ทำการบันทึกข้อมูลเรียบร้อยแล้ว"
                };
            }

            return new EditResult
            {
                IsSuccess = false,
                Message = "ไม่มีบัญชีนี้ในระบบ"
            };
        }

        // Delete api/account/{id}
        [HttpDelete("{id}")]
        public DeleteResult Delete(string id)
        {
            //เช็คid ว่ามีidที่ต้องการลบข้อมูลไหม
            var isDeletedAccount = accounts.Any(it => it.Id == id);

            if (isDeletedAccount)
            {
                 //ลบข้อมูลจาก id
                var findAccount = accounts.FirstOrDefault(it => it.Id == id);
                accounts.Remove(findAccount);
                return new DeleteResult
                {
                    IsSuccess = true,
                    Message = "บัญชีของคุณถูกลบออกจากระบบเรียบร้อยแล้ว"
                };
            }
            else
            {
                return new DeleteResult
                {
                    IsSuccess = false,
                    Message = "id ไม่ถูกต้อง"
                };
            }
        }
    }
}
