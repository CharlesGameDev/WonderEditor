using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    [SerializeField] Animator wipeAnimator;
    [SerializeField] bool playWipeOutOnLoad = true;
    AudioSource soundSource;

    private void Awake()
    {
        if (playWipeOutOnLoad) wipeAnimator.Play("WipeOut");
        soundSource = gameObject.AddComponent<AudioSource>();
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

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
