/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUnit {
    
    bool IsIdle();
    void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition);
    void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted);

}