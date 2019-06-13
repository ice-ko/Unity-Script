using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 物品槽
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itmePrefab;
    public int slotId;
    /// <summary>
    /// 把item放在当前物品槽
    /// 如果已经存在则数量加1 
    /// 如果没有则实例化新的物品item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent">父对象</param>
    public void StoreItem(Item item, Transform parent = null)
    {
        parent = parent == null ? transform : parent;
        if (parent.childCount == 0)
        {
            //创建新的物品
            var itemObj = Instantiate(itmePrefab);
            itemObj.transform.SetParent(parent, false);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.GetComponent<ItemUI>().SetItem(item);

        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }
    /// <summary>
    /// 获取当前物品槽存储的物品信息
    /// </summary>
    /// <returns></returns>
    public ItemUI GetItme()
    {
        return transform.GetChild(0).GetComponent<ItemUI>();
    }
    /// <summary>
    /// 鼠标进入
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string text = transform.GetChild(0).GetComponent<ItemUI>().item.GetToolTipText();
            InventoryManager.Instance.ShowToolTip(text);
        }
    }
    /// <summary>
    /// 鼠标移出
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.HideToolTip();
    }
    /// <summary>
    /// 鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //获取跟随鼠标移动的物品槽
        var moveItemParent = InventoryManager.Instance.moveParentTransform;
        //获取跟鼠标移动的物品itemUI脚本
        var moveItem = InventoryManager.Instance.clickItemUI;
        if (transform.childCount > 0)
        {
            //获取当前物品槽下的子物体
            var currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            //按下左边ctrl 并且当前物品数量必须大于1才允许拆分物品
            if (Input.GetKey(KeyCode.LeftControl) && currentItem.amount > 1)
            {
                if (InventoryManager.Instance.isClickItem && currentItem.item.Id != moveItem.item.Id)
                {
                    SetItemParent();
                    return;
                }
                var count = currentItem.amount / 2;
                //没有移动的物品并且数量小于0 则创建信息的物品
                if (count <= 0)
                {
                    //在当前物品槽实例化一个新的子物体
                    Destroy(transform.GetChild(0).gameObject);
                    moveItem.AddAmount();
                }
                else
                {
                    //移动物品槽无可移动子物体则新建子物体
                    if (moveItemParent.childCount == 0)
                    {
                        this.StoreItem(currentItem.item, moveItemParent);
                        currentItem.AddAmount(-count);
                        moveItemParent.GetChild(0).GetComponent<ItemUI>().AddAmount(count-1);
                    }
                    else
                    {
                        //拆分物品 在当前物品总数量减去一半并创建新的物品 放到跟随鼠标移动的物品槽中
                        currentItem.AddAmount(-count);
                        moveItemParent.GetChild(0).GetComponent<ItemUI>().AddAmount(count);
                    }
                    //
                    InventoryManager.Instance.isClickItem = true;
                    InventoryManager.Instance.clickItemUI = moveItemParent.GetChild(0).GetComponent<ItemUI>();
                }
            }
            else
            {
                //判断是否有跟随鼠标移动的物品槽如果有则判断是否同一个物品 如果是则添加数量
                if (InventoryManager.Instance.isClickItem && currentItem.item.Id == moveItem.item.Id)
                {
                    //相同物品为达到最大容量则添加数量
                    if (currentItem.item.Capacity > currentItem.amount && currentItem.amount >= moveItem.amount)
                    {
                        currentItem.AddAmount();
                        moveItem.amount--;
                        moveItem.RefreshAmount();
                    }
                    else
                    {
                        //如果拆分的物品数量等于1则交换两位物品
                        SetItemParent();
                    }
                    if (moveItem.amount <= 0)
                    {
                        Destroy(moveItem.gameObject);
                        InventoryManager.Instance.isClickItem = false;
                        InventoryManager.Instance.clickItemUI = null;
                    }
                }
                else if (!InventoryManager.Instance.isClickItem)
                {
                    SetMoveItemParent();
                }
                else
                {
                    SetItemParent();
                }
            }
        }
        else if (InventoryManager.Instance.isClickItem)
        {
            //把当前跟随鼠标的物品放置在点击的空物品槽上
            moveItemParent.GetChild(0).transform.SetParent(transform);
            transform.GetChild(0).transform.localPosition = Vector3.zero;
            InventoryManager.Instance.isClickItem = false;
            InventoryManager.Instance.clickItemUI = null;
        }
    }
    /// <summary>
    /// 设置item新的父对象（跟随鼠标移动）
    /// </summary>
    public void SetMoveItemParent()
    {
        var moveItemParent = InventoryManager.Instance.moveParentTransform;
        //获取当前点击的物品槽子物体
        var newItem = transform.GetChild(0).GetComponent<ItemUI>();
        //把跟随鼠标移动的物品槽子物体父对象设置为当前点击的物品槽
        // moveItemParent.GetChild(0).transform.SetParent(transform);
        //把当前点击的物品槽子物体父对象设置为跟随鼠标移动物品槽
        newItem.transform.SetParent(moveItemParent);
        //重置本地位置
        moveItemParent.GetChild(0).transform.localPosition = Vector3.zero;
        //newItem.transform.localPosition = Vector3.zero;
        //
        InventoryManager.Instance.clickItemUI = newItem;
        InventoryManager.Instance.isClickItem = true;
    }
    /// <summary>
    /// 设置item新的父对象（交换物品）
    /// </summary>
    public void SetItemParent()
    {
        var moveItemParent = InventoryManager.Instance.moveParentTransform;
        //获取当前点击的物品槽子物体
        var newItem = transform.GetChild(0).GetComponent<ItemUI>();
        //把跟随鼠标移动的物品槽子物体父对象设置为当前点击的物品槽
        moveItemParent.GetChild(0).transform.SetParent(transform);
        //把当前点击的物品槽子物体父对象设置为跟随鼠标移动物品槽
        newItem.transform.SetParent(moveItemParent);
        //重置本地位置
        moveItemParent.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        //
        InventoryManager.Instance.clickItemUI = newItem;
        //InventoryManager.Instance.isClickItem = true;
    }
}
