using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// tile字典
/// </summary>
[System.Serializable]
public class TileDictionary : SerializableDictionaryBase<TileType, Tile>{}
/// <summary>
/// 基础菜单字典
/// </summary>
[System.Serializable]
public class BaseMenuDictionary : SerializableDictionaryBase<BaseMenuType, Sprite>{}