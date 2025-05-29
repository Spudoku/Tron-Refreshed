using UnityEngine;

public class SpawnManager : MonoBehaviour
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

    private void TeleportTo(GameObject go, GameObject location)
    {
        if (location == null)
        {
            Debug.Log("[TeleportTo] teleport failed: no valid location points");
            return;
        }
        else if (go == null)
        {
            Debug.Log("[TeleportTo] teleport failed: invalid teleporting object");
        }
        go.transform.position = location.transform.position;
    }

    public void TeleportToRandomSpawnPoint(GameObject go)
    {
        int index = Random.Range(0, spawnPoints.Length);

        TeleportTo(go, spawnPoints[index]);
    }

    public void TeleportToJail(GameObject go)
    {
        int index = Random.Range(0, jailPoints.Length);

        TeleportTo(go, jailPoints[index]);
    }


}
