using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIBaseMenuController : MonoBehaviour
{
    //基础菜单sprite字典
    public BaseMenuDictionary baseSpriteDic;
    public BaseMenuDictionary baseUISpriteDic;
    public Image tooltip;

    public GameObject toolTip;

    private ButtonType buttonType;
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(UtilityClass.GetMouseWorldPos(), Vector2.zero);
            if (hit.collider != null)
            {
                switch (buttonType.type)
                {
                    case BaseMenuType.Mining:
                        GameObject go = SimplePool.Spawn(toolTip,Vector3.zero,Quaternion.identity);
                        go.transform.SetParent(hit.collider.transform, false);
                        go.GetComponent<SpriteRenderer>().sprite = baseSpriteDic[buttonType.type];
                        go.transform.position = hit.collider.transform.position;
                        MiningController.Instance.MiningTask(go, UtilityClass.GetMouseWorldPos());
                        break;
                    default:
                        break;
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && tooltip.gameObject.activeInHierarchy)
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void Mining(ButtonType button)
    {
        if (baseUISpriteDic.ContainsKey(button.type))
        {
            tooltip.GetComponent<Image>().sprite = baseUISpriteDic[button.type];
            tooltip.transform.position = UtilityClass.GetMouseWorldPos();
            tooltip.gameObject.SetActive(true);

            buttonType = button;
        }
    }

    public void ShowOrHide(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
}
