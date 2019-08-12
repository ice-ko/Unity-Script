using UnityEngine;
using System.Collections;

public class TestNumAi : MonoBehaviour
{
    public GameObject Target;
    public int MaxNum;
	
	void Start ()
	{
	    StartCoroutine(GenerateAi());
	}

    private IEnumerator GenerateAi()
    {
        Target.SetActive(false);
        for (int i = 0; i < MaxNum; i++)
        {
            GameObject go = Instantiate(Target);
            go.name = i.ToString();
            go.SetActive(true);
            AddRandomWork(go);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public Material MatMiner;
    public Material MatLogger;
    public Material MatBlacksmith;
    public Material MatWoodCutter;
    private void AddRandomWork(GameObject go)
    {
        MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
        float r = Random.Range(0f, 1f);
        if (r < 0.2f)
        {
            go.AddComponent<MineOreAction>();
            go.AddComponent<DropOffOreAction>();
            mr.sharedMaterial = MatMiner;
        }
        else if (r < 0.4f)
        {
            go.AddComponent<ChopTreeAction>();
            go.AddComponent<DropOffLogsAction>();
            mr.sharedMaterial = MatLogger;
        }
        else if (r < 0.6f)
        {
            go.AddComponent<PickUpOreAction>();
            go.AddComponent<ForgeToolAction>();
            go.AddComponent<DropOffToolsAction>();
            mr.sharedMaterial = MatBlacksmith;
        }
        else if (r < 0.8f)
        {
            go.AddComponent<ChopFirewoodAction>();
            go.AddComponent<DropOffFirewoodAction>();
            mr.sharedMaterial = MatWoodCutter;
        }
        else
        {
            //nothing
        }
    }
}
