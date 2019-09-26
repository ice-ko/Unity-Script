using UnityEngine;

public class RocketTurret : MonoBehaviour
{

    public Transform target;       //目标
    public GameObject rocket;      //火箭 预设体
    public Transform firePoint;    //发射火箭的 围着
    public float fireSpeed = 5;    //发射间隔

    // Update is called once per frame
    void Update()
    {
        fireSpeed -= Time.deltaTime;
        if (fireSpeed < 0)
        {
            fireSpeed = 5;
            Fire();
        }
    }

    //开火方法
    void Fire()
    {
        GameObject tempRocket = Instantiate(rocket, firePoint.position, firePoint.rotation);
        tempRocket.GetComponent<Rocket>().target = GameObject.FindGameObjectWithTag("Target").transform;
    }

}
