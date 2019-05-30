using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlArrow : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        //是否有刚体组件
        if (transform.GetComponent<Rigidbody2D>() != null)
        {
            // 是否正在移动
            if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                // 得到实际的速度
                Vector3 vel = GetComponent<Rigidbody2D>().velocity;
                // 通过简单的atan2从x和y速度计算旋转
                float angleZ = Mathf.Atan2( vel.y, vel.x) * Mathf.Rad2Deg;
                //通过atan2计算z和x的倾斜角度乘2保证不管从那个方向射箭都能保证倾斜角方向正常
                float angleY = Mathf.Atan2(vel.z, vel.x) * Mathf.Rad2Deg*2;
                // 根据轨迹旋转箭头
                transform.eulerAngles = new Vector3(0, angleY, angleZ);
            }
        }
    }
}
