using UnityEngine;

public class ExplosionForce2D : MonoBehaviour
{
    /// <summary>
    /// 添加爆炸效果
    /// </summary>
    /// <param name="body">刚体</param>
    /// <param name="expForce">爆炸威力</param>
    /// <param name="expPosition">爆炸位置</param>
    /// <param name="expRadius">爆炸半径</param>
    /// <param name="offset"></param>
    public static void AddExplosionForce(Rigidbody2D body, float expForce, Vector3 expPosition, float expRadius, float offset=1)
    {
        var dir = (body.transform.position - expPosition);
        float calc = 1 - (dir.magnitude / expRadius);
        if (calc <= 0)
        {
            calc = 0;
        }
        // body.AddForce(dir.normalized * expForce * calc);
        Vector2 vector2 = dir.normalized * expForce * calc;
        vector2.y = vector2.y * offset;
        body.AddForceAtPosition(vector2, Vector2.up);
    }
}