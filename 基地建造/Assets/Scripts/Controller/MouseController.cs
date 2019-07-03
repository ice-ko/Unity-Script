using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public GameObject circleCursorPrefab;//

    // 鼠标最后一帧的世界位置。
    Vector3 lastFramePosition;
    Vector3 currFramePosition;

    //我们的左键拖动操作的世界位置开始
    Vector3 dragStartPosition;
    List<GameObject> dragPreviewGameObjects = new List<GameObject>();
    void Start()
    {

    }

    void Update()
    {
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        // UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();

        // 从此框架中保存鼠标位置
        //我们不使用currFramePosition，因为我们可能已经移动了相机。
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }
    /// <summary>
    ///获取当前位置的tile
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);
        return WorldController.instance.world.GetTileAt(x, y);
    }
    /// <summary>
    /// 拖动
    /// </summary>
    void UpdateDragging()
    {
        //检测是否点击ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // 开始拖动
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currFramePosition;
        }

        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currFramePosition.x);
        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currFramePosition.y);

        //我们可能会拖向“错误”的方向，所以如果需要可以翻转。
        if (end_x < start_x)
        {
            int tmp = end_x;
            end_x = start_x;
            start_x = tmp;
        }
        if (end_y < start_y)
        {
            int tmp = end_y;
            end_y = start_y;
            start_y = tmp;
        }

        // 清理旧拖动预览
        while (dragPreviewGameObjects.Count > 0)
        {
            GameObject go = dragPreviewGameObjects[0];
            dragPreviewGameObjects.RemoveAt(0);
            SimplePool.Despawn(go);
        }

        if (Input.GetMouseButton(0))
        {
            // 显示拖动区域的预览
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.instance.world.GetTileAt(x, y);
                    if (t != null)
                    {
                        // 在此图块位置顶部显示建筑物提示
                        GameObject go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGameObjects.Add(go);
                    }
                }
            }
        }

        //结束拖动
        if (Input.GetMouseButtonUp(0))
        {
            var buildMode = GameObject.FindObjectOfType<BuildModeController>();
            // 循环遍历所有瓷砖
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.instance.world.GetTileAt(x, y);
                    if (t != null)
                    {
                        buildMode.DoBuild(t);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 更新相机位置
    /// </summary>
    void UpdateCameraMovement()
    {
        // 处理屏幕平移
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {   // 鼠标右键或鼠标中键
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }
        //摄像机缩放
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        //限制摄像机缩放范围
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }
}
