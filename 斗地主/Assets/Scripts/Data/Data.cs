using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 所有数据
/// </summary>
public static class Data
{
    public static GameData GameData;
    static Data()
    {
        GameData = new GameData();
    }
}
