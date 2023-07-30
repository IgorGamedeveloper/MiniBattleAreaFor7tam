using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

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

    public SpawnPointItem GetFreeSpawnPoint()
    {
        FindSpawnPoint();

        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            int index = Random.Range(0, _spawnPoints.Length);

            for (int i = index; i < _spawnPoints.Length; i++)
            {
                if (_spawnPoints[i].IsBusy == false)
                {
                    return _spawnPoints[i];
                }
                else
                {
                    continue;
                }
            }
        }
        else
        {
            Debug.LogError("Ќе удалось обнаружить точки возрождени€!");
            return null;
        }

        Debug.LogError("Ќе удалось обнаружить свободные точки возрождени€!");
        return null;
    }
}
