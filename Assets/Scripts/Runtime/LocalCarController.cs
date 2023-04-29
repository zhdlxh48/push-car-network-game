using System;
using PushCar.Runtime.Attributes.ReadOnly;
using PushCar.Runtime.Interfaces;
using UnityEngine;

namespace PushCar.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class LocalCarController : MonoBehaviour, IInitializable
    {
        [SerializeField] private InputReader inputReader;
    
        [SerializeField, ReadOnly] private float speed = 0f;
        [SerializeField] private float speedMultiplier = 300f;
    
        [SerializeField] private Transform target;
        private Vector3 startPos;

        private float _time;

        private AudioSource _audioSource;
    
        private bool _isMoving = false;

        public event Action<float, float> OnStopCar;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            startPos = transform.position;
        }

        private void OnEnable()
        {
            inputReader.OnEndSwipe += SetSpeed;
        }
    
        private void OnDisable()
        {
            inputReader.OnEndSwipe -= SetSpeed;
        }
    
        public void Initialize()
        {
            transform.position = startPos;
        }

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);

            if (speed > 0.1f)
            {
                speed *= 0.99f;
            }
            else
            {
                speed = 0f;

                if (_isMoving)
                {
                    _isMoving = false;
                
                    _time = Time.time - _time;
                    var dist = GetDistance();
                
                    OnStopCar?.Invoke(_time, dist);
                }
            }
        }

        private void SetSpeed(float power)
        {
            speed = power * speedMultiplier;
        
            _time = Time.time;
        
            _audioSource.Play();
            _isMoving = true;
        }

        private float GetDistance() => target.position.x - transform.position.x;
    }
}
