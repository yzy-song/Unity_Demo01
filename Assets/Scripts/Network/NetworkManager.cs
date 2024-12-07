using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net.Sockets;
using System.Text;
public class NetworkManager : Singleton<NetworkManager>
{

    private TcpClient _client;
    private NetworkStream _stream;

    public void Connect(string ip, int port)
    {
        _client = new TcpClient(ip, port);
        _stream = _client.GetStream();
    }

    public new void SendMessage(string message)
    {
        if (_stream != null && _stream.CanWrite)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }
    }

    public string ReceiveMessage()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = _stream.Read(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }
    public void Login(string username, string password)
    {
        StartCoroutine(LoginRequest(username, password));
    }

    private IEnumerator LoginRequest(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://yourserver.com/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Login Failed: " + www.error);
            }
            else
            {
                Debug.Log("Login Successful: " + www.downloadHandler.text);
            }
        }
    }
}
