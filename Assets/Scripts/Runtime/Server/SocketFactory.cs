using System.Net.Sockets;
using UnityEngine;

namespace PushCar.Runtime.Server
{
    public static class SocketFactory
    {
        public static Socket Create(ProtocolType type)
        {
            Socket socket = null;
            try
            {
                switch (type)
                {
                    case ProtocolType.Tcp:
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        break;
                    case ProtocolType.Udp:
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        break;
                    default:
                        socket = new Socket(SocketType.Unknown, ProtocolType.Unknown);
                        break;
                }
            }
            catch (SocketException ex)
            {
                Debug.LogErrorFormat("[Network Socket Error] Code: {0}, {1}", ex.ErrorCode, ex.Message);
                Debug.LogErrorFormat(ex.StackTrace);
                Application.Quit();
            }

            return socket;
        }
    }
}