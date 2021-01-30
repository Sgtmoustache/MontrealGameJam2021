using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    
    public void LoadSoloGame()
    {
        StartCoroutine(LoadSceneTransition());
    }

    public IEnumerator LoadSceneTransition()
    {
        yield return FadeManager._Instance.FadeOutRoutine();
        SceneManager.LoadScene(sceneToLoad);
    }
}
