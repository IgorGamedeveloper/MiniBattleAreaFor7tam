using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    public static SpawnPointsManager Instance { get; private set; }

    private SpawnPointItem[] _spawnPoints;





    private void OnEnable()
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

    private void FindSpawnPoint()
    {
        _spawnPoints = FindObjectsOfType<SpawnPointItem>(true);
    }

    public SpawnPointItem GetRandomSpawnPoint()
    {
        FindSpawnPoint();

        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            int index = Random.Range(0, _spawnPoints.Length);
            return _spawnPoints[index];
        }
        else
        {
            Debug.LogError("Ќе удалось обнаружить точки возрождени€!");
            return null;
        }

    }
}
