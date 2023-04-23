using System;
using Runtime.Server;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform car;
    [SerializeField] private Transform flag;
    
    [SerializeField] private CarController carController;
    [SerializeField] private ClientHandler clientHandler;

    [SerializeField] private TextMeshProUGUI distanceText;

    private void OnEnable()
    {
        carController.OnStopCar += OnGameEnd;
    }

    private void OnDisable()
    {
        carController.OnStopCar -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        
    }

    private void Update()
    {
        var dist = GetDistance();

        if (!IsGameOver(dist))
        {
            distanceText.text = string.Format("Distance (Car-Flag): {0:0.00}", dist);
        }
        else
        {
            distanceText.text = "Game Over";
        }
    }

    private float GetDistance() => flag.position.x - car.position.x;

    public bool IsGameOver(float dist)
    {
        return dist < 0f;
    }
}
