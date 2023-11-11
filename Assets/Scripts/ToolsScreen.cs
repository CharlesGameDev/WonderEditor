using TMPro;
using UnityEngine;

public class ToolsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text creditsText;
    [SerializeField] AudioSource soundSource;

    private void Start()
    {
        creditsText.text = string.Format(creditsText.text, Application.version);
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}
