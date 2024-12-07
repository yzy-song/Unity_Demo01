using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    public Button closeButton;
    public Slider volumeSlider;
    public Toggle toggle;
    public GameObject isOnObj;
    public GameObject isOffObj;
    public Toggle toggle2;
    public GameObject isOnObj2;
    public GameObject isOffObj2;
    private void Start()
    {
        // 注册设置界面到 UIManager
        UIManager.Instance.RegisterPanel("SettingsPanel", gameObject);

        if (volumeSlider != null)
        {
            // 初始化 Slider 的值为当前音量
            volumeSlider.value = AudioManager.Instance.GetVolume();

            // 监听 Slider 的值改变事件
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        else
        {
            Debug.LogError("Slider 未绑定！");
        }
        if (toggle2 != null)
        {
            toggle2.onValueChanged.AddListener((value) => OnMuted(value, "BgmToggle"));
        }

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((value) => OnMuted(value, "SfxToggle"));
        }

    }

    public void OnClose()
    {
        UIManager.Instance.HidePanel("SettingsPanel");
    }

    // 当 Slider 的值改变时调用
    public void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

    public void OnMuted(bool value, string toggleName)
    {
        Debug.Log($"Toggle: {toggleName} 被点击，值为: {value}");
        if (toggleName == "BgmToggle")
        {
            AudioManager.Instance.MuteBGM(!toggle2.isOn);
            isOnObj2.SetActive(toggle2.isOn);
            isOffObj2.SetActive(!toggle2.isOn);
        }
        else
        {
            AudioManager.Instance.MuteSFX(!toggle.isOn);
            isOnObj.SetActive(toggle.isOn);
            isOffObj.SetActive(!toggle.isOn);
        }
    }
}
