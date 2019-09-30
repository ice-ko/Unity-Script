using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : SingleObject<PlayerController>
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {

    }
    public void PlayMining(bool isPlay)
    {
        animator.SetBool("IsMining", isPlay);
    }
}
