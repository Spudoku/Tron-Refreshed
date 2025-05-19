using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GenericHitmarker : NetworkBehaviour
{
    public ulong spawnerClientId;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StartCoroutine(DieLater());
        Debug.Log($"[GenericHitMarker] my owner is {spawnerClientId}");
    }

    private IEnumerator DieLater()
    {
        yield return new WaitForSeconds(1f);
        if (IsServer)
        {
            NetworkObject.Despawn();
        }
    }
}
