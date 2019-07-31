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
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    public Window_QuestPointer windowQuestPointer;
    private void Start()
    {
        //创建新的任务箭头图标
        Window_QuestPointer.QuestPointer questPointer_1 = windowQuestPointer.CreatePointer(new Vector3(-101, 60));
        //创建新的委托
        FunctionUpdater.Create(() =>
        {
            //如果到达目标范围40m左右就删除任务图标
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(-101, 60)) < 20f)
            {
                windowQuestPointer.DestroyPointer(questPointer_1);
                return true;
            }
            else
            {
                return false;
            }
        });
        //创建新的任务箭头图标
        Window_QuestPointer.QuestPointer questPointer_2 = windowQuestPointer.CreatePointer(new Vector3(57, 55));
        //创建新的委托
        FunctionUpdater.Create(() =>
        {
            //如果到达目标范围40m左右就删除任务图标
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(57, 55)) < 15f)
            {
                windowQuestPointer.DestroyPointer(questPointer_2);
                return true;
            }
            else
            {
                return false;
            }
        });
        //创建新的任务箭头图标
        Window_QuestPointer.QuestPointer questPointer_3 = windowQuestPointer.CreatePointer(new Vector3(120, 30));
        //创建新的委托
        FunctionUpdater.Create(() =>
        {
            //如果到达目标范围40m左右就删除任务图标
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(120, 30)) < 20f)
            {
                windowQuestPointer.DestroyPointer(questPointer_3);
                return true;
            }
            else
            {
                return false;
            }
        });
    }
}
