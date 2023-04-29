using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;
using PushCar.Runtime.Attributes.ReadOnly;
using PushCarLib;
using UnityEngine;

namespace PushCar.Runtime.Server
{
    public class ClientHandler : MonoBehaviour
    {
        [SerializeField, ReadOnly] private string serverIP = IPAddress.Loopback.ToString();
        [SerializeField, ReadOnly] private int serverPort = 9948;

        private const int BufferSize = 512;
        
        private Socket _clientSocket;

        public event Action OnConnectionError;

        private void Awake()
        {
            try
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (SocketException ex)
            {
                Debug.LogErrorFormat("[Network Socket Error] Code: {0}, {1}", ex.ErrorCode, ex.Message);
                Debug.LogErrorFormat(ex.StackTrace);
                Application.Quit();
            }
        }

        public void SendToServer(float time, float dist)
        {
            var buffer = new byte[BufferSize];
            
            using (var clientSocket = SocketFactory.Create(ProtocolType.Tcp))
            {
                if (TryConnect(clientSocket))
                {
                    var score = new Score(time, dist);
                    Debug.Log(score);
                    
                    JObject jObject = new JObject();
                    jObject.Add("flag", "game/result");
                    jObject.Add("score", JToken.Parse(score.Serialize()));
                    
                    SendJsonObject(clientSocket, jObject);
                    
                    var recvLen = clientSocket.Receive(buffer, BufferSize, SocketFlags.None);
                    if (recvLen == 0) return;
                    
                    var json = Encoding.UTF8.GetString(buffer, 0, recvLen);
                    var obj = JObject.Parse(json);
                    
                    var flag = obj.GetValue("flag")?.ToString();
                    var result = obj.GetValue("result")?.ToString();
                    
                    Debug.LogFormat("flag: {0}, result: {1}", flag, result);
                }

                _clientSocket.Close();
            }
        }

        public Score[] ReceiveFromServer(int count)
        {
            var buffer = new byte[BufferSize];

            using (var clientSocket = SocketFactory.Create(ProtocolType.Tcp))
            {
                if (TryConnect(clientSocket))
                {
                    JObject jObject = new JObject();
                    jObject.Add("flag", "game/req-scores");
                    jObject.Add("count", count);
                    
                    SendJsonObject(clientSocket, jObject);
                    
                    var recvLen = clientSocket.Receive(buffer, BufferSize, SocketFlags.None);
                    if (recvLen == 0) return null;
                    
                    var json = Encoding.UTF8.GetString(buffer, 0, recvLen);
                    var obj = JObject.Parse(json);
                    
                    var flag = obj.GetValue("flag")?.ToString();
                    var result = obj.GetValue("response")?.ToObject<Score[]>();
                    
                    Debug.LogFormat("flag: {0}, result: {1}", flag, result.Length);
                    
                    return result;
                }

                _clientSocket.Close();

                return null;
            }
        }

        public Score ReceiveRandomFromServer()
        {
            var buffer = new byte[BufferSize];

            using (var clientSocket = SocketFactory.Create(ProtocolType.Tcp))
            {
                if (TryConnect(clientSocket))
                {
                    JObject jObject = new JObject();
                    jObject.Add("flag", "game/req-random");
                    
                    SendJsonObject(clientSocket, jObject);
                    
                    var recvLen = clientSocket.Receive(buffer, BufferSize, SocketFlags.None);
                    if (recvLen == 0) return null;
                    
                    var json = Encoding.UTF8.GetString(buffer, 0, recvLen);
                    var obj = JObject.Parse(json);
                    
                    var flag = obj.GetValue("flag")?.ToString();
                    var result = obj.GetValue("response")?.ToObject<Score>();
                    
                    Debug.LogFormat("flag: {0}, result: {1}", flag, result);
                    
                    return result;
                }

                _clientSocket.Close();

                return null;
            }
        }

        private bool TryConnect(Socket socket)
        {
            try
            {
                socket.Connect(serverIP, serverPort);
                return true;
            }
            catch (SocketException ex)
            {
                Debug.LogErrorFormat("[Network Socket Error] Code: {0}, {1}", ex.ErrorCode, ex.Message);
                Debug.LogErrorFormat(ex.StackTrace);
                OnConnectionError?.Invoke();
            }
            
            return false;
        }

        private void SendJsonObject(Socket socket, JObject obj)
        {
            var buffer = Encoding.UTF8.GetBytes(obj.ToString());
            int len = buffer.Length > BufferSize ? BufferSize : buffer.Length;
            socket.Send(buffer, 0, len, SocketFlags.None);
        }
    }
}