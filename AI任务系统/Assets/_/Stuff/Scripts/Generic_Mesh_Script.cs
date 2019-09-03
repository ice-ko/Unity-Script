using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generic_Mesh_Script : MonoBehaviour {
	
	private static Dictionary<string,Generic_Mesh_Script> dictionary = new Dictionary<string,Generic_Mesh_Script>();

    private static Quaternion[] cachedQuaternionEulerArr;
    private static void CacheQuaternionEuler() {
        if (cachedQuaternionEulerArr != null) return;
        cachedQuaternionEulerArr = new Quaternion[360];
        for (int i=0; i<360; i++) {
            cachedQuaternionEulerArr[i] = Quaternion.Euler(0,0,i);
        }
    }
    private static Quaternion GetQuaternionEuler(float rot) {
        if (rot < 0) rot += 360;
        return cachedQuaternionEulerArr[Mathf.RoundToInt(rot)];
    }

	private Mesh mesh;
	private Vector3[] vertices;
	private Vector2[] uvs;
	private int[] triangles;
	private int index = 0;
    public Bounds bounds;
	public  int amtMax = 15000;
	private bool update = false;
	private bool updateUvs = false;
	public Vector2 UVTextureSize = new Vector2(1,1);
    public bool spawnNewMeshWhenFull = false;
	
	public UVVectors[] UVList;
	private Vector2[] presetUV;
	private float timer = .1f;
	//private float timerAmount = .1f;
    public float updateRate = .033f;
	private string particles;
    public bool destroy = true;
	
	void Awake() {
        CacheQuaternionEuler();
        if (destroy) return;
		//amtMax = 1000;
		Generic_Mesh_Script.dictionary[name] = this;
		
		//Each square has: 4 vertices, 4 uvs, 2*3 triangles
		//1000 squares have: 4000 vertices, 4000 uvs, 6000 triangles
		mesh = new Mesh();
		vertices = new Vector3[4*amtMax];
		uvs = new Vector2[4*amtMax];
		triangles = new int[6*amtMax];
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		//mesh.RecalculateNormals();
		
		//Calculate preset UV
		presetUV = new Vector2[4*UVList.Length];
		for (int i=0; i<UVList.Length; i++) {
			presetUV[i*4+0] = new Vector2(UVList[i].Vector_1.x/UVTextureSize.x,UVList[i].Vector_1.y/UVTextureSize.y);
			presetUV[i*4+1] = new Vector2(UVList[i].Vector_2.x/UVTextureSize.x,UVList[i].Vector_2.y/UVTextureSize.y);
			presetUV[i*4+2] = new Vector2(UVList[i].Vector_3.x/UVTextureSize.x,UVList[i].Vector_3.y/UVTextureSize.y);
			presetUV[i*4+3] = new Vector2(UVList[i].Vector_4.x/UVTextureSize.x,UVList[i].Vector_4.y/UVTextureSize.y);
		}
		/*
		 * 0,1
		 * 0,0
		 * 1,0
		 * 1,1
		*/
		//mesh.bounds = new Bounds(new Vector3(Gauntlet.width*25-25+Gauntlet.startPos.x*50,1,-Gauntlet.height*25+25+Gauntlet.startPos.y*50),new Vector3(50*Gauntlet.width,1,50*Gauntlet.height));
        mesh.bounds = bounds;
		
		GetComponent<MeshFilter>().mesh = mesh;
		
		//particles = Options.Particles;
		//timerAmount = .033f; // 30FPS
        updateRate = .033f;
        //timerAmount = .016f; // 60FPS
		//if (particles == "Medium") timerAmount = .066f;
		//if (particles == "Low") timerAmount = .1f;

		//timer = .1f+Random.Range(0,timerAmount);
        timer = .1f+Random.Range(0,updateRate);
	}
    void OnDestroy() {
        dictionary = new Dictionary<string,Generic_Mesh_Script>();
        vertices = null;
        uvs = null;
        mesh = null;
    }
	private void myAwake() {
		destroy = false;
		Awake();
	}
	private void addGeneric(Vector3 pos, float rot, int material, float offset, Vector3 baseDir, bool skewed) {
		if (index >= amtMax) return;
		update = true;
		//pos.z -= offset;
		
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;
		
        if (skewed) {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseDir.x,  baseDir.y);
			vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseDir.x, -baseDir.y);
			vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseDir.x, -baseDir.y);
			vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseDir;
		} else {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseDir;
			vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseDir;
			vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseDir;
			vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseDir;
		}
		
		//Relocate UVs
		uvs[vIndex0] = presetUV[material*4+0];
		uvs[vIndex1] = presetUV[material*4+1];
		uvs[vIndex2] = presetUV[material*4+2];
		uvs[vIndex3] = presetUV[material*4+3];
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
		
		//index++;
		index = (index+1)%amtMax;

        
		if (index == 0 && spawnNewMeshWhenFull) {
			//Create a new instance
			GameObject newInstance = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer), typeof(Generic_Mesh_Script));
			Transform trans = newInstance.transform;
			trans.parent = transform.parent;
			trans.localPosition = transform.localPosition;
			//newInstance.AddComponent<MeshFilter>();
			//newInstance.AddComponent<MeshRenderer>();
			newInstance.GetComponent<Renderer>().material = transform.GetComponent<MeshRenderer>().material;
			Generic_Mesh_Script meshScript = newInstance.GetComponent<Generic_Mesh_Script>();
            meshScript.bounds = bounds;
            meshScript.amtMax = amtMax;
            meshScript.UVTextureSize = UVTextureSize;
            meshScript.UVList = UVList;
            meshScript.spawnNewMeshWhenFull = spawnNewMeshWhenFull;

            dictionary[name] = meshScript;
            
            int number = 2;
            for (int i=0; i<999; i++) {
                if (transform.parent.Find(name+"_"+number) == null) {
                    break;
                }
                number++;
            }
			trans.name = name;
            name = name+"_"+number;

            meshScript.myAwake();
        }
	}
	public void updateGeneric(int index, Vector3 pos, float rot, int material, float offset, Vector3 baseDir, bool skewed) {
		update = true;
		//pos.z -= offset;
		
		//Relocate vertices
		int vIndex = index*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;
		
        if (skewed) {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseDir.x,baseDir.y);
			vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseDir.x,-baseDir.y);
			vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3(baseDir.x,-baseDir.y);
			vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseDir;
		} else {
			vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseDir;
			vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseDir;
			vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseDir;
			vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseDir;
		}
        
		
		//Relocate UVs
		uvs[vIndex0] = presetUV[material*4+0];
		uvs[vIndex1] = presetUV[material*4+1];
		uvs[vIndex2] = presetUV[material*4+2];
		uvs[vIndex3] = presetUV[material*4+3];
		
		//Create triangles
		int tIndex = index*6;
		
		triangles[tIndex+0] = vIndex0;
		triangles[tIndex+1] = vIndex3;
		triangles[tIndex+2] = vIndex1;
		
		triangles[tIndex+3] = vIndex1;
		triangles[tIndex+4] = vIndex3;
		triangles[tIndex+5] = vIndex2;
	}
	private void updateUV(int ind, int material) {
		int vIndex = ind*4;
		int vIndex0 = vIndex;
		int vIndex1 = vIndex+1;
		int vIndex2 = vIndex+2;
		int vIndex3 = vIndex+3;
		
		//Relocate UVs
		uvs[vIndex0] = presetUV[material*4+0];
		uvs[vIndex1] = presetUV[material*4+1];
		uvs[vIndex2] = presetUV[material*4+2];
		uvs[vIndex3] = presetUV[material*4+3];
		
		updateUvs = true;
	}
	public void myLateUpdate() {
		LateUpdate();
	}
	void LateUpdate() {
		timer -= Time.deltaTime;
		if (timer <= 0)
			timer = updateRate;
		else
			return;
		if (update) {
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uvs;
			update = false;
			/*if (particles != Options.Particles) {
				particles = Options.Particles;
				timerAmount = .033f;
				if (particles == "Medium") timerAmount = .066f;
				if (particles == "Low") timerAmount = .1f;	
			}*/
		    //mesh.RecalculateNormals();
			updateUvs = false;
			return;
		}
		if (updateUvs) {
			mesh.uv = uvs;
			updateUvs = false;
		}
	}
	public static Generic_Mesh_Script GetMeshScript(string meshName) {
		return dictionary[meshName];
	}
	public static int GetIndex(string meshName) {
		return dictionary[meshName].index;
	}
	public static void AddGeneric(string meshName, Vector3 pos, float rot, int material, float offset, Vector3 baseDir, bool skewed) {
		dictionary[meshName].addGeneric(pos, rot, material, offset, baseDir, skewed);
	}
	public static void UpdateGeneric(string meshName, int index, Vector3 pos, float rot, int material, float offset, Vector3 baseDir, bool skewed) {
		dictionary[meshName].updateGeneric(index, pos, rot, material, offset, baseDir, skewed);
	}
	public static void UpdateUV(string meshName, int ind, int material) {
		dictionary[meshName].updateUV(ind,material);
	}
}