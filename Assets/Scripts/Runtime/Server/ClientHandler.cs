using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PushCarLib;
using UnityEngine;

namespace Runtime.Server
{
    public class ClientHandler : MonoBehaviour
    {
        private Socket _clientSocket;
        
        private DBConnector _dbConnector = new ();

        private void Awake()
        {
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            _dbConnector = new DBConnector();
        }
        
        // private async void SendToServer()

        private async void TestSendToNetwork(string str)
        {
            var time = 4f;
            var dist = 100f;

            var score = new Score(time, dist);
            var buf = score.CopyToBuffer(1024);
        
            EndPoint srvEP = new IPEndPoint(IPAddress.Loopback, 9948);

            try
            {
                await Task.Run((() => _clientSocket.SendTo(buf, srvEP)));

                var recvBuf = new byte[1024];
                var rcvLen = _clientSocket.ReceiveFrom(recvBuf, ref srvEP);
                var text = Encoding.Default.GetString(recvBuf, 0, rcvLen);

                Debug.Log(text);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                throw;
            }
        }
    }
}