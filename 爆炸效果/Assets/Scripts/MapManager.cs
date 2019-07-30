using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    // Use this for initialization
    private float roomRadii;//房间的长度半径
    private float roomRatio;//由宽决定长的比值
    public float roomX;//房间的长
    public float roomY;//房间的宽
    public float roomXi;//房间i的长
    public float roomYi;//房间i的宽

    public int roomXMin = 10;
    public int roomXMax = 16;

    public Vector2 posO;//房间中心点

    int roomIndex = 0;

    public List<Vector2> roomPos = new List<Vector2>();//创建储存房间有效位置的二维表
    public List<Vector2> offset = new List<Vector2>();//创建储存房间中心点的二维表

    public GameObject[] floorArray;

    public Transform roomHolder;

    void Start()
    {
        roomPos.Clear();//后续房间内的所有可用位置都会添加到这个二维表里
        SetRoomXY();
        posO = new Vector2(0, 0);

        //生成12个房间进行检验
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
        CreateRoom(posO);
    }

    void SetRoomXY()//取得房间长宽半径
    {
        roomRadii = Random.Range(roomXMin, roomXMax);//标准值取随机值
        roomRatio = Random.Range(16, 20);
        roomRatio = roomRatio / 10;
        roomX = 2 * roomRadii;//房间的长是标准值的两倍，因此就是偶数
        roomY = roomX / roomRatio;//房间的宽由长来决定
        roomY = Mathf.Round(roomY);//对Y取整数
        if (roomY % 2 != 0)//对Y进行判断，取偶数 {
            roomY += 1;
    }
    Vector2 CreateCenter(Vector2 pos)//对中心点进行偏移
    {
        int x = Random.Range(2 * roomXMin + 1, 2 * roomXMax + 1);
        offset.Clear();
        for (int i = 2 * roomXMin + 1; i <= 2 * roomXMax + 1; i++)
        {
            for (int j = 30; j <= 50; j++)
            {
                offset.Add(new Vector2(i, j));
            }
        }
        //现在取出一个符合基本规则的中心点偏移量
        int index = Random.Range(0, offset.Count);
        float offsetX = offset[index].x;
        float offsetY = offset[index].y;

        Vector2 pos2 = new Vector2(0, 0);

        int o = Random.Range(0, 4);
        if (o == 0)
        {
            pos2 = pos + new Vector2(offsetX, offsetY);
        }
        if (o == 1)
        {
            pos2 = pos + new Vector2(offsetX, -offsetY);
        }
        if (o == 2)
        {
            pos2 = pos + new Vector2(-offsetX, offsetY);
        }
        if (o == 3)
        {
            pos2 = pos + new Vector2(-offsetX, -offsetY);
        }

        //现在取得了下一个房间中心
        posO = pos2;
        return posO;
    }

    void CreateRoom(Vector2 pos)//生成基础房间(铺设地板)
    {
        CreateCenter(pos);//对上一个房间的中心点进行偏移，取得本次房间的中心点
        pos = posO;
        SetRoomXY();//取得当前要生成房间的长宽半径

        roomXi = roomX;
        roomYi = roomY;

        int isRoom = 0;

        //对预生成的房间先进行是否有房间的检测(基于房间九点：左上，中上，右上，左中……)
        Vector2 checkPos1 = new Vector2(pos.x - roomXi, pos.y - roomYi) + new Vector2(-4, -4);
        Vector2 checkPos2 = new Vector2(pos.x - roomXi, pos.y + roomYi) + new Vector2(-4, 4);
        Vector2 checkPos3 = new Vector2(pos.x + roomXi, pos.y - roomYi) + new Vector2(-4, 4);
        Vector2 checkPos4 = new Vector2(pos.x + roomXi, pos.y + roomYi) + new Vector2(4, 4);
        Vector2 checkPos6 = new Vector2(pos.x, pos.y - roomYi) + new Vector2(0, -4);
        Vector2 checkPos7 = new Vector2(pos.x, pos.y + roomYi) + new Vector2(0, 4);
        Vector2 checkPos8 = new Vector2(pos.x + roomXi, pos.y) + new Vector2(4, 0);
        Vector2 checkPos9 = new Vector2(pos.x - roomXi, pos.y) + new Vector2(-4, 0);

        RaycastHit2D hit1 = Physics2D.Linecast(checkPos1, checkPos1, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit2 = Physics2D.Linecast(checkPos2, checkPos2, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit3 = Physics2D.Linecast(checkPos3, checkPos3, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit4 = Physics2D.Linecast(checkPos4, checkPos4, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit5 = Physics2D.Linecast(pos, pos, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit6 = Physics2D.Linecast(checkPos6, checkPos6, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit7 = Physics2D.Linecast(checkPos7, checkPos7, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit8 = Physics2D.Linecast(checkPos8, checkPos8, 1 << LayerMask.NameToLayer("floor"));
        RaycastHit2D hit9 = Physics2D.Linecast(checkPos9, checkPos9, 1 << LayerMask.NameToLayer("floor"));
        if (hit1.collider == null && hit2.collider == null && hit3.collider == null && hit4.collider == null && hit5.collider == null && hit6.collider == null && hit7.collider == null && hit8.collider == null && hit9.collider == null)
        {
            isRoom += 1;
        }
        if (isRoom >= 0)
        {
            if (isRoom == 0)
            {
                CreateRoom(pos);
            }
            else
            {
                roomIndex += 1;//可以生成房间的时候，给房间记上标记
                roomHolder = new GameObject("Room" + roomIndex).transform;//给父类添加名字

                //为地板进行二维数组赋值
                for (float x = pos.x - roomXi; x <= pos.x + roomXi; x++)
                {
                    for (float y = pos.y - roomYi; y <= pos.y + roomYi; y++)
                    {
                        Vector2 posFloor = new Vector2(x, y);
                        roomPos.Add(new Vector2(x, y));
                        int i = Random.Range(0, floorArray.Length);
                        GameObject go = Instantiate(floorArray[i], posFloor, Quaternion.identity);
                        go.transform.SetParent(roomHolder);
                    }
                }
            }
        }
    }
}