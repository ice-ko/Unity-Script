using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public List<Sprite> spritesArr;
    void Start()
    {

    }

    void Update()
    {

    }
    /// <summary>
    /// 更换精灵
    /// </summary>
    /// <param name="index"></param>
    public void ChangeSprite(int index)
    {
        GetComponent<SpriteRenderer>().sprite = spritesArr[index];
    }
}
