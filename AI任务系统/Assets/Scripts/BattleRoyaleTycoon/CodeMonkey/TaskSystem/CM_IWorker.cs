/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM_TaskSystem {

    public interface CM_IWorker {
        
        Vector3 GetPosition();
        void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
        void PlayVictoryAnimation(Action onAnimationComplete);
        void PlayCleanUpAnimation(Action onAnimationComplete);

    }

}