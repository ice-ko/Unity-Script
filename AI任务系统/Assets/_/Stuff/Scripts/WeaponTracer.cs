using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class WeaponTracer {

    public static void Create(Vector3 fromPosition, Vector3 targetPosition) {
        Vector3 shootDir = (targetPosition - fromPosition).normalized;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        float shootAngle = UtilsClass.GetAngleFromVectorFloat(shootDir);
        Vector3 spawnTracerPosition = fromPosition + shootDir * distance * .5f;
        Material tracerMaterial = null;// new Material(GameAssets.i.m_WeaponTracer);
        tracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 256f));
        World_Mesh worldMesh = new World_Mesh(null, spawnTracerPosition, new Vector3(1, 1), shootAngle - 90, 6f, distance, tracerMaterial, null, 10000);

		int frame = 0;
        int frameBase = 0;

        worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame + 64 * frameBase, 0, 16, 256));
        float framerate = .016f;
		float timer = framerate;
		FunctionUpdater.Create(delegate {
			timer -= Time.deltaTime;
			if (timer < 0) {
				timer += framerate;
				frame++;
				if (frame >= 4) {
                    worldMesh.DestroySelf();
					return true;
				}
                worldMesh.AddPosition(shootDir * 2f);
                worldMesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame + 64 * frameBase, 0, 16, 256));
			}
			return false;
		});
    }

}
