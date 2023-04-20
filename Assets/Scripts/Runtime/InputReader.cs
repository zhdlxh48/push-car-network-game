using PushCar.Attributes;
using UnityEngine;

namespace PushCar
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private Camera _inputReadCamera;

        [SerializeField, ReadOnly] private Vector2 startPosition = Vector2.zero;
        [SerializeField, ReadOnly] private Vector2 endPosition = Vector2.zero;

        [SerializeField, ReadOnly] private Vector2 startViewport = Vector2.zero;
        [SerializeField, ReadOnly] private Vector2 endViewport = Vector2.zero;

        [SerializeField, ReadOnly] private float swipeLength = 0f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
                startViewport = _inputReadCamera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
                endViewport = _inputReadCamera.ScreenToViewportPoint(Input.mousePosition);

                var swipeLen = endPosition.x - startPosition.x;
                swipeLength = Vector3.Distance(startViewport, endViewport) * ((endViewport.x - startViewport.x) > 0 ? 1f : -1f);
            }
        }
    }
}