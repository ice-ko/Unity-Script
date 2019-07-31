/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_QuestPointer : MonoBehaviour
{
    [Header("任务指针预制体")]
    public GameObject questPointerPrefabs;
    [Header("ui相机")]
    public Camera uiCamera;
    /// <summary>
    /// 任务箭头游戏对象集合
    /// </summary>
    private List<QuestPointer> questPointerList;

    private void Awake()
    {
        questPointerList = new List<QuestPointer>();
    }

    private void Update()
    {
        foreach (QuestPointer questPointer in questPointerList)
        {
            questPointer.Update();
        }
    }
    /// <summary>
    /// 创建任务指针
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    public QuestPointer CreatePointer(Vector3 targetPosition)
    {
        //实例化预制体
        GameObject pointerGameObject = Instantiate(questPointerPrefabs);
        //设置父物体
        pointerGameObject.transform.SetParent(transform, false);
        //创建任务指针
        QuestPointer questPointer = new QuestPointer(targetPosition, pointerGameObject, uiCamera);
        //
        questPointerList.Add(questPointer);
        return questPointer;
    }
    /// <summary>
    /// 从集合中删除任务指针
    /// </summary>
    /// <param name="questPointer"></param>
    public void DestroyPointer(QuestPointer questPointer)
    {
        questPointerList.Remove(questPointer);
        questPointer.DestroySelf();
    }

    public class QuestPointer
    {
        /// <summary>
        /// 目标位置
        /// </summary>
        private Vector3 targetPosition;
        /// <summary>
        /// 任务指针
        /// </summary>
        private GameObject pointerGameObject;

        private Camera uiCamera;
        private RectTransform pointerRectTransform;

        public QuestPointer(Vector3 targetPosition, GameObject pointerGameObject, Camera uiCamera)
        {
            this.targetPosition = targetPosition;
            this.pointerGameObject = pointerGameObject;
            this.uiCamera = uiCamera;

            pointerRectTransform = pointerGameObject.GetComponent<RectTransform>();
        }

        public void Update()
        {
            //距离边框的距离
            float borderSize = 100f;
            //把目标坐标转化为场景坐标
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            //判断是否在屏幕外
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize
                || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen)
            {
                RotatePointerTowardsTargetPosition();

                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                //计算x、y的范围
                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);
                //设置任务指针坐标
                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            }
            else
            {
                //把目标坐标转换场景坐标
                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
                //
                pointerRectTransform.localEulerAngles = Vector3.zero;
            }
        }
        /// <summary>
        /// 设置指针的角度
        /// </summary>
        private void RotatePointerTowardsTargetPosition()
        {
            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            //计算角度
            float angle = UtilsClass.GetAngleFromVectorFloat(dir);
            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        }
        /// <summary>
        /// 销毁游戏对象
        /// </summary>
        public void DestroySelf()
        {
            Destroy(pointerGameObject);
        }

    }
}
