using Octokit;
using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Application = UnityEngine.Application;

public class ToolsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text creditsText;
    [SerializeField] AudioSource soundSource;
    [SerializeField] GameObject downloadPopupBox;
    [SerializeField] TMP_Text downloadPopupText;

    private IEnumerator Start()
    {
        creditsText.text = string.Format(creditsText.text, Application.version);

        var github = new GitHubClient(new ProductHeaderValue("WonderEditor"));

        var t = Task.Run(async () => await github.Repository.Release.GetAll("CharlesGameDev", "WonderEditor"));
        yield return new WaitUntil(() => t.IsCompleted);

        Release release = t.Result[0];
        if (release.Name != Application.version) {
            downloadPopupBox.SetActive(true);
            downloadPopupText.text = string.Format(downloadPopupText.text, Application.version, release.Name);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
