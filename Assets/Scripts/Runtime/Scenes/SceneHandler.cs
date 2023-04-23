using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PushCar
{
    public class SceneHandler : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        
        [SerializeField] private float delayScale = 0.1f;
        [SerializeField] private GameObject loadingScreen;

        private const float WaitTime = 1f;

        public void ChangeScene()
        {
            EnableLoading();
            
            ChangeScene(sceneName, DisableLoading);
        }

        private void EnableLoading()
        {
            loadingScreen.SetActive(true);
        }
        
        private void DisableLoading()
        {
            loadingScreen.SetActive(false);
        }
        
        public static async void ChangeScene(string sceneName, Action onEnd = null, float delayScale = 0.1f)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);

            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                await Task.Delay((int)(1000 * delayScale));
            }
            await Task.Delay((int)(1000 * WaitTime));

            onEnd?.Invoke();
            op.allowSceneActivation = true;
        }
    }
}
