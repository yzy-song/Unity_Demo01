using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    // 场景和背景音乐的映射
    public AudioClip defaultBGM;
    public AudioClip battleBGM;

    private float bgmVolume = 1.0f;    // BGM 音量
    private float sfxVolume = 1.0f;    // 音效音量

    private bool isBgmMuted = false;   // BGM 是否静音
    private bool isSfxMuted = false;  // 音效是否静音

    private float volume = 1.0f;

    // 停止 BGM
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!isSfxMuted)
        {
            Debug.Log("静音中...");
        }
        else
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayBGMWithFade(AudioClip clip, float fadeDuration = 1f)
    {
        StartCoroutine(FadeInBGM(clip, fadeDuration));
    }

    private System.Collections.IEnumerator FadeInBGM(AudioClip newClip, float fadeDuration)
    {
        if (bgmSource.isPlaying)
        {
            // 渐出
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                bgmSource.volume = Mathf.Lerp(1, 0, t / fadeDuration);
                yield return null;
            }
            bgmSource.Stop();
        }

        // 切换音乐
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 渐入
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }

    // 设置音量
    public void SetVolume(float value)
    {
        volume = Mathf.Clamp01(value); // 确保音量在 0 到 1 之间
        Debug.Log($"音量设置为: {volume}");

        // 更新所有音频源的音量
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.volume = volume;
        }
    }

    // 获取当前音量
    public float GetVolume()
    {
        return volume;
    }

    // 静音 BGM
    public void MuteBGM(bool mute)
    {
        isBgmMuted = mute;
        UpdateVolumes();

    }

    // 静音音效
    public void MuteSFX(bool mute)
    {
        isSfxMuted = mute;
    }

    // 更新音量
    private void UpdateVolumes()
    {
        if (bgmSource != null)
        {
            bgmSource.volume = isBgmMuted ? 0 : bgmVolume * volume;
        }
    }

}
