using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField passwordConfirmInput;
    public Button loginButton;       // 登录按钮
    public Button regButton;       // 注册按钮
    public Text messageText;         // 提示文字

    private bool isRegistering = false;
    private void Start()
    {
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            UIManager.Instance.SetCanvas(canvas.transform);
        }
        passwordConfirmInput.gameObject.SetActive(isRegistering);
        // 绑定按钮点击事件
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        regButton.onClick.AddListener(OnRegButtonClicked);

        // 订阅登录/注册结果事件
        LoginManager.Instance.OnLoginResult += HandleLoginResult;
        LoginManager.Instance.OnRegResult += HandleRegResult;

        LoginManager.Instance.LoadUserDatabase();
        usernameInput.text = "yzy290";
        passwordInput.text = "123456";
    }

    private void OnDestroy()
    {
        // 取消订阅事件
        LoginManager.Instance.OnLoginResult -= HandleLoginResult;
        LoginManager.Instance.OnRegResult -= HandleRegResult;
    }

    private void OnLoginButtonClicked()
    {
        // 获取输入框内容
        string username = usernameInput.text;
        string password = passwordInput.text;
        Debug.Log(username);
        // 调用 LoginManager 的登录方法
        LoginManager.Instance.AttemptLogin(username, password);
    }

    private void OnRegButtonClicked()
    {
        if (isRegistering)
        {
            // 获取输入框内容
            string username = usernameInput.text;
            string password = passwordInput.text;
            string password2 = passwordConfirmInput.text;

            // 调用 LoginManager 的注册方法
            LoginManager.Instance.AttemptRegister(username, password, password2);
        }
        else
        {
            isRegistering = true;
            passwordConfirmInput.gameObject.SetActive(isRegistering);
        }

    }

    private void HandleLoginResult(bool isSuccess, string message)
    {
        // 更新提示文字
        messageText.text = message;

        // 如果登录成功，可能在这里显示一个加载动画（可选）
        if (isSuccess)
        {
            messageText.color = Color.green;
        }
        else
        {
            messageText.color = Color.red;
        }
    }

    private void HandleRegResult(bool isSuccess, string message)
    {
        // 更新提示文字
        messageText.text = message;

        // 如果登录成功，可能在这里显示一个加载动画（可选）
        if (isSuccess)
        {
            messageText.color = Color.green;
        }
        else
        {
            messageText.color = Color.red;
        }
    }
}
