using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LoadSceneMsg
{
    /// <summary>
    /// 场景索引
    /// </summary>
    public int SceneIndex { get; set; } = -1;
    /// <summary>
    /// 场景名称
    /// </summary>
    public string SceneName { get; set; }
    /// <summary>
    /// 委托
    /// </summary>
    public Action OnSceneLoaded { get; set; }
}