using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private static float respawnTime = 5f;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Collider hitbox;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private PlayerName playerName;
    [SerializeField] NetworkVariable<float> maxHP;
    [SerializeField] NetworkVariable<float> curHP;
    [SerializeField] Camera cam;

    public bool isDead = false;

    [SerializeField] private Raycast raycast;
    //[SerializeField] private FireProjectile fireProjectile;

    public override void OnNetworkSpawn()
    {
        // initialize everything?
        curHP = maxHP;
        Debug.Log($"[PlayerStats] my owner is {OwnerClientId}");

        // teleport to spawn point
        StartCoroutine(DelayedInitSpawn());

        isDead = false;
    }

    private IEnumerator DelayedInitSpawn()
    {
        // wait until SpawnManager is ready
        while (SpawnManager.Instance == null || SpawnManager.Instance.GetRandomSpawnPoint() == Vector3.zero)
        {
            yield return null;
        }
        TeleportToSpawnPoint();
    }

    public void ChangeHP(float amount)
    {
        curHP.Value += amount;
        if (curHP.Value <= 0f)
        {
            Die();
        }
        else if (curHP.Value > maxHP.Value)
        {
            curHP.Value = maxHP.Value;
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        StartCoroutine(RespawnSequence());
        isDead = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DieServerRpc()
    {
        Die();
    }

    private IEnumerator RespawnSequence()
    {
        yield return null; // Wait one frame to ensure transform state is ready
                           //SpawnManager.Instance.TeleportToJail(gameObject);
        var position = SpawnManager.Instance.GetRandomJailPoint();

        TeleportToJail();
        yield return new WaitForSeconds(respawnTime);
        TeleportToSpawnPoint();
        //SpawnManager.Instance.TeleportToRandomSpawnPoint(gameObject);
    }

    void Update()
    {
        // Firing raycasts
        if (!IsOwner || isDead) return;

        if (Input.GetButtonDown("Fire1"))
        {
            raycast.ShootRay(cam);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //fireProjectile.ShootServerRpc();
        }

        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     DieServerRpc();
        // }
    }

    [ClientRpc]
    public void TeleportClientRpc(Vector3 position, ClientRpcParams rpcParams)
    {
        if (!IsOwner) return;
        NetworkObject netObj = GetComponent<NetworkObject>();
        NetworkTransform netTrans = GetComponent<NetworkTransform>();
        if (netObj != null)
        {
            netTrans.Teleport(position, Quaternion.identity, transform.localScale);
        }


    }

    // private void TeleportToJail()
    // {
    //     var position = SpawnManager.Instance.GetRandomJailPoint();
    //     var netTransform = GetComponent<NetworkTransform>();
    //     if (netTransform != null)
    //     {
    //         netTransform.Teleport(position, Quaternion.identity, transform.localScale);
    //     }
    // }

    // private void TeleportToSpawnPoint()
    // {
    //     var position = SpawnManager.Instance.GetRandomSpawnPoint();
    //     var netTransform = GetComponent<NetworkTransform>();
    //     if (netTransform != null)
    //     {
    //         netTransform.Teleport(position, Quaternion.identity, transform.localScale);
    //     }
    // }


    private void TeleportToJail()
    {
        var position = SpawnManager.Instance.GetRandomJailPoint();
        // var netTransform = GetComponent<NetworkTransform>();
        // if (netTransform != null)
        // {
        //     netTransform.Teleport(position, Quaternion.identity, transform.localScale);
        // }
        var rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { OwnerClientId }
            }
        };

        TeleportClientRpc(position, rpcParams);
    }

    private void TeleportToSpawnPoint()
    {
        var position = SpawnManager.Instance.GetRandomSpawnPoint();

        var rpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new[] { OwnerClientId }
            }
        };

        TeleportClientRpc(position, rpcParams);
    }


}
