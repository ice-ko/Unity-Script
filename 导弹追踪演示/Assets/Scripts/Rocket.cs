using UnityEngine;


public enum RocketType
{
    Follow,
    Prognosis
}


public class Rocket : MonoBehaviour
{

    public Transform target;               //跟踪目标
    public GameObject FX;                  //爆炸特效
    public float selfSpeed;                //移动速度
    public float rotateSpeed;             //旋转速度

    public RocketType rocketType;


    private float lastTime;
    private Vector3 lastPos;

    private void Start()
    {
        if (target != null && rocketType == RocketType.Prognosis)
        {
            lastTime = Time.time;               //首先初始化 lastTime 和 lastPos
            lastPos = target.position;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            switch (rocketType)
            {
                case RocketType.Follow:

                    transform.LookAt(target);

                    // Vector3 tempDirV = target.position - transform.position;                //计算出正确的转向
                    // Quaternion rightDirQ = Quaternion.LookRotation(tempDirV);               //将转向 转化为 四元数
                    // transform.rotation = Quaternion.Lerp(transform.rotation, rightDirQ, rotateSpeed);  //利用四元数插值方法 将方向赋给 transform.rotation

                    break;
                case RocketType.Prognosis:

                    float targetSpeed = MathTool.GetRightSpeed(lastPos, target.position, lastTime, Time.time);
                    Vector3 rightPos = MathTool.GetRightPos(targetSpeed, selfSpeed, lastPos, transform, target);   //计算 正确的 目标点

                    Debug.DrawLine(transform.position, rightPos, Color.green);
                    Debug.DrawLine(target.position, rightPos, Color.red);

                    if (rightPos.Equals(Vector3.zero))                                            //判断方程有解
                        return;
                    Quaternion rightDir = Quaternion.LookRotation(rightPos - transform.position);     //转化四元数
                    transform.rotation = Quaternion.Lerp(transform.rotation, rightDir, Time.deltaTime * 25); //设置旋转
                    lastPos = target.position;                                              //记录 当前帧的位置
                    lastTime = Time.time;                                                   //记录 当前帧的时间  供下一帧 使用

                    break;
            }
        }
        transform.position += transform.forward * Time.deltaTime * selfSpeed;      //向前移动
    }

    //当 有物体进入到 自己的触发器时 调用一次
    private void OnTriggerEnter(Collider other)
    {
        GameObject tempFX = Instantiate(FX, transform.position, transform.rotation);  //生成一个爆炸特效 并给予位置和旋转信息
        Destroy(gameObject);      //销毁自己
        Destroy(tempFX, 1);       //等待 1秒 销毁刚才 创建生成的爆炸特效
    }
}
