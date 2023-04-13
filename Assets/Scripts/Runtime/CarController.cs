using System.Net;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private float speed;

    private Vector2 startPosition;

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

            speed = swipeLen / 500f;

            _audioSource.Play();
        }

        transform.Translate(speed, 0, 0);

        speed *= 0.98f;
    }
}
