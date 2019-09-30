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
        if (tooltip.gameObject.activeInHierarchy && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(UtilityClass.GetMouseWorldPos(), Vector2.zero);
            if (hit.collider != null && hit.collider.GetComponent<UIView>().type == type && type != MenuType.Houses)
            {
                Create(hit.collider.transform);
            }
            else if (type == MenuType.Houses)
            {
                GameObject go = SimplePool.Spawn(ResourcesManager.LoadPrefab("Prefabs/房屋"), UtilityClass.GetMouseWorldPos(), Quaternion.identity);

                go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

                go.GetComponent<BuildView>().BuildTask(go, UtilityClass.GetMouseWorldPos());
            }
        }
        if (Input.GetMouseButtonDown(1) && tooltip.gameObject.activeInHierarchy)
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void OnClick(UIView material)
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
                transform.GetComponent<MiningView>().MiningTask(go, UtilityClass.GetMouseWorldPos());
                break;
            case MenuType.Felling:
                transform.GetComponent<TreeView>().FellingTreeTask(go, UtilityClass.GetMouseWorldPos());
                break;
            case MenuType.Houses:
                transform.GetComponent<BuildView>().BuildTask(go, UtilityClass.GetMouseWorldPos());
                break;
        }

    }
}
