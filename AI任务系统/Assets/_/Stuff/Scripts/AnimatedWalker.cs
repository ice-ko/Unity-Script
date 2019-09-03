using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;

public class AnimatedWalker {

    private Vector3 lastMoveVector;
    public  V_UnitAnimation unitAnimation;
    private UnitAnimType idleAnimType;
    private UnitAnimType walkAnimType;
    private float idleFrameRate;
    private float walkFrameRate;

    public AnimatedWalker(V_UnitAnimation unitAnimation, UnitAnimType idleAnimType, UnitAnimType walkAnimType, float idleFrameRate, float walkFrameRate) {
        this.unitAnimation = unitAnimation;
        this.idleAnimType = idleAnimType;
        this.walkAnimType = walkAnimType;
        this.idleFrameRate = idleFrameRate;
        this.walkFrameRate = walkFrameRate;
        lastMoveVector = new Vector3(0, -1);
        unitAnimation.PlayAnim(idleAnimType, lastMoveVector, idleFrameRate, null, null, null);
    }

    public void SetMoveVector(Vector3 moveVector) {
        if (moveVector == Vector3.zero) {
            // Idle
            unitAnimation.PlayAnim(idleAnimType, lastMoveVector, idleFrameRate, null, null, null);
        } else {
            // Moving
            lastMoveVector = moveVector;
            unitAnimation.PlayAnim(walkAnimType, lastMoveVector, walkFrameRate, null, null, null);
        }
    }

    public Vector3 GetLastMoveVector() {
        return lastMoveVector;
    }



}
