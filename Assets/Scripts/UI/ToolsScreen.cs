using Octokit;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Application = UnityEngine.Application;

public class ToolsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text creditsText;
    [SerializeField] GameObject downloadPopupBox;
    [SerializeField] TMP_Text downloadPopupText;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip randomMusic;
    [SerializeField, Range(0, 1)] float randomMusicChance;

    private IEnumerator Start()
    {
        creditsText.text = string.Format(creditsText.text, Application.version);

        if (Random.value <= randomMusicChance)
        {
            musicSource.clip = randomMusic;
            musicSource.loop = false;
            musicSource.Play();
        }

        var github = new GitHubClient(new ProductHeaderValue("WonderEditor"));

        var t = Task.Run(async () => await github.Repository.Release.GetAll("CharlesGameDev", "WonderEditor"));
        yield return new WaitUntil(() => t.IsCompleted);

        Release release = t.Result[0];
        if (release.TagName != Application.version) {
            downloadPopupBox.SetActive(true);
            downloadPopupText.text = string.Format(downloadPopupText.text, Application.version, release.TagName);
        }
    }
}
