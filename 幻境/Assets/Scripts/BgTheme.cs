using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour
{
    private SpriteRenderer sr;
    private ManagerVars vars;
    void Start()
    {
        vars = ManagerVars.GetManagerVars();
        sr = GetComponent<SpriteRenderer>();

        //随机获取背景
        int ranValue = Random.Range(0,vars.bgTheme.Count);
        sr.sprite = vars.bgTheme[ranValue];
    }
	
    void Update()
    {
        
    }
}
