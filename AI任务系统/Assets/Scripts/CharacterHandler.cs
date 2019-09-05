using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour
{
    public static CharacterHandler Instance;

    public float speed = 100f;
    /// <summary>
    /// 金矿位置
    /// </summary>
    public Transform goldNode;
    /// <summary>
    /// 储存点
    /// </summary>
    public Transform warehouse;
    public Sprite refuseSprite;
    public Sprite weaponSlotSprite;
    public Sprite weaponSprite;

    public GameObject character;

    private TaskSystem<Task> taskSystem;
    private static TaskSystem<TransporterTask> transporterTask;

    WeaponSlot weaponSlot;
    /// <summary>
    /// 创建类型 1 创建武器 2 创建武器槽 3 场景垃圾
    /// </summary>
    public int type;
    private void Awake()
    {
        Instance = this;
        taskSystem = new TaskSystem<Task>();
        transporterTask = new TaskSystem<TransporterTask>();

        Worker worker = Worker.Create(character, new Vector3(-10, -10));
        WorkerTaskAI workerTaskAI = worker.gameObject.AddComponent<WorkerTaskAI>();
        workerTaskAI.Setup(worker, taskSystem);

        worker = Worker.Create(character, new Vector3(10, 10));
        var workerTransporterTaskAI = worker.gameObject.AddComponent<WorkerTransporterTaskAI>();
        workerTransporterTaskAI.Setup(worker, transporterTask);

        var weaponSlotGo = CreateWeaponSlot(new Vector3(-10, -10));
        weaponSlot = new WeaponSlot(weaponSlotGo.transform);
       
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            type = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            type = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            type = 3;
        }
        if (Input.GetMouseButtonDown(0))
        {
            switch (type)
            {
                case 1:
                    GameObject weapon = CreateWeapon(UtilityClass.GetMouseWorldPos());
                    taskSystem.EnqueueTask(() =>
                    {
                        if (weaponSlot.IsEmpty())
                        {
                            weaponSlot.SetHasWeaponIncoming(true);
                            Task task = new Task.TaskWeaponToWeaponSlot
                            {
                                weaponPosition = weapon.transform.position,
                                weaponSlotPosition = weaponSlot.GetPosition(),
                                grabWeapon = (WorkerTaskAI workerAI) =>
                                {
                                    weapon.transform.SetParent(workerAI.transform);
                                },
                                dropWeapon = () =>
                                {
                                    weapon.transform.SetParent(null);
                                    weaponSlot.SetWeaponTransform(weapon.transform);
                                }
                            };
                            return task;
                        }
                        else
                        {
                            return null;
                        }
                    });
                    break;
                case 2: CreateWeaponSlot(UtilityClass.GetMouseWorldPos()); break;
                case 3: CreateTask(); break;
                default:
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Task task = new Task.Victory
            {
            };
            taskSystem.AddTask(task);
        }
    }

    public void CreateTask()
    {
        GameObject go = CreateSprite(UtilityClass.GetMouseWorldPos());
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        float cleanUpTime = Time.time + 5f;
        taskSystem.EnqueueTask(() =>
        {
            if (Time.time >= cleanUpTime)
            {
                Task task = new Task.ShellFloorCleanUp
                {
                    targetPosition = go.transform.position,
                    cleanUpAction = () =>
                    {
                        float alpha = 1f;
                        FunctionUpdater.Create(() =>
                        {
                            alpha -= Time.deltaTime;
                            spriteRenderer.color = new Color(1, 1, 1, alpha);
                            if (alpha <= 0f)
                            {
                                Destroy(go);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                };
                //taskSystem.AddTask(task);
                return task;
            }
            else
            {
                return null;
            }
        });
    }
    public GameObject CreateSprite(Vector3 position)
    {
        GameObject go = new GameObject("refuse", typeof(SpriteRenderer));
        go.transform.parent = transform;
        go.transform.position = position;
        go.GetComponent<SpriteRenderer>().sortingOrder = 1;
        go.GetComponent<SpriteRenderer>().sprite = refuseSprite;
        return go;
    }
    public GameObject CreateWeapon(Vector3 position)
    {
        GameObject go = new GameObject("Weapon", typeof(SpriteRenderer));
        go.transform.parent = transform;
        go.transform.position = position;
        go.GetComponent<SpriteRenderer>().sortingOrder = 1;
        go.GetComponent<SpriteRenderer>().sprite = weaponSprite;
        return go;
    }
    public GameObject CreateWeaponSlot(Vector3 position)
    {
        GameObject go = new GameObject("WeaponSlot", typeof(SpriteRenderer));
        go.transform.parent = transform;
        go.transform.position = position;
        go.GetComponent<SpriteRenderer>().sprite = weaponSlotSprite;
        go.GetComponent<SpriteRenderer>().sortingOrder = 1;
        go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        return go;
    }
    /// <summary>
    /// 武器插槽
    /// </summary>
    private class WeaponSlot
    {
        /// <summary>
        /// 武器插槽Transform
        /// </summary>
        private Transform weaponSlotTransform;
        private Transform weaponTransform;
        /// <summary>
        /// 武器是否放入
        /// </summary>
        private bool hasWeaponIncoming;
        public WeaponSlot(Transform weaponSlotTransform)
        {
            this.weaponSlotTransform = weaponSlotTransform;
            SetWeaponTransform(null);
        }
        /// <summary>
        /// 当前插槽是否为空
        /// </summary>
        public bool IsEmpty()
        {
            return weaponTransform == null && !hasWeaponIncoming;
        }
        public void SetHasWeaponIncoming(bool hasWeaponIncoming)
        {
            this.hasWeaponIncoming = hasWeaponIncoming;
            UpdateSprite();
        }
        /// <summary>
        /// 设置武器Transform
        /// </summary>
        public void SetWeaponTransform(Transform weaponTransform)
        {
            this.weaponTransform = weaponTransform;
            hasWeaponIncoming = false;
            UpdateSprite();

            if (weaponTransform != null)
            {
                Debug.Log("2222");
                TransporterTask.TakeWeaponFromSlotToPosition task = new TransporterTask.TakeWeaponFromSlotToPosition
                {
                    weaponPosition = GetPosition(),
                    targetPosition = GetPosition() + new Vector3(-50, 0),
                    grabWeapon = (WorkerTransporterTaskAI taskAI) =>
                    {
                        weaponTransform.SetParent(taskAI.transform);
                       // SetWeaponTransform(null);
                    },
                    dropWeapon = () =>
                    {
                        weaponTransform.SetParent(null);
                    }
                };
                transporterTask.AddTask(task);
            }
        }
        /// <summary>
        /// 修改sprite
        /// </summary>
        private void UpdateSprite()
        {
            weaponSlotTransform.GetComponent<SpriteRenderer>().color = IsEmpty() ? Color.gray : Color.red;
        }
        /// <summary>
        ///获取位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return weaponSlotTransform.position;
        }

    }
}

