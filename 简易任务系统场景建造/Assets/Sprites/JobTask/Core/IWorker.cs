using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 工人
/// </summary>
public interface IWorker
{
    /// <summary>
    /// 移动到指定位置。
    /// </summary>
    /// <param name="position">坐标</param>
    /// <param name="onArrivedAtPosition">到了位置.</param>
    void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    /// <summary>
    ///播放胜利动画。
    /// </summary>
    /// <param name="onAnimationComplete">动画完成.</param>
    void PlayVictoryAnimation(Action onAnimationComplete);
    /// <summary>
    /// 播放清理动画。
    /// </summary>
    /// <param name="onAnimationComplete">动画完成.</param>
    void PlayCleanUpAnimation(Action onAnimationComplete);
}