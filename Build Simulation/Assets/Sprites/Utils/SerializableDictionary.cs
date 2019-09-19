using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// tile
/// </summary>
[System.Serializable]
public class TileDictionary : SerializableDictionaryBase<TileType, Tile>{}
/// <summary>
/// 基础菜单
/// </summary>
[System.Serializable]
public class BaseMenuDictionary : SerializableDictionaryBase<MenuType, Sprite>{}

/// <summary>
/// 矿石sprite
/// </summary>
[System.Serializable]
public class OreDictionary : SerializableDictionaryBase<OreType, Sprite> { }
/// <summary>
/// 木材sprite
/// </summary>
[System.Serializable]
public class TreeDictionary : SerializableDictionaryBase<TreeType, Sprite> { }
/// <summary>
/// 工作量
/// </summary>
[System.Serializable]
public class WorkloadDictionary: SerializableDictionaryBase<MenuType, MaterialInfo> { }
