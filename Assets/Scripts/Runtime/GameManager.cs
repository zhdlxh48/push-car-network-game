using System.Collections;
using System.Threading.Tasks;
using PushCar.Runtime.Server;
using PushCarLib;
using UnityEngine;

namespace PushCar.Runtime
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private RemoteCarController remoteCarController;
        [SerializeField] private LocalCarController localCarController;
        [SerializeField] private InputReader inputReader;
    
        [SerializeField] private ClientHandler clientHandler;
        
        [SerializeField] private ScoreUIHandler localScoreUIHandler;
        [SerializeField] private RankingUIHandler rankingUIHandler;
    
        [SerializeField] private GameObject readyText;
        [SerializeField] private GameObject startText;
        [SerializeField] private GameObject startButton;
        
        private Score _currentRivalScore;

        private void Awake()
        {
            readyText.SetActive(false);
            startText.SetActive(false);
            startButton.SetActive(true);
        }

        private void OnEnable()
        {
            inputReader.enabled = false;
            StartCoroutine(UpdateRanking());
        
            localCarController.OnStopCar += GameEnd;
        }

        private void OnDisable()
        {
            localCarController.OnStopCar -= GameEnd;
            StopAllCoroutines();
        }

        private IEnumerator UpdateRanking()
        {
            while (true)
            {
                var scores = clientHandler.ReceiveFromServer(4);
                rankingUIHandler.ShowRanking(scores);

                yield return new WaitForSeconds(60f);
            }
        }

        public async void GameStart()
        {
            _currentRivalScore = clientHandler.ReceiveRandomFromServer();
            
            startButton.SetActive(false);
        
            remoteCarController.Initialize();
            localCarController.Initialize();
            localScoreUIHandler.Initialize();
        
            // UI Start
        
            readyText.SetActive(true);
            startText.SetActive(false);

            await Task.Delay(1000);
            
            remoteCarController.SetMovement(_currentRivalScore.Time, _currentRivalScore.Distance);
        
            readyText.SetActive(false);
            startText.SetActive(true);
        
            await Task.Delay(1000);
        
            readyText.SetActive(false);
            startText.SetActive(false);
        
            // UI End
        
            inputReader.enabled = true;
        }

        public void GameEnd(float time, float dist)
        {
            inputReader.enabled = false;
            localScoreUIHandler.ShowScore(time, dist);

            if (dist > 0f)
            {
                Debug.Log($"Rival dist: {remoteCarController.GetRemainDistance()}, my dist: {dist} / you {(dist <= remoteCarController.GetRemainDistance() ? "win" : "lose")}");
                var isWinner = dist <= remoteCarController.GetRemainDistance();
                localScoreUIHandler.LetsDecideLooser(!isWinner);
                
                clientHandler.SendToServer(time, dist);
            }
            else
            {
                localScoreUIHandler.LetsDecideLooser(true);
            }

            startButton.SetActive(true);
        }
    }
}
