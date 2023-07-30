using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiatePhotonPrefabsManager : MonoBehaviourPunCallbacks
{
    [Header("Название игровой сцены:")]
    [SerializeField] private string _gameSceneName = "GameScene";

    [Space(25f)]
    [Header("Путь к префабу порождающего обьекта:")]
    [SerializeField] private string _pathToPhotonPrefab = "PhotonPrefabs/PlayerCharacterInstantiateManager";


    public static InstantiatePhotonPrefabsManager Instance { get; private set; }






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == _gameSceneName)
        {
            PhotonNetwork.Instantiate(_pathToPhotonPrefab, Vector2.zero, Quaternion.identity);
        }
    }
}
