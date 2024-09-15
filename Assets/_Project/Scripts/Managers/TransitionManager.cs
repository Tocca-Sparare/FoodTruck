using redd096;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] Animator anim;
    [SerializeField] float animationDuration = 1f;

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene));
    }

    IEnumerator LoadSceneCoroutine(string scene)
    {
        //play from 0
        anim.Play("Transition", -1, 0);

        yield return new WaitForSeconds(animationDuration);

        //change scene online or normally
        if (NetworkManager.IsOnline)
            SceneLoaderOnline.LoadScene(scene);
        else
            SceneManager.LoadScene(scene);
    }
}
