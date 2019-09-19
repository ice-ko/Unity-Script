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

    private MenuType type;
    void Start()
    {

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(UtilityClass.GetMouseWorldPos(), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<MaterialController>().type == type)
            {
                Create(hit.collider.transform);
            }
        }
        if (Input.GetMouseButtonDown(1) && tooltip.gameObject.activeInHierarchy)
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void Mining(MaterialController material)
    {
        if (baseUISpriteDic.ContainsKey(material.type))
        {
            tooltip.GetComponent<Image>().sprite = baseUISpriteDic[material.type];
            tooltip.transform.position = UtilityClass.GetWorldToScreenPos();
            tooltip.gameObject.SetActive(true);

            type = material.type;
        }
    }

    public void ShowOrHide(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }

    private void Create(Transform transform)
    {
        GameObject go = SimplePool.Spawn(toolTip, transform.position, Quaternion.identity);
        go.transform.SetParent(transform, false);
        go.GetComponent<SpriteRenderer>().sprite = baseSpriteDic[type];
        go.transform.position = transform.position;
        //
        switch (type)
        {
            case MenuType.Mining:
                MiningController.Instance.MiningTask(go, UtilityClass.GetMouseWorldPos());
                break;
            case MenuType.Felling:
                TreeController.Instance.FellingTreeTask(go, UtilityClass.GetMouseWorldPos());
                break;
        }

    }
}
