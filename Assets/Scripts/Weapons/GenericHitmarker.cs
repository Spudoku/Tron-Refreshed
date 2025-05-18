using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GenericHitmarker : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StartCoroutine(DieLater());
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
