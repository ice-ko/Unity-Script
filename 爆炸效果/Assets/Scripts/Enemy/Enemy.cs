using System.Collections;
using UnityEngine;
/// <summary>
/// 敌人基础
/// </summary>
public class Enemy : MonoBehaviour
{
    //移动速度
    public float speed = 2f;
    //动画播放时间
    public float animTime = 5f;
    //移动时间
    float moveTime = 0f;
    //空闲时间
    float idleTime = 1f;
    //是否移动
    public bool isMove = false;
    //最好移动方向
    float last_delatX;
    bool isAttack = false;
    Animator anim;
    Rigidbody2D rig2d;
    void Start()
    {
        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("moveX", 1);
    }
    void Update()
    {
        if (isAttack)
        {
            return;
        }
        animTime -= Time.deltaTime;
        idleTime -= Time.deltaTime;
        if (animTime <= 0 && !isMove)
        {
            last_delatX = Random.Range(-1, 1);
            last_delatX = last_delatX == 0 ? 1 : last_delatX;
            anim.SetFloat("moveX", last_delatX);
            if (idleTime <= 0)
            {
                moveTime = 3f;
                animTime = 5f;
                idleTime = 3f;
            }
        }
        moveTime -= Time.deltaTime;
        if (moveTime >= 0)
        {
            Vector2 move = new Vector2(last_delatX * speed, rig2d.velocity.y);
            rig2d.velocity = move;
            isMove = true;
            anim.SetBool("IsMove", true);
        }
        else if (moveTime <= 0)
        {
            isMove = false;
            anim.SetBool("IsMove", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var rig = other.GetComponent<Rigidbody2D>();
        if (rig == null)
        {
            return;
        }
        if (other.tag == "Bomb")
        {
            Move(other.transform.position);

            StartCoroutine(Delay(rig, other.transform.position));
        }
        if (other.tag == "Player")
        {
            Move(other.transform.position);
            StartCoroutine(DelayAttackPlayer(rig, other.transform));
        }
    }
    /// <summary>
    /// 移动到指定位置
    /// </summary>
    /// <param name="target"></param>
    void Move(Vector3 target)
    {
        var m = target - transform.position;
        anim.SetFloat("moveX", m.normalized.x > 0 ? 1 : -1);
        m.y = rig2d.velocity.y;
        m.x = m.x * speed;
        rig2d.velocity = m;
        isAttack = true;
    }
    IEnumerator Delay(Rigidbody2D rig, Vector3 pos)
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.3f);
        var m = pos - transform.position;
        rig.velocity = new Vector2(m.x * 6f, rig.velocity.y + 8f);
        isAttack = false;
    }
    IEnumerator DelayAttackPlayer(Rigidbody2D rig, Transform pos)
    {
        anim.SetTrigger("Attack");
        Debug.Log(pos.gameObject.name);
        yield return new WaitForSeconds(0.3f);
        //var m = pos - transform.position;
        //rig.velocity = new Vector2(0f, rig.velocity.y + 8f);
        Vector3 force = Quaternion.Euler(
       new Vector3(pos.rotation.eulerAngles.x, pos.rotation.eulerAngles.y,
       pos.rotation.eulerAngles.z)) * new Vector3(300f, 0, 0);
        //给刚体添加力
        rig.GetComponent<Rigidbody2D>().AddForce(force);
        isAttack = false;
    }
}