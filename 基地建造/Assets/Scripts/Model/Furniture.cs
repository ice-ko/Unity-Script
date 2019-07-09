using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Furnitures就像墙壁，门和家具（例如沙发）
/// </summary>
public class Furniture : IXmlSerializable
{

    public Dictionary<string, float> furnParameters;
    public System.Action<Furniture, float> updateActions;
    public System.Func<Furniture,Enterability> IsEnterable;

    public void Update(float deltaTime)
    {
        Debug.Log("Update: " + updateActions);
        if (updateActions != null)
        {
            updateActions(this, deltaTime);
        }
    }

    //这表示对象的BASE图块 - 但实际上，大对象实际上可能占用
    //多层瓷砖
    public Tile tile;
    // 精灵类型
    public string objectType;
    //这是一个乘数。 所以这里的值为“2”意味着你移动两倍（即半速）
    //可以组合平铺类型和其他环境影响。
    //例如，一张“粗糙”的瓷砖（成本为2），表格（成本为3）着火（成本为3）
    //总移动成本为（2 + 3 + 3 = 8），因此您将以1/8正常速度移动此平铺。
    // SPECIAL：如果movementCost = 0，则此图块无法通过。 （例如墙）。
    public float movementCost = 5f;
    //例如，沙发可能是3x2（实际图形似乎只覆盖3x1区域，但额外的行是用于腿部空间。）
    int width;
    int height;
    //链接到邻居
    public bool linksToNeighbour = false;
    //
    System.Action<Furniture> cbOnChanged;
    //位置验证
    public System.Func<Tile, bool> funcPositionValidation;

    public Furniture()
    {
        furnParameters = new Dictionary<string, float>();
    }
    public Furniture(Furniture other)
    {
        this.objectType = other.objectType;
        this.movementCost = other.movementCost;
        this.width = other.width;
        this.height = other.height;
        this.linksToNeighbour = other.linksToNeighbour;

        this.furnParameters = new Dictionary<string, float>(other.furnParameters);
        if (other.updateActions != null)
        {
            this.updateActions = (System.Action<Furniture, float>)other.updateActions.Clone();
        }
        this.IsEnterable = other.IsEnterable;
    }
    /// <summary>
    /// 创建家具建造
    /// </summary>
    /// <param name="objectType">类型</param>
    /// <param name="linksToNeighbour"></param>
    /// <param name="movementCost">移动速度</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Furniture(string objectType, bool linksToNeighbour, float movementCost = 1f, int width = 1,
    int height = 1)
    {
        this.objectType = objectType;
        this.movementCost = movementCost;
        this.width = width;
        this.height = height;
        this.linksToNeighbour = linksToNeighbour;
        this.funcPositionValidation = this.__IsValidPosition;

        furnParameters = new Dictionary<string, float>();
    }
    /// <summary>
    /// 放置对象
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    public static Furniture PlaceObject(Furniture proto, Tile tile)
    {
        if (proto.funcPositionValidation != null && !proto.funcPositionValidation(tile))
        {
            //Debug.LogError("PlaceObject  - 位置有效性函数返回FALSE。");
            return null;
        }
        Furniture info = proto.Clone();
        info.tile = tile;
        if (tile.PlaceObject(info) == false)
        {
            return null;
        }
        if (info.linksToNeighbour)
        {
            // 这种类型的家具与其邻居相连，
            //所以我们应该通知你的邻居他们有一个新的伙伴。 只需触发他们的OnChange回调。
            int x = tile.x; int y = tile.y;
            var t = tile.world.GetTileAt(x, y + 1);
            if (t != null && t.furniture != null && t.furniture.objectType == info.objectType)
            {
                //我们有一个与我们相同的对象类型的北方邻居，所以
                //通过触发告诉它已经改变了回调。
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x + 1, y);
            if (t != null && t.furniture != null && t.furniture.cbOnChanged != null && t.furniture.objectType == info.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x, y - 1);
            if (t != null && t.furniture != null && t.furniture.cbOnChanged != null && t.furniture.objectType == info.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x - 1, y);
            if (t != null && t.furniture != null && t.furniture.cbOnChanged != null && t.furniture.objectType == info.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
        }

        return info;
    }
    /// <summary>
    /// 注册已创建的 Furniture
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void RegisterOnChangedCallback(System.Action<Furniture> cakkbackFunc)
    {
        cbOnChanged += cakkbackFunc;
    }
    /// <summary>
    ///注销已创建的 Furniture
    /// </summary>
    /// <param name="cakkbackFunc"></param>
    public void UnregisterOnChangedCallback(System.Action<Furniture> cakkbackFunc)
    {
        cbOnChanged -= cakkbackFunc;
    }
    public bool IsValidPosition(Tile tile)
    {
        return funcPositionValidation(tile);
    }
    public bool __IsValidPosition(Tile t)
    {
        //确保瓷砖是FLOOR
        if (t.TileType != TileType.Floor)
        {
            return false;
        }
        //确保瓷砖还没有家具
        if (t.furniture != null)
        {
            return false;
        }

        return true;
    }
    public bool IsValidPosition_Door(Tile tile)
    {
        if (!__IsValidPosition(tile))
        {
            return false;
        }
        return true;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {

        objectType = reader.GetAttribute("objectType");
        movementCost = float.Parse(reader.GetAttribute("movementCost"));
        if (reader.ReadToDescendant("Param"))
        {
            do
            {
                var k = reader.GetAttribute("Name");
                var v =int.Parse(reader.GetAttribute("Value"));
                furnParameters[k] = v;
            } while (reader.ReadToNextSibling("Param"));
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("X", tile.x.ToString());
        writer.WriteAttributeString("Y", tile.y.ToString());
        writer.WriteAttributeString("objectType", objectType);
        writer.WriteAttributeString("movementCost", movementCost.ToString());
        foreach (string item in furnParameters.Keys)
        {
            writer.WriteStartElement("Param");
            writer.WriteAttributeString("Name",item);
            writer.WriteAttributeString("Value", furnParameters[item].ToString());
            writer.WriteEndElement();
        }
    }
    virtual public Furniture Clone()
    {
        return new Furniture(this);
    }


}
