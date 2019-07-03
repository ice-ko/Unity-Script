using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物信息
/// </summary>
public class Character
{
    public float X
    {
        get
        {
            return Mathf.Lerp(currTile.x, destTile.x, movementPercentage);
        }
    }
    public float Y
    {
        get
        {
            return Mathf.Lerp(currTile.y, destTile.y, movementPercentage);
        }
    }
    //当前
    Tile currTile;
    //如果我们没有移动，那么destTile = currTile
    Tile destTile;
    //当我们从currTile转移到destTile时，从0到1
    float movementPercentage;

    float speed = 5f;

    public void Update(float deltaTime)
    {
        // 我们到了吗？
        if (currTile == destTile)
            return;

        // 从A点到B点的总距离是多少？
        float distToTravel = Mathf.Sqrt(Mathf.Pow(currTile.x - destTile.x, 2) + Mathf.Pow(currTile.y - destTile.y, 2));

        // 这个更新可以运动多长时间？
        float distThisFrame = speed * deltaTime;

        // 到目的地的需要多久？
        float percThisFrame = distToTravel / distToTravel;

        // 将其添加到旅行的总体百分比。
        movementPercentage += percThisFrame;

        if (movementPercentage >= 1)
        {
            // 我们到达了目的地
            currTile = destTile;
            movementPercentage = 0;
            // 我们真的想保留任何超车运动吗？
        }
    }
    public Character(Tile tile)
    {
        currTile = destTile = tile;
    }
    /// <summary>
    /// 设置目的地
    /// </summary>
    /// <param name="tile"></param>
    public void SetDestination(Tile tile)
    {
        if (!currTile.IsNeighbour(tile))
        {
            return;
        }
    }
}
