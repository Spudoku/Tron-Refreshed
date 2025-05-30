using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] jailPoints;
    public static SpawnManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        jailPoints = GameObject.FindGameObjectsWithTag("JailPoint");
        Debug.Log("[SpawnManager] initialized!");
    }



    public Vector3 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
    }

    public Vector3 GetRandomJailPoint()
    {
        return jailPoints[Random.Range(0, jailPoints.Length)].transform.position;
    }
}
