using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Sound_Manager {

    private class MonoBehaviourDummy : MonoBehaviour { }

    public enum Sound {
        None,
        BulletTime_In,
        BulletTime_Out,
        Dash,
        Dash_2,
        Dash_3,
        Dash_4,
        Player_Counter,
        Sword_Hit,
        Sword_Kill,
        Sword_Kill_2,
        Sword_Wrong,
        Sword_Draw,
        Sword_Sheathe,
        Sword,
        Sword_2,
        Sword_3,
        Sword_4,
        Sword_5,
        Sword_6,
        Sword_7,
        ComboButton,
        Footsteps_Hero,
        Footstep_Minion,
        Footstep_Minion_2,
        Footstep_Minion_3,
        Footstep_Minion_4,
        Footstep_Minion_5,
        Footstep_Minion_6,
        Combo_Healing,
        Combo_Earthshatter,
        Countered,
        Countered_2,
        Menu_Over,
        Menu_Click,
        Open_Map,
        Arrow_Fire,
        Arrow_Hit,
        Arrow_Deflect,
        Minion_Sword,
        Minion_Sword_2,
        Minion_Sword_3,
        Minion_Sword_4,
        Minion_Sword_5,
        Minion_Sword_6,
        Minion_Sword_7,
        Minion_Sword_8,
        Minion_Sword_9,
        Minion_Sword_10,
        Minion_Sword_11,
        Minion_Dead,
        Minion_Dead_2,
        Minion_Dead_3,
        Player_Hit,
        Player_Hit_2,
        Player_Hit_3,
        Player_Hit_4,
        Player_Hit_5,

        ExperienceTick,
        Ding,

        Victory,
        Defeat,
        Pickup,
        
        Rifle_Fire,
        Rifle_Fire_1,
        Rifle_Fire_2,
        Rifle_Fire_3,
        Rifle_Fire_4,

        Building,
        Demolish,
        TimeSwoosh,
        Warning,
        Axe_1,
        Axe_2,
        Axe_3,
        Axe_4,
        Axe_5,
        Axe_6,
        Pickaxe_1,
        Pickaxe_2,
        Pickaxe_3,
        Pickaxe_4,
        Pickaxe_5,
        Talking,
        Gong,
        Coins,
        AsianBonus,
    }
    public enum AudioType {
        Master,
        Music,
        Sound
    }

	private static bool isInit = false;
	private static Dictionary<Sound,AudioClip> dictionary;
	private static Dictionary<Sound,float> timers;
    private static bool disableAllSounds = false;

    private static GameObject globalGameObject;
    public delegate float DelGetAudio(AudioType audioType);
    private static DelGetAudio GetAudioFunc;

    public static void Init() {
        Sound_Manager.Init((Sound_Manager.AudioType audioType) => .1f);
    }
	public static void Init(DelGetAudio GetAudioFunc) {
		if (isInit) return;
		Debug.Log("Loading Sounds...");
		isInit = true;
        Sound_Manager.GetAudioFunc = GetAudioFunc;
        CreateGlobalObject();
		dictionary = new Dictionary<Sound,AudioClip>();
		timers = new Dictionary<Sound,float>();

        foreach(Sound sound in System.Enum.GetValues(typeof(Sound))) {
            if (sound == Sound.None) continue;
            Load(sound);
        }
        PoolObjects();
	}
    private static void CreateGlobalObject() {
        globalGameObject = new GameObject();
        Object.DontDestroyOnLoad(globalGameObject);
        globalGameObject.AddComponent<MonoBehaviourDummy>();
    }
	public static void Load(Sound sound, string folder = "") {
        if (folder == "") folder = "Sounds/"; //Default sound folder
		dictionary[sound] = (AudioClip) Resources.Load(folder+sound);
		if (dictionary[sound] == null) Debug.Log("Couldnt load "+folder+sound);
		timers[sound] = 0f;
	}
	public static float GetSoundLength(Sound sound) {
		if (!dictionary.ContainsKey(sound)) {
			Debug.Log("Sound not found: "+sound);
			return 0f;
		}
		return dictionary[sound].length;
	}
    public static void DisableAllSounds() {
        disableAllSounds = true;
    }
    public static void EnableAllSounds() {
        disableAllSounds = false;
    }
    /*
    public static AudioClip GetAudioClip(CustomSound.Type unitSound) {
        Sound sound = ConvertUnitSoundToSound(unitSound);
		//Init();
        if (disableAllSounds) return null;
		if (!dictionary.ContainsKey(sound)) {
			Debug.Log("Sound not found: "+sound);
			return null;
		}
        return dictionary[sound];
    }
    public static Sound ConvertUnitSoundToSound(CustomSound.Type unitSound) {
        switch (unitSound) {
        default:
        case CustomSound.Type.Sword:              return Sound.Minion_Sword;
        case CustomSound.Type.Gunshot_Rifle:      return Sound.Rifle_Fire;
        case CustomSound.Type.Crash:              return Sound.Combo_Earthshatter;
        }
    }*/
    public static float GetAudio(AudioType audioType) {
        return GetAudioFunc(audioType);
    }
    /*public static AudioSource PlaySound(CustomSound.Type unitSound, Vector3 pos = default(Vector3), object obj = null) {
        return PlaySound(ConvertUnitSoundToSound(unitSound), pos, obj);
    }*/
    /*public static AudioSource PlaySound(CustomSound unitSound, Vector3 pos) {
        return PlaySound(unitSound.audioClip, true, pos);
    }*/
	public static AudioSource PlaySound(AudioClip audioClip, bool usePosition = false, Vector3 pos = default(Vector3)) {
        if (usePosition) {
            return PlayClipAtPoint(audioClip, pos);
        } else {
            return PlayClip(audioClip);
        }
    }
	public static AudioSource PlaySound(Sound sound, Vector3 pos = default(Vector3), object obj = null) {
        //Window_DebugMessages.AddMessage(sound);
		//Init();
        if (disableAllSounds) return null;
		if (!dictionary.ContainsKey(sound)) {
			Debug.Log("Sound not found: "+sound);
			return null;
		}
		switch (sound) {
        default:
            return PlayClip(dictionary[sound]);
        case Sound.Victory:
            return PlayClip(dictionary[sound], 90);
            //return PlayClipAtPoint(dictionary[sound], pos);
        case Sound.Arrow_Fire:
        case Sound.Arrow_Hit:
        case Sound.Arrow_Deflect:
        case Sound.Sword_Draw:
        case Sound.Sword_Sheathe:
            //return PlayClipAtPoint(dictionary[sound], pos);
            return PlayClip(dictionary[sound]);
        case Sound.Sword_Hit:
        case Sound.Sword_Wrong:
            return PlayClip(dictionary[sound]);
            /*if (CanPlay(sound)) {
                if (Time.realtimeSinceStartup - timers[sound] > .1f)
                    timers[sound] = Time.realtimeSinceStartup - .05f;
                timers[sound] += .01f;
                return PlayClipAtPoint(dictionary[sound], pos, 100);
            } return null;*/
        case Sound.Dash:
            Sound[] sounds = new[] { Sound.Dash, Sound.Dash_2, Sound.Dash_3, Sound.Dash_4 };
            return PlayClip(dictionary[sounds[Random.Range(0, sounds.Length)]]);
            //return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100);
        case Sound.Sword_Kill:
            if (CanPlay(sound)) {
                if (Time.realtimeSinceStartup - timers[sound] > .1f)
                    timers[sound] = Time.realtimeSinceStartup - .03f;
                timers[sound] += .01f;
                sounds = new[] { Sound.Sword_Kill, Sound.Sword_Kill_2, };
                return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100);
            }
            return null;
        case Sound.Sword:
            sounds = new[] { Sound.Sword, Sound.Sword_2, Sound.Sword_3, Sound.Sword_4, Sound.Sword_5, Sound.Sword_6, Sound.Sword_7 };
            return PlayClip(dictionary[sounds[Random.Range(0, sounds.Length)]]);
            //return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100);
        case Sound.Footsteps_Hero:
            return PlayClipAtPoint(dictionary[sound], pos, true);
        case Sound.Footstep_Minion:
            sounds = new[] { Sound.Footstep_Minion, Sound.Footstep_Minion_2, Sound.Footstep_Minion_3, Sound.Footstep_Minion_4, Sound.Footstep_Minion_5, Sound.Footstep_Minion_6 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 140, .5f * GetAudio(AudioType.Sound));
        /*case Sound.Minion_Dead:
            if (CanPlay(sound)) {
                if (Time.realtimeSinceStartup - timers[sound] > .1f)
                    timers[sound] = Time.realtimeSinceStartup - .05f;
                timers[sound] += .01f;
                sounds = new[] { Sound.Minion_Dead, Sound.Minion_Dead_2, Sound.Minion_Dead_3 };
                return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos);
            }
            return null;*/
        case Sound.Countered:
            sounds = new[] { Sound.Countered, Sound.Countered_2 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos);
        case Sound.Player_Hit:
            sounds = new[] { Sound.Player_Hit, Sound.Player_Hit_2, Sound.Player_Hit_3, Sound.Player_Hit_4, Sound.Player_Hit_5 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100);
            
            
        case Sound.Minion_Sword:
            if (!CanPlay(sound)) return null;
            timers[sound] = Time.realtimeSinceStartup + Random.Range(.05f, .2f);
            sounds = new[] { Sound.Minion_Sword, Sound.Minion_Sword_2, Sound.Minion_Sword_3, Sound.Minion_Sword_4, Sound.Minion_Sword_5, Sound.Minion_Sword_6, Sound.Minion_Sword_7, Sound.Minion_Sword_8, Sound.Minion_Sword_9, Sound.Minion_Sword_10, Sound.Minion_Sword_11 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos);

        case Sound.Minion_Dead:
            if (!CanPlay(sound)) return null;
            timers[sound] = Time.realtimeSinceStartup + Random.Range(.05f, .2f);
            sounds = new[] { Sound.Minion_Dead, Sound.Minion_Dead_2, Sound.Minion_Dead_3 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos);
            
        case Sound.Axe_1:
            if (!CanPlay(sound)) return null;
            timers[sound] = Time.realtimeSinceStartup + Random.Range(.05f, .2f);
            sounds = new[] { Sound.Axe_1, Sound.Axe_2, Sound.Axe_3, Sound.Axe_4, Sound.Axe_5, Sound.Axe_6 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100, .1f);

        case Sound.Pickaxe_1:
            if (!CanPlay(sound)) return null;
            timers[sound] = Time.realtimeSinceStartup + Random.Range(.05f, .2f);
            sounds = new[] { Sound.Pickaxe_1, Sound.Pickaxe_2, Sound.Pickaxe_3, Sound.Pickaxe_4, Sound.Pickaxe_5 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos, 100, .1f);

        case Sound.Rifle_Fire:
            if (!CanPlay(sound)) return null;
            /*
            if (Time.realtimeSinceStartup - timers[sound] > .1f)
                timers[sound] = Time.realtimeSinceStartup - .03f;
            timers[sound] += .01f;
            */
            timers[sound] = Time.realtimeSinceStartup + Random.Range(.05f, .1f);
            sounds = new[] { Sound.Rifle_Fire_1, Sound.Rifle_Fire_2, Sound.Rifle_Fire_3, Sound.Rifle_Fire_4 };
            return PlayClipAtPoint(dictionary[sounds[Random.Range(0, sounds.Length)]], pos);


        case Sound.TimeSwoosh:
            if (!CanPlay(sound)) return null;
            timers[sound] = Time.realtimeSinceStartup + .1f;
            return PlayClip(dictionary[sound]);
        case Sound.Building:
            return PlayClipAtPoint(dictionary[sound], pos);
        case Sound.Talking:
            return PlayClip(dictionary[sound], true);
		}
	}
	public static bool CanPlay(Sound sound) {
		return Time.realtimeSinceStartup > timers[sound];
	}
	private static AudioSource PlayClip(AudioClip clip) {
		AudioSource src = PlayClipAtPoint(clip, Vector3.zero);
		src.spatialBlend = 0f;
		return src;
	}
	private static AudioSource PlayClip(AudioClip clip, bool loop) {
		AudioSource src = PlayClipAtPoint(clip, Vector3.zero, 128, GetAudio(AudioType.Sound), loop);
		src.spatialBlend = 0f;
		return src;
	}
	private static AudioSource PlayClip(AudioClip clip, int priority) {
		AudioSource src = PlayClipAtPoint(clip, Vector3.zero, priority, GetAudio(AudioType.Sound));
		src.spatialBlend = 0f;
		return src;
	}
	private static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos) {
		return PlayClipAtPoint(clip, pos, 128);
	}
	private static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, bool loop) {
		return PlayClipAtPoint(clip, pos, 128, 1f, loop);
	}
	private static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, int priority) {
		return PlayClipAtPoint(clip, pos, priority, GetAudio(AudioType.Sound), false);
	}
	private static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, int priority, float volMultiplier) {
		return PlayClipAtPoint(clip, pos, priority, GetAudio(AudioType.Sound) * volMultiplier, false);
	}
	private static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, int priority, float vol, bool loop = false) {
		GameObject tempGO = GetPooledObject();
		AudioSource aSource = tempGO.GetComponent<AudioSource>();
		tempGO.transform.position = pos; // set its position
		aSource.clip = clip; // define the clip
		aSource.priority = priority;
		// set other aSource properties here, if desired
		aSource.spatialBlend = 1f;//0.5f;
        aSource.dopplerLevel = .0f;
        aSource.minDistance = 60;
        aSource.maxDistance = 400;
        aSource.rolloffMode = AudioRolloffMode.Linear;
		aSource.volume = vol;
        if (!loop) {
            aSource.loop = false;
		    tempGO.SetActive(true);
		    globalGameObject.GetComponent<MonoBehaviourDummy>().StartCoroutine(DisablePooledObject(tempGO, clip.length));
        } else {
            aSource.loop = true;
            tempGO.SetActive(true);
        }
		aSource.Play(); // start the sound
		return aSource; // return the AudioSource reference
	}
    public static void StopSound(AudioSource audioSource) {
        audioSource.Stop();
        audioSource.transform.gameObject.SetActive(false);
    }
    public static void StopSound(AudioSource audioSource, string clipName) {
        if (clipName == audioSource.clip.name)
            StopSound(audioSource);
    }
	public static IEnumerator DisablePooledObject(GameObject obj, float time) {
		yield return globalGameObject.GetComponent<MonoBehaviourDummy>().StartCoroutine(WaitForRealSeconds(time)); // waits
		if (obj != null) {
			obj.SetActive(false);
        }
	}
    public static IEnumerator WaitForRealSeconds(float time) {
         float start = Time.realtimeSinceStartup;
         while (Time.realtimeSinceStartup < start + time) {
             yield return null;
         }
     }
	private static GameObject[] poolArr = null;
	private static int poolAmount = 30;
    private static int poolAmountMax = 50;
    /*public static void Init() {
        PoolObjects();
    }*/
	public static void PoolObjects() {
		//Init();
		List<GameObject> poolList = new List<GameObject>();
		for (int i=0; i<poolAmount; i++) {
			GameObject tempGO = new GameObject("Pooled_Audio_"+i, typeof(AudioSource)); // create the temp object
			//tempGO.AddComponent<AudioSource>(); // add an audio source
			tempGO.SetActive(false);
			poolList.Add(tempGO);
		}
		poolArr = poolList.ToArray();
	}
	private static GameObject GetPooledObject() {
		if (poolArr == null || poolArr.Length < poolAmount || poolArr[0] == null) PoolObjects();
		for (int i=0; i<poolAmount; i++) {
			if (poolArr[i] != null && !poolArr[i].activeSelf) {
				return poolArr[i];
			}
		}
        if (poolAmount >= poolAmountMax) {
            // Maximum objects in pool, dont create more!
            return poolArr[Random.Range(0, poolArr.Length)];
        }
		GameObject tempGO = new GameObject("Pooled_Audio_"+poolAmount, typeof(AudioSource)); // create the temp object
		//tempGO.AddComponent<AudioSource>(); // add an audio source
		tempGO.SetActive(false);
		List<GameObject> poolList = new List<GameObject>(poolArr);
		poolList.Add(tempGO);
		poolAmount++;
		poolArr = poolList.ToArray();
		return tempGO;
	}
}
