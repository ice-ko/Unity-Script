using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 用户信息
/// </summary>
[Serializable]
public class UserInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; }
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}