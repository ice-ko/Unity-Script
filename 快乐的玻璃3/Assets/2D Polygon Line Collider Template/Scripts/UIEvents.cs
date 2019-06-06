using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 2D Polygon Line Collider Package
 *
 * @license		    Unity Asset Store EULA https://unity3d.com/legal/as_terms
 * @author		    Indie Studio - Baraa Nasser
 * @Website		    https://indiestd.com
 * @Asset Store     https://www.assetstore.unity3d.com/en/#!/publisher/9268
 * @Unity Connect   https://connect.unity.com/u/5822191d090915001dbaf653/column
 * @email		    info@indiestd.com
 *
 */

[DisallowMultipleComponent]
public class UIEvents : MonoBehaviour {

    public void SimulateBall(Rigidbody2D ball)
    {
        if (ball == null) return;
        ball.isKinematic = false;
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadExample1()
    {
        SceneManager.LoadScene("Example1");
    }

    public void LoadExample2()
    {
        SceneManager.LoadScene("Example2");
    }
}
