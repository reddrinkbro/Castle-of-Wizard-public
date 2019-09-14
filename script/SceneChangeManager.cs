using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour {

    [SerializeField] WarpEffectController warpEffectController;

    // Use this for initialization
    void Start () {
        if (SceneManager.GetActiveScene().name == "WarpScene")
        {
            if(warpEffectController != null)
            {
                //AsyncLoadScene("gestureTest");
                AsyncLoadScene("TestScene");
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AsyncLoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return new WaitForSeconds(0.5f);
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        asyncOper.allowSceneActivation = false;
        //while (!asyncOper.isDone)
        //{
        //    yield return null;
        //    if (asyncOper.progress >= 0.9f)
        //    {
        //        yield return StartCoroutine(warpEffectController.AccelWarpEffect());
        //        asyncOper.allowSceneActivation = true;
        //    }
        //}

        while (asyncOper.progress < 0.9f)
        {
            Debug.Log("progress: " + asyncOper.progress);
            yield return null;
        }

        yield return StartCoroutine(warpEffectController.AccelWarpEffect());
        asyncOper.allowSceneActivation = true;
    }
}
