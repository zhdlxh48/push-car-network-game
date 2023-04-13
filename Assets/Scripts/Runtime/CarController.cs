using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private float speed;
    
    private Vector2 startPosition;

    [SerializeField] private Transform car;
    [SerializeField] private Transform flag;

    private int networkFlag = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var endPos = Input.mousePosition;
            var swipeLen = endPos.x - startPosition.x;

            speed = swipeLen > 0f ? swipeLen / 500f : 0f;

            _audioSource.Play();
            networkFlag = 1;
        }

        transform.Translate(speed, 0, 0);

        if (speed > 0.01f)
        {
            speed *= 0.98f;
        }
        else
        {
            speed = 0f;

            if (networkFlag == 1)
            {
                networkFlag = 0;
                var dist = GetDistance();

                TestSendToNetwork("Reached: " + dist.ToString());
            }
        }
    }

    private float GetDistance() => flag.position.x - car.position.x;

    private void TestSendToNetwork(string str)
    {
        var clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var sendBuf = Encoding.Default.GetBytes(str);
        EndPoint srvEP = new IPEndPoint(IPAddress.Loopback, 10200);

        clientSock.SendTo(sendBuf, srvEP);

        var recvBuf = new byte[1024];
        var rcvLen = clientSock.ReceiveFrom(recvBuf, ref srvEP);
        var text = Encoding.Default.GetString(recvBuf, 0, rcvLen);

        Debug.Log(text);
    }
}
