using UnityEngine;
using System;

using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string username;
    public string password;
}

[Serializable]
public class UserDatabase
{
    public List<UserData> users = new List<UserData>();
}

public class LoginManager : Singleton<LoginManager>
{
    public static string LoggedInUsername { get; private set; } // 存储登录用户名

    private const string USER_DATA_PATH = "UserData";
    private UserDatabase userDatabase;
    // 登录结果事件
    public event Action<bool, string> OnLoginResult;
    public event Action<bool, string> OnRegResult;

    // 模拟的服务器地址
    private string serverUrl = "https://your-game-server.com/api/login";

    private System.Collections.IEnumerator SendLoginRequest(string username, string password)
    {
        Debug.Log(System.Environment.GetEnvironmentVariable("Path"));
        // 模拟延迟
        yield return new WaitForSeconds(1f);

        // 检查用户是否存在
        bool loginSuccess = userDatabase.users.Exists(
            user => user.username == username && user.password == password
        );

        string message = loginSuccess ? "Login Successful!" : "Invalid username or password.";

        if (loginSuccess)
        {
            // 假设登录成功
            Debug.Log("登录成功");
            LoggedInUsername = username;
            // 跳转到主界面
            GameManager.Instance.LoadScene("MainScene");

        }
        else
        {
            // 通知 UI 登录失败
            Debug.LogWarning("用户名或密码错误");
        }

        // 通知 UI 结果
        OnLoginResult?.Invoke(loginSuccess, message);

    }

    private System.Collections.IEnumerator SendRegRequest(string username, string password, string password2)
    {
        // 模拟延迟
        yield return new WaitForSeconds(1f);

        // 检查用户是否存在
        bool loginSuccess = UserExists(username);

        string message = loginSuccess ? "Register Successful!" : "Invalid username or password.";

        if (loginSuccess)
        {
            // 假设注册成功
            Debug.Log("注册成功");
            AttemptLogin(username, password);

        }
        else
        {
            // 通知 UI 登录失败
            Debug.LogWarning("用户名或密码错误");
        }


        // 通知 UI 结果
        OnRegResult?.Invoke(loginSuccess, message);
    }

    public void LoadUserDatabase()
    {
        // 如果文件不存在，创建空的数据库
        TextAsset jsonFile = Resources.Load<TextAsset>(USER_DATA_PATH);
        if (jsonFile != null)
        {
            userDatabase = JsonUtility.FromJson<UserDatabase>(jsonFile.text);
            Debug.Log("User data loaded successfully!");
        }
        else
        {
            userDatabase = new UserDatabase();
            Debug.LogWarning("No user data found, starting with an empty list.");
        }

    }

    private void SaveUserDatabase()
    {
        string json = JsonUtility.ToJson(userDatabase, true);
        File.WriteAllText(Application.persistentDataPath + "/" + USER_DATA_PATH, json);
    }

    public void AttemptLogin(string name, string pwd)
    {
        // 模拟服务器通信
        StartCoroutine(SendLoginRequest(name, pwd));
    }

    public void AttemptRegister(string name, string pwd, string pwd2)
    {
        // 模拟服务器通信
        StartCoroutine(SendRegRequest(name, pwd, pwd2));
        // 验证输入
        if (string.IsNullOrEmpty(name) || name.Length < 3)
        {
            Debug.LogWarning("用户名太短");
            return;
        }

        if (pwd != pwd2)
        {
            Debug.LogWarning("两次密码输入不一致");
            return;
        }

        // 检查用户是否已存在
        if (UserExists(name))
        {
            Debug.LogWarning("用户名已存在");
            return;
        }

        // 添加新用户
        UserData newUser = new UserData
        {
            username = name,
            password = pwd
        };
        userDatabase.users.Add(newUser);

        // 保存数据库
        SaveUserDatabase();

        // 执行登录操作
        AttemptLogin(name, pwd);
    }

    private bool UserExists(string name)
    {
        foreach (UserData user in userDatabase.users)
        {
            if (user.username == name)
            {
                return true;
            }
        }
        return false;
    }
}
