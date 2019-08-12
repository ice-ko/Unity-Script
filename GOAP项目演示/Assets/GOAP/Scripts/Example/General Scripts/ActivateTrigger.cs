using UnityEngine;

public class ActivateTrigger : MonoBehaviour {
	public enum Mode {
        /// <summary>
        /// 只需将操作广播到目标
        /// </summary>
		Trigger = 0, // Just broadcast the action on to the target
        /// <summary>
        /// 用源替换目标
        /// </summary>
		Replace = 1, // replace target with source
        /// <summary>
        /// 激活目标GameObject
        /// </summary>
		Activate = 2, // Activate the target GameObject
        /// <summary>
        /// 启用组件
        /// </summary>
		Enable = 3, // Enable a component
        /// <summary>
        /// 在目标上开始动画
        /// </summary>
		Animate = 4, // Start animation on target
        /// <summary>
        /// 停用目标GameObject
        /// </summary>
		Deactivate = 5 // Decativate target GameObject
	}

    ///<summary>
    /// The action to accomplish
    ///要完成的行动
    ///</summary>
    public Mode action = Mode.Activate;

    /// <summary>
    /// The game object to affect. If none, the trigger work on this game object
    /// 影响游戏对象。 如果没有，则触发器对此游戏对象起作用
    /// </summary>
    public Object target;
	public GameObject source;
	public int triggerCount = 1;///
	public bool repeatTrigger = false;
	
	void DoActivateTrigger () {
		triggerCount--;

		if (triggerCount == 0 || repeatTrigger) {
			Object currentTarget = target != null ? target : gameObject;
			Behaviour targetBehaviour = currentTarget as Behaviour;
			GameObject targetGameObject = currentTarget as GameObject;
			if (targetBehaviour != null)
				targetGameObject = targetBehaviour.gameObject;
		
			switch (action) {
				case Mode.Trigger:
					targetGameObject.BroadcastMessage ("DoActivateTrigger");
					break;
				case Mode.Replace:
					if (source != null) {
						Object.Instantiate (source, targetGameObject.transform.position, targetGameObject.transform.rotation);
						DestroyObject (targetGameObject);
					}
					break;
				case Mode.Activate:
					targetGameObject.SetActive(true);
					break;
				case Mode.Enable:
					if (targetBehaviour != null)
						targetBehaviour.enabled = true;
					break;	
				case Mode.Animate:
					targetGameObject.GetComponent<Animation>().Play ();
					break;	
				case Mode.Deactivate:
					targetGameObject.SetActive(false);
					break;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		DoActivateTrigger ();
	}
}