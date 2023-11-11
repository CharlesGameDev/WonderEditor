using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    [SerializeField] Animator wipeAnimator;
    [SerializeField] bool playWipeOutOnLoad = true;

    private void Awake()
    {
        if (playWipeOutOnLoad) wipeAnimator.Play("WipeOut");
    }

    public void LoadScene(string sceneName) => StartCoroutine(ILoadScene(sceneName));

    IEnumerator ILoadScene(string sceneName)
    {
        wipeAnimator.Play("WipeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void Close()
    {
        Application.Quit();
    }
}
