using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private Transform car;
    [SerializeField] private Transform flag;

    [SerializeField] private TextMeshProUGUI distanceText;

    private void Update()
    {
        var dist = Vector3.Distance(car.position, flag.position);

        if (!IsGameOver(dist))
        {
            distanceText.text = string.Format("Distance (Car-Flag): {0:0.00}", dist);
        }
        else
        {
            distanceText.text = "Game Over";
        }
    }

    public bool IsGameOver(float dist)
    {
        return dist < 0f;
    }
}
