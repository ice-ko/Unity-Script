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

public class GathererAI : MonoBehaviour {

    private enum State {
        Idle,
        MovingToResourceNode,
        GatheringResources,
        MovingToStorage,
    }

    private IUnit unit;
    private State state;
    private Transform resourceNodeTransform;
    private Transform storageTransform;
    private int goldInventoryAmount;

    private void Awake() {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;
    }

    private void Update() {
        switch (state) {
        case State.Idle:
            resourceNodeTransform = GameHandler.GetResourceNode_Static();
            state = State.MovingToResourceNode;
            break;
        case State.MovingToResourceNode:
            if (unit.IsIdle()) {
                unit.MoveTo(resourceNodeTransform.position, 10f, () => {
                    state = State.GatheringResources;
                });
            }
            break;
        case State.GatheringResources:
            if (unit.IsIdle()) {
                if (goldInventoryAmount > 0) {
                    // Move to storage
                    storageTransform = GameHandler.GetStorage_Static();
                    state = State.MovingToStorage;
                } else {
                    // Gather resources
                    unit.PlayAnimationMine(resourceNodeTransform.position, () => {
                        goldInventoryAmount++;
                    });
                }
            }
            break;
        case State.MovingToStorage:
            if (unit.IsIdle()) {
                unit.MoveTo(storageTransform.position, 10f, () => {
                    goldInventoryAmount = 0;
                    state = State.Idle;
                });
            }
            break;
        }
    }
}
