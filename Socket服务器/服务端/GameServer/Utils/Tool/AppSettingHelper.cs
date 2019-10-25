using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Configuration;

namespace Utils.Tool
{
    /// <summary>
    ///读取配置信息类
    /// </summary>
    public class AppSettingHelper
    {
        public static IConfiguration Configuration { get; set; }
        static AppSettingHelper()
        {
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        static public string MysqlConnection => Configuration["DefaultDatabasConnection"];
        /// <summary>
        /// ip地址
        /// </summary>
        static public string IpAddress => Configuration["IP"];
    }
}
