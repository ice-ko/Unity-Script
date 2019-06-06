using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public bool isGameEndStarted;
    public GameObject StartEndCanvas;

    private LineFactory lineFactory;
    void Start()
    {
        lineFactory = FindObjectOfType<LineFactory>();
    }

    void Update()
    {
        if (lineFactory.isActiveAndEnabled && !isGameEndStarted)
        {
            Invoke("StartEndScreed", 2f);
        }
    }
    public void StartEndScreed()
    {
        StartEndCanvas.SetActive(true);
    }
}
