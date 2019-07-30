using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 角色投掷炸弹
/// </summary>
public class PlayerThrow : Singleton<PlayerThrow>
{
    /// <summary>
    /// 投掷力大小
    /// </summary>
    public float throwForce = 0;
    //炸弹预制体
    public GameObject bombPrefab;
    public Transform bombPos;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (throwForce <= 1)
            {
                throwForce += 0.1f;
            }
        }
        else
        {
            if (throwForce > 0)
            {
                Animator anim = GetComponentInChildren<Animator>();
                if (anim.GetFloat("moveX") > 0)
                {
                    bombPos.localPosition = new Vector3(0.3f, 0.35f, 0);
                }
                else
                {
                    bombPos.localPosition = new Vector3(-0.3f, 0.35f, 0);
                }
                GameObject go = Instantiate(bombPrefab, bombPos.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().velocity = TrajectoryMath.GetParableInitialVelocity(go.transform.position, Utility.GetMouseWorldPos(), throwForce);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            throwForce = 0f;
        }
        UIManager.Instance.TrajectoryProgressBar(throwForce);
    }
}