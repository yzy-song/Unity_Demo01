using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound; // 音效文件
    private Button button;
    private AudioSource audioSource;

    private void Awake()
    {
        button = GetComponent<Button>();
        audioSource = FindObjectOfType<AudioSource>();

        if (button != null && audioSource != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // 自动加载默认音效的方法
    public void LoadDefaultSound()
    {
        if (clickSound == null)
        {
            clickSound = Resources.Load<AudioClip>("Sound/SE/SE_Click_7");
            if (clickSound == null)
            {
                Debug.LogWarning("Default click sound not found in Resources/Sound/SE/SE_Click_7");
            }
        }
    }
}
