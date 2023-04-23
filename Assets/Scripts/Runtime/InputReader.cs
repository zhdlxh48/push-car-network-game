using System;
using PushCar.Runtime.Attributes.ReadOnly;
using UnityEngine;

namespace PushCar
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Camera _inputReadCamera;

        [SerializeField, ReadOnly] private Vector2 startViewport = Vector2.zero;
        [SerializeField, ReadOnly] private Vector2 endViewport = Vector2.zero;

        [field: SerializeField, ReadOnly] public float SwipeLength { get; private set; } = 0f;
        
        public event Action OnStartSwipe;
        public event Action<float> OnEndSwipe;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startViewport = _inputReadCamera.ScreenToViewportPoint(Input.mousePosition);
                
                SwipeLength = 0f;
                OnStartSwipe?.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endViewport = _inputReadCamera.ScreenToViewportPoint(Input.mousePosition);

                var xSwipeDir = endViewport.x - startViewport.x;
                SwipeLength = xSwipeDir > 0f ? Vector3.Distance(startViewport, endViewport) : 0f;
                OnEndSwipe?.Invoke(SwipeLength);
            }
        }
    }
}