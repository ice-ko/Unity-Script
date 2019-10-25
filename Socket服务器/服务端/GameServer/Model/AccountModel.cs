using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model
{
    /// <summary>
    /// 账号的数据模型
    /// </summary>
    public class AccountModel
    {
        public int Id;
        public string Account;
        public string Password;

        //...创建日期 电话号码

        public AccountModel(int id, string account, string password)
        {
            this.Id = id;
            this.Account = account;
            this.Password = password;
        }
    }
}
