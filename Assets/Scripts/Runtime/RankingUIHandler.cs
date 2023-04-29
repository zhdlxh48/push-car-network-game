using PushCarLib;
using TMPro;
using UnityEngine;

namespace PushCar.Runtime
{
    public class RankingUIHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankingText;

        public void ShowRanking(Score[] list)
        {
            rankingText.text = "";
            for (var i = 0; i < list.Length; i++)
            {
                rankingText.text += $"{i + 1}. {list[i].Distance:0.00}m / {list[i].Time:0.00}sec\n";
            }
        }
    }
}