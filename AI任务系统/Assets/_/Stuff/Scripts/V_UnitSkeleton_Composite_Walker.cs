using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using V_ObjectSystem;

/*
 * Handle setting individual body parts to Walk or Idle based on V_IObjectWalker
 * */
public class V_UnitSkeleton_Composite_Walker {

    private V_Object parentObject;

    //private V_IObjectWalker objectWalker;
    private V_UnitSkeleton unitSkeleton;
    private UnitAnimType walkAnimType;
    private UnitAnimType idleAnimType;
    private string[] replaceBodyPartArray;

    public V_UnitSkeleton_Composite_Walker(V_Object parentObject, V_UnitSkeleton unitSkeleton, UnitAnimType walkAnimType, UnitAnimType idleAnimType, string[] replaceBodyPartArray) {
        this.parentObject = parentObject;
        //this.objectWalker = objectWalker;
        this.unitSkeleton = unitSkeleton;
        this.walkAnimType = walkAnimType;
        this.idleAnimType = idleAnimType;
        this.replaceBodyPartArray = replaceBodyPartArray;
    }

    public void UpdateBodyParts(bool isMoving, Vector3 dir) {
        //if (objectWalker.IsMoving()) {
        if (isMoving) {
            // Moving
            unitSkeleton.ReplaceBodyPartSkeletonAnim(walkAnimType.GetUnitAnim(dir), replaceBodyPartArray);
        } else {
            // Not moving
            unitSkeleton.ReplaceBodyPartSkeletonAnim(idleAnimType.GetUnitAnim(dir), replaceBodyPartArray);
        }
    }

}
