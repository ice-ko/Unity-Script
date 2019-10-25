using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Model
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
