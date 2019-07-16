using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FurnitureSpriteController : MonoBehaviour
{

    public World world { get { return WorldController.instance.world; } }

    public Dictionary<Furniture, GameObject> furnitureGameObjectMap;
    public Dictionary<string, Sprite> furnitureSprite;
    void Start()
    {
        LoadSprites();
        //
        furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();

        //注册委托
        world.RegisterFurnitureCreated(OnFurnitureCreated);

        foreach (Furniture item in world.furnituresList)
        {
            OnFurnitureCreated(item);
        }
    }
    void LoadSprites()
    {
        //加载Sprite
        furnitureSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Furniture");
        foreach (var item in sprites)
        {
            furnitureSprite.Add(item.name, item);
        }
    }
    /// <summary>
    /// 创建Furniture
    /// </summary>
    /// <param name="furn"></param>
    public void OnFurnitureCreated(Furniture furn)
    {
        // 创建链接到此数据的可视GameObject。
        GameObject furn_go = new GameObject();
        //将我们的tile / GO对添加到字典中。
        furnitureGameObjectMap.Add(furn, furn_go);

        furn_go.name = furn.objectType + "_" + furn.tile.x + "_" + furn.tile.y;
        furn_go.transform.position = new Vector3(furn.tile.x, furn.tile.y, 0);
        furn_go.transform.parent = transform;

        if (furn.objectType == "Door")
        {
            //上边的tile
            Tile upTile = world.GetTileAt(furn.tile.x, furn.tile.y + 1);
            //下边的tile
            Tile downTile = world.GetTileAt(furn.tile.x, furn.tile.y - 1);
            if (upTile != null && downTile != null && upTile.furniture != null && downTile.furniture != null)
            {
                if (upTile.furniture.objectType == "Wall" && downTile.furniture.objectType == "Wall")
                {
                    //旋转90度
                    furn_go.transform.rotation = Quaternion.Euler(0, 0, 90);
                    //缩放
                    furn_go.transform.Translate(1f,0,0,Space.World);
                }
            }
        }

        // FIXME：我们假设对象必须是墙，所以请 
        //添加SpriteRenderer
        var sr = furn_go.AddComponent<SpriteRenderer>();
        sr.sprite = GetSpriteForFurniture(furn);
        sr.sortingLayerName = "Furniture";
        //注册我们的回调，以便我们的GameObject随时更新
        furn.RegisterOnChangedCallback(OnFurnitureChanged);
    }
    /// <summary>
    /// 获取已安装对象的精灵
    /// </summary>
    /// <param name="furn"></param>
    /// <returns></returns>
    Sprite GetSpriteForFurniture(Furniture furn)
    {
        var spriteName = furn.objectType;
        if (!furn.linksToNeighbour)
        {
            if (furn.objectType == "Door")
            {
                if (furn.furnParameters["openness"] < 0.1f)
                {
                    spriteName = "Door";
                }
                else if (furn.furnParameters["openness"] < 0.5f)
                {
                    spriteName = "Door_open_1";
                }
                else if (furn.furnParameters["openness"] < 0.9f)
                {
                    spriteName = "Door_open_2";
                }
                else
                {
                    spriteName = "Door_open_3";
                }
            }
            return furnitureSprite[spriteName];
        }

        spriteName = furn.objectType + "_";
        //判断方向
        int x = furn.tile.x; int y = furn.tile.y;
        Tile tile;
        tile = world.GetTileAt(x, y + 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == furn.objectType)
        {
            spriteName += "N";
        }
        tile = world.GetTileAt(x + 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == furn.objectType)
        {
            spriteName += "E";
        }
        tile = world.GetTileAt(x, y - 1);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == furn.objectType)
        {
            spriteName += "S";
        }
        tile = world.GetTileAt(x - 1, y);
        if (tile != null && tile.furniture != null && tile.furniture.objectType == furn.objectType)
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
        if (!furnitureGameObjectMap.ContainsKey(furniture))
        {
            return;
        }
        GameObject obj_go = furnitureGameObjectMap[furniture];
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
