using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace GameServer.Model
{
    ///<summary>
    ///用户表
    ///</summary>
    public partial class UaseInfo
    {
           public UaseInfo(){


           }
        /// <summary>
        /// Desc:用户标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id {get;set;}

           /// <summary>
           /// Desc:账号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Account { get;set;}

           /// <summary>
           /// Desc:密码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Password {get;set;}

           /// <summary>
           /// Desc:登录标识
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Token {get;set;}

    }
}
