using PushCar.Runtime.Interfaces;
using TMPro;
using UnityEngine;

namespace PushCar.Runtime
{
    public class ScoreUIHandler : MonoBehaviour, IInitializable
    {
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private TextMeshProUGUI timeText;
        
        [SerializeField] private GameObject winText;
        [SerializeField] private GameObject loseText;

        public void Initialize()
        {
            distanceText.text = "0 m";
            timeText.text = "0 sec";
            
            loseText.SetActive(false);
            winText.SetActive(false);
        }

        public void ShowScore(float time, float dist)
        {
            if (dist > 0f)
            {
                distanceText.text = $"{dist:0.00} m";
                timeText.text = $"{time:0.00} sec";
            }
            else
            {
                distanceText.text = "Game Over";
            }
        }

        public void LetsDecideLooser(bool areYouLooser)
        {
            if (areYouLooser)
            {
                loseText.SetActive(true);
                winText.SetActive(false);
            }
            else
            {
                loseText.SetActive(false);
                winText.SetActive(true);
            }
        }
    }
}