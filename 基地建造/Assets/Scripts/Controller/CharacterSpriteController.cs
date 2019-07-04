using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteController : MonoBehaviour
{
    Dictionary<Character, GameObject> characterGameObjectMap;
    Dictionary<string, Sprite> characterSprite;

    public World world { get { return WorldController.instance.world; } }
    void Start()
    {
        LoadSprites();
        //
        characterGameObjectMap = new Dictionary<Character, GameObject>();

        //注册委托
        world.RegisterCharacterCreated(OnCharacterCreated);
        Character character = world.CreateCharacter(world.GetTileAt(world.width / 2, world.height / 2));
       // character.SetDestination(world.GetTileAt(world.width / 2 + 5, world.height / 2));
    }

    void Update()
    {

    }
    void LoadSprites()
    {
        //加载Sprite
        characterSprite = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Characters");
        foreach (var item in sprites)
        {
            characterSprite.Add(item.name, item);
        }
    }
    /// <summary>
    /// 创建Furniture
    /// </summary>
    /// <param name="obj"></param>
    public void OnCharacterCreated(Character obj)
    {
        // 创建链接到此数据的可视GameObject。

        // FIXME：不考虑多瓦片对象也不考虑旋转对象

        //这会创建一个新的GameObject并将其添加到我们的场景中。
        GameObject obj_go = new GameObject();
        //将我们的tile / GO对添加到字典中。
        characterGameObjectMap.Add(obj, obj_go);

        obj_go.name = "Character_" + obj.X + "_" + obj.Y;
        obj_go.transform.position = new Vector3(obj.currTile.x, obj.currTile.y, 0);
        obj_go.transform.parent = transform;
        // FIXME：我们假设对象必须是墙，所以请 
        //添加SpriteRenderer
        var sr = obj_go.AddComponent<SpriteRenderer>();
        sr.sprite = characterSprite["p1_front"];
        sr.sortingLayerName = "Character";
        //注册我们的回调，以便我们的GameObject随时更新
        obj.RegisterCharacterCreated(OnCharacterChanged);
    }
    /// <summary>
    /// 替换指定的Character sprite
    /// </summary> 
    /// <param name="furniture"></param>
    void OnCharacterChanged(Character  character)
    {
        if (!characterGameObjectMap.ContainsKey(character))
        {
            return;
        }
        GameObject obj_go = characterGameObjectMap[character];
        obj_go.transform.position= new Vector3(character.X, character.Y, 0);
    }
}
