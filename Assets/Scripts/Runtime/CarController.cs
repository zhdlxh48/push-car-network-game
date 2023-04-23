using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PushCar;
using PushCarLib;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    
    private AudioSource _audioSource;

    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier = 2f;
    
    private Vector2 startPosition;

    [SerializeField] private Transform car;
    [SerializeField] private Transform flag;

    private int networkFlag = 0;

    public event Action OnStopCar;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        inputReader.OnStartSwipe += OnStartSwipe;
        inputReader.OnEndSwipe += OnEndSwipe;
    }
    
    private void OnDisable()
    {
        inputReader.OnStartSwipe -= OnStartSwipe;
        inputReader.OnEndSwipe -= OnEndSwipe;
    }

    private void Update()
    {
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
                
                OnStopCar?.Invoke();
                // var dist = GetDistance();
                //
                // TestSendToNetwork("Reached: " + dist.ToString());
            }
        }
    }

    private void OnStartSwipe()
    {
        speed = 0f;
    }

    private void OnEndSwipe(float len)
    {
        speed = len * speedMultiplier;
        
        _audioSource.Play();
        networkFlag = 1;
    }

    private float GetDistance() => flag.position.x - car.position.x;

    private async void TestSendToNetwork(string str)
    {
        var clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        var time = 4f;
        var dist = 100;

        var score = new Score(time, dist);
        var sendBuf = score.CopyToBuffer(1024);
        
        EndPoint serverEp = new IPEndPoint(IPAddress.Loopback, 9948);

        try
        {
            await Task.Run((() => clientSock.SendTo(sendBuf, serverEp)));

            var recvBuf = new byte[1024];
            var rcvLen = clientSock.ReceiveFrom(recvBuf, ref serverEp);
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
