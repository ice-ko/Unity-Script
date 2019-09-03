using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用移动脚本
/// </summary>
public class MoveHandler : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed;
    void Start()
    {

    }


    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        //transform.localPosition = new Vector3(
        //    Mathf.Lerp(transform.localPosition.x, targetPosition.x, step),
        //    Mathf.Lerp(transform.localPosition.y, targetPosition.y, step),
        //    Mathf.Lerp(transform.localPosition.z, targetPosition.z, step));
    }
}
