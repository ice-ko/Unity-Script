using UnityEngine;

public class MathTool
{
    /// <summary>
    /// 计算目标 当前帧的移动速度
    /// </summary>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>
    /// <param name="lastTime"></param>
    /// <param name="currentTime"></param>
    /// <returns></returns>
    public static float GetRightSpeed(Vector3 lastPos, Vector3 currentPos, float lastTime, float currentTime)
    {
        if ((currentTime - lastTime) == 0)
            return -1;
        return Vector3.Distance(currentPos, lastPos) / (currentTime - lastTime);
    }

    /// <summary>
    /// 计算目标 正确的移动方向
    /// </summary>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>
    /// <returns></returns>
    public static Vector3 GetDirection(Vector3 lastPos, Vector3 currentPos)
    {
        return (currentPos - lastPos).normalized;
    }

    /// <summary>
    /// 计算出 detla
    /// </summary>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static float GetDetla(float b, float a, float c)
    {
        return b * b - 4 * a * c;
    }

    /// <summary>
    /// 计算目标 正确的碰撞时间
    /// </summary>
    /// <param name="b"></param>
    /// <param name="detla"></param>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static float GetRightTime(float b, float detla, float a, float c)
    {
        if (detla < 0) //判断 detla < 0 则方程无解
        {
            return -1;
        }
        else if (a == 0)   //at^2 + bt +c = 0  当 a == 0 时 。 
        {
            return -(b / c);
        }
        else if (detla == 0)   //detla == 0 时。
        {
            return -(b / 2 * a);
        }
        else
        {
            float time1 = (-b + Mathf.Sqrt(detla)) / (2 * a);
            float time2 = (-b - Mathf.Sqrt(detla)) / (2 * a);
            return time1 > time2 ? time1 : time2;
        }

    }

    /// <summary>
    /// 计算目标 正确的目标移动位置
    /// </summary>
    /// <param name="targetSpeed"></param>
    /// <param name="selfSpeed"></param>
    /// <param name="lastPos"></param>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 GetRightPos(float targetSpeed, float selfSpeed, Vector3 lastPos, Transform self, Transform target)
    {
        if (targetSpeed == -1)
            return Vector3.zero;
        Vector3 targetDir = GetDirection(lastPos, target.position);                    //获取 目标 移动方向
        Vector3 AB = target.position - self.position;
        float angle = Vector3.Angle(AB, targetDir);         //获取角度
        float L1 = Mathf.Sin(angle * Mathf.Deg2Rad) * AB.magnitude;         //计算L1
        float L2 = Mathf.Cos(angle * Mathf.Deg2Rad) * AB.magnitude;         //计算L2
        float a = targetSpeed * targetSpeed - selfSpeed * selfSpeed;      //计算 a
        float b = 2 * targetSpeed * L2;                                   //计算 b
        float c = L1 * L1 + L2 * L2;                                      //计算 c
        if (Vector3.Dot(AB, targetDir) < 0)                   //判断 同向 还是 反向
        {
            b *= -1;                                                      //如果 反向 则b应该为相反数
        }
        float detla = GetDetla(b, a, c);                      //计算 detla
        float rightTime = GetRightTime(b, detla, a, c);                //计算 正确的时间
        if (rightTime == -1)
            return Vector3.zero;
        Vector3 rightPos = targetDir * rightTime * targetSpeed + target.position;   //计算 正确的 目标点
        return rightPos;
    }
}
