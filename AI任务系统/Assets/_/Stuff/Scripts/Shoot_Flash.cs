using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shoot_Flash {
	
	private static List<Shoot_Flash> shootList = new List<Shoot_Flash>();
	private static float deltaTime;
	
	public static void ResetStatic() {
		shootList = new List<Shoot_Flash>();
	}
	
	private float timer = .05f;
	private int index;
	private static Vector3 baseSize = new Vector3(17, 17);
	
	public Shoot_Flash(Vector3 pos) {
		index = Generic_Mesh_Script.GetIndex("Mesh_Top");
		Generic_Mesh_Script.AddGeneric("Mesh_Top", pos, 0f, 0, 0f, baseSize, false);
	}
	
	private void Update() {
		timer -= deltaTime;
		if (timer < 0) {
			//Generic_Mesh_Script.RemoveGeneric("Mesh_Top", index);
            shootList.Remove(this);
		}
	}
    
	public static void AddFlash(Vector3 pos) {
		Shoot_Flash sh = new Shoot_Flash(pos);
		shootList.Add(sh);
	}
	public static void Update_Static() {
		deltaTime = Time.deltaTime;

        List<Shoot_Flash> tmpShootList = new List<Shoot_Flash>(shootList);
        for (int i = 0; i < tmpShootList.Count; i++) {
            tmpShootList[i].Update();
        }
	}
}