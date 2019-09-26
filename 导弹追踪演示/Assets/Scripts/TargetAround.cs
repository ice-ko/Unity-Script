using UnityEngine;

public class TargetAround : MonoBehaviour
{

    public Transform startPos;
    public Transform endPos;

    public float moveSpeed = 10;

    private Vector3 tempPos;
    private void Start()
    {
        tempPos = startPos.position;
    }
    
    public bool flag = true;
    public float time = 0;
    public float dis;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 2 && flag == false)
        {
            time = 0;
            flag = true;
        }
        dis = (transform.position - tempPos).magnitude;
        if (dis < 5 && flag)
        {
            flag = false;
            if (tempPos.Equals(startPos.position))
            {
                tempPos = endPos.position;
                moveSpeed *= -1;
            }
            else
            {
                tempPos = startPos.position;
                moveSpeed *= -1;
            }

        }
        //transform.position = Vector3.Lerp(transform.position, tempPos, Time.deltaTime * moveSpeed);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //transform.RotateAround(Vector3.zero, Vector3.up, 30 * Time.deltaTime);

    }

}
