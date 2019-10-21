// This script is for determining Frames Per Second on mobile. This is only for diagnostics
// and shouldn't be used in production. Additionally, this is not as accurate as the 
// profile and should only be used for quick esitmations

using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text fpsText;

	float deltaTime;
	
	void Update ()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        SetFPS();
	}

	void SetFPS()
	{
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		fpsText.text = string.Format("FPS: {0:00.} ({1:00.0} ms)", fps, msec);
	}
}
