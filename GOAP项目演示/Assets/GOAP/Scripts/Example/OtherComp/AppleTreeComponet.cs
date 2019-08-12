using UnityEngine;
using System.Collections;

public class AppleTreeComponet : MonoBehaviour
{
    public int AppleNum = 5;

    private float _costTime = 0;
    void Update()
    {
        if (AppleNum >= 10)
            return;
        _costTime += Time.deltaTime;
        if (_costTime >= 1)
        {
            AppleNum+=5;
            _costTime = 0;
        }
    }
}

