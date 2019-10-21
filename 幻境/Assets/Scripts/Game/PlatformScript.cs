using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] sprites;

    public GameObject obatacle;

    /// <summary>
    /// 计时器
    /// </summary>
    private bool startTimer;
    /// <summary>
    /// 掉落时间
    /// </summary>
    private float fallTime;

    private Rigidbody2D rg2d;
    void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (GameManager.Instance.IsGameStart==false||GameManager.Instance.PlayerIsMove==false)
        {
            return;
        }
        if (startTimer)
        {
            fallTime -= Time.deltaTime;
            if (fallTime < 0)
            {
                //掉落
                startTimer = false;
                if (rg2d.bodyType != RigidbodyType2D.Dynamic)
                {
                    rg2d.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(CoroutineHelper.DealyHide(gameObject));
                }
            }
        }
        if (transform.position.y-Camera.main.transform.position.y<-6)
        {
            StartCoroutine(CoroutineHelper.DealyHide(gameObject));
        }
    }
    public void Init(Sprite sprite, float fallTime, int obatacleDir)
    {
        this.fallTime = fallTime;
        startTimer = true;
        rg2d.bodyType = RigidbodyType2D.Static;


        foreach (var sp in sprites)
        {
            sp.sprite = sprite;
        }
        //朝向右边
        if (obatacleDir == 0)
        {
            if (obatacle != null)
            {
                obatacle.transform.localPosition = new Vector3(-obatacle.transform.localPosition.x
                    , obatacle.transform.localPosition.y, 0);
            }
        }
    }

}
