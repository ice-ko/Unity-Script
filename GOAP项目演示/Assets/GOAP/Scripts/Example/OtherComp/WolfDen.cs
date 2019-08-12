using UnityEngine;
using System.Collections;

public class WolfDen : MonoBehaviour
{
    public int WolfNum = 3;
    public float HuntRisk = 0.3f;

    private float _costTime = 0;
    void Update()
    {
        if (WolfNum >= 10)
            return;
        _costTime += Time.deltaTime;
        if (_costTime >= 1)
        {
            WolfNum+=3;
            _costTime = 0;
        }
    }
}
