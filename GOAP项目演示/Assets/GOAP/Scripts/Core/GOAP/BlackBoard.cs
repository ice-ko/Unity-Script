using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 黑板
/// </summary>
public class BlackBoard {
    Dictionary<string,object> _data =new Dictionary<string, object>();
    Dictionary<string, object[]> _datas = new Dictionary<string, object[]>();

    public void AddData(string key, object val)
    {
        if (!_data.ContainsKey(key))
            _data.Add(key, val);
        else
            _data[key] = val;
    }

    public object GetData(string key)
    {
        if (_data.ContainsKey(key))
            return _data[key];
        return null;
    }
    public void AddDatas(string key, object[] val)
    {
        if (!_datas.ContainsKey(key))
            _datas.Add(key, val);
        else
            _datas[key] = val;
    }

    public object[] GetDatas(string key)
    {
        if (_datas.ContainsKey(key))
            return _datas[key];
        return null;
    }
}
