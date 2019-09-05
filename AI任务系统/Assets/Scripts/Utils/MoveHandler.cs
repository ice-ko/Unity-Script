using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用移动脚本
/// </summary>
public class MoveHandler : MonoBehaviour
{
    public Vector3 targetPosition;
    public Action onArrivedAtPosition;
    Rigidbody2D rb;

    bool isPos = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        float dis = (targetPosition - transform.position).sqrMagnitude;
        if (dis < 1 && onArrivedAtPosition != null)
        {
            //移动到指定位置后触发委托有问题
           // Action iss= onArrivedAtPosition();
        }
        float step = CharacterHandler.Instance.speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.localPosition = new Vector3(
            Mathf.Lerp(transform.localPosition.x, targetPosition.x, step),
            Mathf.Lerp(transform.localPosition.y, targetPosition.y, step),
            Mathf.Lerp(transform.localPosition.z, targetPosition.z, step));

    }
    private void FixedUpdate()
    {
        //float dis = (targetPosition - transform.position).sqrMagnitude;
        //if (dis<1)
        //{
        //    return;
        //}
        //Vector3 offset = targetPosition - transform.position;
        //float power = offset.magnitude;
        //rb.velocity = offset.normalized * 1000f * Time.fixedDeltaTime;
        //rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(targetPosition.x * 15f, rb.velocity.y)), Time.deltaTime);
        //rg.AddForce((targetPosition - transform.position).normalized * 1.0f);
        //rg.MovePosition(targetPosition * CharacterHandler.Instance.speed * Time.fixedDeltaTime);

        // rg.MovePosition(rg.position + new Vector2(targetPosition.x,targetPosition.y) * Time.fixedDeltaTime);
    }
}
