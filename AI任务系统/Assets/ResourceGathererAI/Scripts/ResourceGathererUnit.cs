using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class ResourceGathererUnit : MonoBehaviour, IUnit {
    
    private enum State {
        Idle,
        Moving,
        Animating,
    }

    private const float speed = 30f;

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private Vector3 targetPosition;
    private float stopDistance;
    private Action onArrivedAtPosition;
    private State state;


    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, UnitAnimType.GetUnitAnimType("dBareHands_Idle"), UnitAnimType.GetUnitAnimType("dBareHands_Walk"), 1f, 1f);
    }

    private void Update() {
        switch (state) {
        case State.Idle:
            animatedWalker.SetMoveVector(Vector3.zero);
            break;
        case State.Moving:
            HandleMovement();
            break;
        case State.Animating:
            break;
        }
        unitSkeleton.Update(Time.deltaTime);
        /*
        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition(UtilsClass.GetMouseWorldPosition());
        }*/
    }

    private void HandleMovement() {
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance) {
            Vector3 moveDir = (targetPosition - transform.position).normalized;

            float distanceBefore = Vector3.Distance(transform.position, targetPosition);
            animatedWalker.SetMoveVector(moveDir);
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
        } else {
            // Arrived
            animatedWalker.SetMoveVector(Vector3.zero);
            if (onArrivedAtPosition != null) {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                state = State.Idle;
                tmpAction();
            }
        }
    }


    public void SetTargetPosition(Vector3 targetPosition) {
        targetPosition.z = 0f;
        this.targetPosition = targetPosition;
    }

    public bool IsIdle() {
        return state == State.Idle;
    }

    public void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition) {
        SetTargetPosition(position);
        this.stopDistance = stopDistance;
        this.onArrivedAtPosition = onArrivedAtPosition;
        state = State.Moving;
    }

    public void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted) {
        state = State.Animating;
        string anim = "MineDownRight";
        if (lookAtPosition.x > transform.position.x) {
            anim = "MineDownRight";
        } else {
            anim = "MineDownLeft";
        }
        animatedWalker.unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim(anim), 1f, () => {
            state = State.Idle;
            if (onAnimationCompleted != null) {
                onAnimationCompleted();
            }
        });
        /*animatedWalker.unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("MineDownRight"), 1f, () => {
            animatedWalker.unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("MineDownRight"), 1f, () => {
                animatedWalker.unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("MineDownRight"), 1f, () => {
                    state = State.Idle;
                    onAnimationCompleted();
                });
            });
        });*/
    }

}