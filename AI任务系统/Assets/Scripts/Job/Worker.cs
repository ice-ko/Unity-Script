using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Worker : IWorker
{
    public GameObject gameObject;
    private Animator animator;

    public static Worker Create(GameObject gameObject, Vector3 position)
    {
        return new Worker(gameObject, position);
    }
    public Worker(GameObject gameObject, Vector3 position)
    {
        this.gameObject = GameObject.Instantiate(gameObject, position, Quaternion.identity);
    }
    /// <summary>
    /// 移动到指定位置。
    /// </summary>
    /// <param name="position">坐标</param>
    /// <param name="onArrivedAtPosition">到了位置.</param>
    public void MoveTo(Vector3 position, Action onArrivedAtPosition = null)
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Run");
        var move = gameObject.GetComponent<MoveHandler>();
        move.onArrivedAtPosition = onArrivedAtPosition;
        move.targetPosition = position;
    }
    /// <summary>
    /// 播放胜利动画。
    /// </summary>
    /// <param name="onAnimationComplete">动画完成.</param>
    public void PlayVictoryAnimation(Action onAnimationComplete)
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Victory");
        onAnimationComplete?.Invoke();
    }
    /// <summary>
    /// 播放清理动画。
    /// </summary>
    /// <param name="onAnimationComplete">动画完成.</param>
    public void PlayCleanUpAnimation(Action onAnimationComplete)
    {
        //animator.SetTrigger("Victory");
        var anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        anim.SetTrigger("Victory");
        onAnimationComplete?.Invoke();
    }

}