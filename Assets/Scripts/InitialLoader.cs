using System.Collections;
using TMPro;
using UnityEngine;

public class InitialLoader : MonoBehaviour
{
    public GameObject activeWhenDone;
    public TMP_Text progressText;
    string currentAction;

    IEnumerator Start()
    {
        CameraController.Instance.enabled = false;

        currentAction = "Loading images...";
        yield return StartCoroutine(ActorManager.Instance.ILoadImages(SetProgress));

        Destroy(gameObject);
        activeWhenDone.SetActive(true);

        CameraController.Instance.enabled = true;
    }

    void SetProgress(int current, int total, string extra)
    {
        progressText.text = $"{currentAction}\n";
        progressText.text += $"{Mathf.RoundToInt((float)current / total * 100f)}%\n";
        progressText.text += $"{extra}";
    }
}
