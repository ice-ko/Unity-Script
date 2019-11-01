using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Model
{
    /// <summary>
    /// 用户角色信息
    /// </summary>
    public class UserCharacterInfo:UserInfoDto
    {
        /// <summary>
        /// 豆子数量
        /// </summary>
        public int Been { get; set; } = 10000;
        /// <summary>
        /// 胜利场次
        /// </summary>
        public int WinCount { get; set; }
        /// <summary>
        /// 失败场次
        /// </summary>
        public int LoseCount { get; set; }
        /// <summary>
        /// 逃跑场次
        /// </summary>
        public int RunCount { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Lv { get; set; } = 1;
        /// <summary>
        /// 经验
        /// </summary>
        public int Exp { get; set; }
    }
}
