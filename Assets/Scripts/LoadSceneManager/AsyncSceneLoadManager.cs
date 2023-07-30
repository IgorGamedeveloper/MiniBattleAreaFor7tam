using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMG.SceneManagement
{
    public class AsyncSceneLoadManager : MonoBehaviour
    {
        public static AsyncSceneLoadManager Instance { get; private set; }

        private IEnumerator StartLoadingSceneAsync;

        [Header("Название загружаемой сцены:")]
        [SerializeField] private string _gameplaySceneName = "GameScene";

        [Space(10f)]
        [Header("Название промежуточной загрузочной сцены:")]
        [SerializeField] private string _loadSceneName = "LoadScene";
        private bool _loadingStarted = false;

        [Space(25f)]
        [Header("Задержка перед переходом на сцену:")]
        [SerializeField] private float _startGameDelay = 2.0f;






        private void OnEnable()
        {
            _loadingStarted = false;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public void StartLoadSceneAsync()
        {
            if (_loadingStarted == false)
            {
                _loadingStarted = true;

                if (StartLoadingSceneAsync != null)
                {
                    StopCoroutine(StartLoadingSceneAsync);
                    StartLoadingSceneAsync = null;
                }

                StartLoadingSceneAsync = LoadSceneAsync(_gameplaySceneName);
                StartCoroutine(StartLoadingSceneAsync);
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_loadSceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = true;

            if (asyncOperation.isDone == false)
            {
                yield return null;
            }

            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            if (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(_startGameDelay);

            asyncOperation.allowSceneActivation = true;
        }
    }
}
