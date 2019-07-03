using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    float soundCooldown = 0f;
    void Start()
    {
        WorldController.instance.world.RegisterFurnitureCreated(OnFurnitureCreated);
        WorldController.instance.world.RegisterTileChanged(OnTileTypeChanged);
    }

    void Update()
    {
        soundCooldown -= Time.deltaTime;
    }
    /// <summary>
    /// tile 改变时
    /// </summary>
    void OnTileTypeChanged(Tile tile)
    {
        if (soundCooldown > 0)
        {
            return;
        }
        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/Floor_OnCreated");
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }
    /// <summary>
    /// 创建家具、围墙
    /// </summary>
    void OnFurnitureCreated(Furniture furniture)
    {
        if (soundCooldown > 0)
        {
            return;
        }

        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + furniture.objectType + "_OnCreated");
        if (audioClip==null)
        {
            audioClip = Resources.Load<AudioClip>("Sounds/Wall_OnCreated");
        }
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        soundCooldown = 0.1f;
    }
}
