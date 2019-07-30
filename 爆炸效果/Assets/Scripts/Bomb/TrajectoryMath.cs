using UnityEngine;

public class TrajectoryMath : MonoBehaviour
{
    /// <summary>
    /// 返回初始速度以到达二维中的特定点
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="offsetY"></param>
    /// <returns></returns>
    public static Vector2 GetParableInitialVelocity(Vector3 origin, Vector3 target, float offsetY = 0.0f)
    {
        // 初始轨迹变量
        float gravity = Physics2D.gravity.magnitude;
        float height = Mathf.Abs(target.y - origin.y + offsetY);
        float dist = Mathf.Abs(target.x - origin.x);
        float vertVelocity = 0.0f;
        float time = 0.0f;
        float horzVelocity = 0.0f;

        if (height < 0.1f) height = 0.1f; // 防止除以零
        if (gravity < 0.1f) gravity = 0.1f; // 防止除以零

        //如果我们向上走
        //我们将使用直接的比喻轨迹
        //并达到最高点
        if (target.y - origin.y > 1.0f)
        {
            vertVelocity = Mathf.Sqrt(2.0f * gravity * height);
            time = vertVelocity / gravity;
            horzVelocity = dist / time;
        }
        //如果我们要往下走
        //我们将使用直接的比喻轨迹
        //没有垂直速度
        else if (target.y - origin.y < -1.0f)
        {
            vertVelocity = 0.0f;
            time = Mathf.Sqrt(2 * height / gravity);
            horzVelocity = dist / time;
        }
        //否则我们将遵循一个完整的寓言
        //并确定跳跃的高度
        //取决于2点之间的距离
        else
        {
            height = dist / 4;
            vertVelocity = Mathf.Sqrt(2.0f * gravity * height);
            time = 2 * vertVelocity / gravity;
            horzVelocity = dist / time;
        }

        if (vertVelocity == 0.0f && horzVelocity == 0.0f)
        {
            return Vector2.zero;
        }

        // Jump right
        if (target.x - origin.x > 0.0f && !float.IsNaN(vertVelocity) && !float.IsNaN(horzVelocity))
        {
            return new Vector2(horzVelocity, vertVelocity);
        }
        // Jump left
        else if (!float.IsNaN(vertVelocity) && !float.IsNaN(horzVelocity))
        {
            return new Vector2(-horzVelocity, vertVelocity);
        }
        else
        {
            return Vector2.zero;
        }
    }
}