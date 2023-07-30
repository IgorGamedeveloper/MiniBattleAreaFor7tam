using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerCharacterInstantiateManager : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;

    [SerializeField] private string _pathToPlayerCharacterPrefab = "PhotonPrefabs/Player";


    private float _setSpawnPointTimeDelay = 5f;






    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SetSpawnPointAndInstantiateCharacter();
    }

    private void SetSpawnPointAndInstantiateCharacter()
    {
        if (_photonView != null)
        {
            if (_photonView.IsMine == true)
            {
                SpawnPointItem spawnPoint = SpawnPointsManager.Instance.GetRandomSpawnPoint();

                if (spawnPoint != null)
                {
                    PhotonNetwork.Instantiate(_pathToPlayerCharacterPrefab, spawnPoint.transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Нет свободных точек возрождения!");

                    Invoke("SetSpawnPointAndInstantiateCharacter", _setSpawnPointTimeDelay);
                }
            }
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на компонент PhotoView!");
        }
    }
}
