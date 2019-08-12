using UnityEngine;
using System.Collections;

public class WalkaroundAction : GoapAction
{
    private GameObject _walkTarget;
	private bool isReached = false;

    public WalkaroundAction()
    {
		AddEffect ("walkaround", true);
	}
	
	
	public override void Reset ()
	{
        isReached = false;
	}
	
	public override bool IsDone ()
	{
        return isReached;
	}
	
	public override bool RequiresInRange ()
	{
		return true;
	}
	
	public override bool CheckProceduralPrecondition (GameObject agent,BlackBoard bb)
	{
	    if (_walkTarget == null)
        {
            _walkTarget = new GameObject("walkTarget");
            RandomTarget(agent);
	    }

	    target = _walkTarget;

        return true;
	}

    void RandomTarget(GameObject agent)
    {
        _walkTarget.transform.position = agent.transform.position;
        _walkTarget.transform.position += new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)); 
    }
	
	public override bool Perform(GameObject agent, BlackBoard bb)
	{
	    RandomTarget(agent);

        isReached = true;

		return true;
	}
	
}
