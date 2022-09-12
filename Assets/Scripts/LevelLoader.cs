using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class LevelLoader : MonoBehaviour
{
    public float LoadingTime = 1f;
    public Animator Animator;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        Animator.SetTrigger(AnimationParameter.Start);
        yield return new WaitForSeconds(LoadingTime);
        SceneManager.LoadScene(sceneName);
    }
}
