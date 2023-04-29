using PushCar.Runtime.Attributes.ReadOnly;
using PushCar.Runtime.Interfaces;
using UnityEngine;

namespace PushCar.Runtime
{
    public class RemoteCarController : MonoBehaviour, IInitializable
    {
        [SerializeField] private AnimationCurve curve;
        
        [SerializeField] private Transform target;
        private float _targetDist;
        
        [SerializeField, ReadOnly] private Vector3 targetPos;
        private Vector3 startPos;

        [SerializeField, ReadOnly] private float targetTime;
        private float _time;

        private AudioSource _audioSource;
        private bool _canMove;
        
        [SerializeField] private ScoreUIHandler remoteScoreUIHandler;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _canMove = false;
            
            _targetDist = target.position.x - transform.position.x;
            startPos = transform.position;
        }
    
        public void Initialize()
        {
            _canMove = false;
            _time = 0f;
            
            transform.position = startPos;
            
            remoteScoreUIHandler.Initialize();
        }

        private void Update()
        {
            if (_canMove)
            {
                var curveVal = curve.Evaluate(Mathf.Clamp01(_time / targetTime));
                transform.position = Vector3.Lerp(startPos, targetPos, curveVal);
            
                if (_time / targetTime >= 1f)
                {
                    _canMove = false;
                    _time = 0f;
                }
                else
                {
                    _time += Time.deltaTime;
                }
            }
        }

        private float _serverDistValue;
        public void SetMovement(float time, float dist)
        {
            targetPos = transform.position + Vector3.right * dist;
            targetTime = time;
            _serverDistValue = dist;
            
            _audioSource.Play();
            
            _canMove = true;
            
            remoteScoreUIHandler.ShowScore(time, GetRemainDistance());
        }
        
        public float GetRemainDistance()
        {
            return _targetDist - _serverDistValue;
        }
    }
}