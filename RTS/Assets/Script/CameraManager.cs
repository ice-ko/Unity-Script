using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// 平移速度
    /// </summary>
    public float panSpeed;
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotateSpeed;
    /// <summary>
    /// 旋转量
    /// </summary>
    public float rotateAmount;
    /// <summary>
    /// 旋转
    /// </summary>
    private Quaternion rotation;
    /// <summary>
    /// 平移检测
    /// </summary>
    private float panDetect = 15f;
    /// <summary>
    /// 最小高度
    /// </summary>
    private float minHeight = 5f;
    /// <summary>
    /// 最大高度
    /// </summary>
    private float maxHeight = 100f;

    void Start()
    {
        rotation = Camera.main.transform.rotation;
    }
    void Update()
    {
        MoveCamera();
        RorateCamera();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.rotation = rotation;
        }
    }
    void MoveCamera()
    {
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;
        float moveZ = Camera.main.transform.position.z;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if (Input.GetKey(KeyCode.A) || xPos > 0 && xPos < panDetect)
        {
            moveX -= panSpeed;
        }
        else if (Input.GetKey(KeyCode.D) || xPos < Screen.width && xPos > Screen.width - panDetect)
        {
            moveX += panSpeed;
        }
        else if (Input.GetKey(KeyCode.W) || yPos < Screen.height && yPos > Screen.height - panDetect)
        {
            moveZ += panSpeed;
        }
        else if (Input.GetKey(KeyCode.S) || yPos > 0 && yPos < panDetect)
        {
            moveZ -= panSpeed;
        }
        moveY -= Input.GetAxis("Mouse ScrollWheel") * (panSpeed * 20);
        moveY = Mathf.Clamp(moveY,minHeight,maxHeight);
        Vector3 newPos = new Vector3(moveX, moveY, moveZ);
        Camera.main.transform.position = newPos;
    }
    void RorateCamera()
    {
        //起点
        Vector3 origin = Camera.main.transform.eulerAngles;
        //目的点
        Vector3 destination = origin;
        if (Input.GetMouseButtonDown(2))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }
}
