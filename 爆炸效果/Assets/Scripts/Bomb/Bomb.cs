using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //爆炸时间
    public float explotionTime;
    //初始时间
    public float initTime;
    //爆炸半径
    public float radius = 5.0f;
    //指定检测层
    public LayerMask layerMask;
    //爆炸威力
    float power = 10.0f;
    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        explotionTime -= Time.deltaTime;
        if (explotionTime <= 0)
        {
            anim.SetTrigger("IsExplotion");
            explotionTime = initTime;
            //修改tag
            gameObject.tag = "Untagged";
            Destroy(GetComponent<Rigidbody2D>());
            //
            ExplosionForce();
            Destroy(gameObject, 0.8f);
        }
    }
    void ExplosionForce()
    {
        var explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D hit in colliders)
        {
            if (hit == null || hit.tag == "Untagged")
            {
                continue;
            }
            Rigidbody2D rig2d = hit.GetComponent<Rigidbody2D>();
            switch (hit.tag)
            {
                case "Player":
                    power = (float)ExplosionData.Player;
                    hit.GetComponent<Player>().enabled = false;
                    break;
                case "Bomb": power = (float)ExplosionData.Bomb; break;
                case "Enemy":
                    power = (float)ExplosionData.Enemy;
                    hit.GetComponent<Enemy>().enabled = false;
                    break;
                case "OtherObjects": power = (float)ExplosionData.OtherObjects; break;
                case "Table": power = (float)ExplosionData.Table; break;
                case "Chair": power = (float)ExplosionData.Chair; break;
            }
            if (rig2d != null)
            {
                // if (hit.tag == "OtherObjects")
                // {
                //     rig2d.AddForceAtPosition(new Vector2(0, power), Vector2.up);

                // }
                // else
                // {
                //     ExplosionForce2D.AddExplosionForce(rig2d, power, transform.position, radius);
                // }
                var dir = (rig2d.transform.position - transform.position);
                float calc = 1 - (dir.magnitude / radius);
                if (calc <= 0)
                {
                    calc = 0;
                }
                Vector2 vt = dir.normalized * power * calc;
                // rig2d.AddForceAtPosition(vt, Vector2.up);
                ExplosionForce2D.AddExplosionForce(rig2d, power, explosionPos, radius);
                if (hit.tag == "Player" || hit.tag == "Enemy")
                {
                    StartCoroutine(Delay(hit));
                }
            }
        }
    }
    /// <summary>
    /// 延迟1秒启动脚本
    /// </summary>
    /// <param name="collider2D"></param>
    /// <returns></returns>
    IEnumerator Delay(Collider2D collider2D)
    {
        collider2D.GetComponentInChildren<Animator>().SetTrigger("IsHit");
        yield return new WaitForSeconds(0.5f);
        switch (collider2D.tag)
        {
            case "Player":
                collider2D.GetComponent<Player>().enabled = true;
                break;
            case "Enemy":
                collider2D.GetComponent<Enemy>().enabled = true;
                collider2D.GetComponent<Enemy>().animTime = 5f;
                collider2D.GetComponent<Enemy>().isMove = false;
                break;
        }
    }
    IEnumerator BombTimer()
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 explosionPos = transform.position;
        Collider[] collider = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in collider)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0f);
            }
        }
    }
}
