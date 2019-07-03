using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FurnitureSpriteController : MonoBehaviour
{

    public World world { get { return WorldController.instance.world; } }

    public Dictionary<Furniture, GameObject> FurnitureGameObjectMap;
    public Dictionary<string, Sprite> furnitureSprite;
    void Start()
    {
        LoadSprites();
        //
        FurnitureGameObjectMap = new Dictionary<Furniture, GameObject>();

        //注册委托
        world.RegisterFurnitureCreated(OnFurnitureCreated);
    }
    void LoadSprites()
    {
        //加载Sprite
        furnitureSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furniture/Wall");
        foreach (var item in sprites)
        {
            furnitureSprite.Add(item.name, item);
        }
    }
    /// <summary>
    /// 创建Furniture
    /// </summary>
    /// <param name="obj"></param>
    public void OnFurnitureCreated(Furniture obj)
    {
        // 创建链接到此数据的可视GameObject。

        // FIXME：不考虑多瓦片对象也不考虑旋转对象

        //这会创建一个新的GameObject并将其添加到我们的场景中。
        GameObject obj_go = new GameObject();
        //将我们的tile / GO对添加到字典中。
        FurnitureGameObjectMap.Add(obj, obj_go);

        obj_go.name = obj.objectType + "_" + obj.tile.x + "_" + obj.tile.y;
        obj_go.transform.position = new Vector3(obj.tile.x, obj.tile.y, 0);
        obj_go.transform.parent = transform;
        // FIXME：我们假设对象必须是墙，所以请 
        //添加SpriteRenderer
        var sr = obj_go.AddComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForFurniture(obj);
        sr.sortingLayerName = "Wall";
        //注册我们的回调，以便我们的GameObject随时更新
        obj.RegisterOnChangedCallback(OnFurnitureChanged);
    }
    /// <summary>
    /// 获取已安装对象的精灵
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    Sprite GetSpriteForFurniture(Furniture obj)
    {
        if (!obj.linksToNeighbour)
        {
            return furnitureSprite[obj.objectType];
        }

        var spriteName = obj.objectType + "_";
        //判断方向
        int x = obj.tile.x; int y = obj.tile.y;
        Tile tile;
        tile = world.GetTileAt(x, y + 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "N";
        }
        tile = world.GetTileAt(x + 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "E";
        }
        tile = world.GetTileAt(x, y - 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "S";
        }
        tile = world.GetTileAt(x - 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == obj.objectType)
        {
            spriteName += "W";
        }
        if (!furnitureSprite.ContainsKey(spriteName))
        {
            return null;
        }
        return furnitureSprite[spriteName];


    }
    /// <summary>
    /// 替换指定的tile sprite
    /// </summary> 
    /// <param name="furniture"></param>
    void OnFurnitureChanged(Furniture furniture)
    {
        if (!FurnitureGameObjectMap.ContainsKey(furniture))
        {
            return;
        }
        GameObject obj_go = FurnitureGameObjectMap[furniture];
        obj_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furniture);
    }
    /// <summary>
    /// 获取Sprite
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    public Sprite GetSpriteFurniture(string objectType)
    {

        if (furnitureSprite.ContainsKey(objectType))
        {
            return furnitureSprite[objectType];
        }
        if (furnitureSprite.ContainsKey(objectType + "_"))
        {
            return furnitureSprite[objectType + "_"];
        }
        return null;
    }
}
