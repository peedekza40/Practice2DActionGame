using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Constants;

public class LevelLoader : MonoBehaviour
{
    public Animator Animator;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        Animator.SetTrigger(AnimationParameter.Start);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Progress Value : " + progressValue);

            yield return null;
        }
    }
}
