using Unity.Netcode;
using UnityEngine;

public class Raycast : NetworkBehaviour
{
    [SerializeField] GameObject hitMarker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ShootRay(Camera cam)
    {

        Vector3 point = new(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);

        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit);
        if (hitSomething)
        {
            SpawnHitMarkerServerRpc(hit.point, OwnerClientId);
        }
    }

    // Spawn a hitmarker
    [ServerRpc]
    private void SpawnHitMarkerServerRpc(Vector3 hit, ulong clientId)
    {
        Debug.Log($"[Raycast] my owner is {OwnerClientId}");
        Debug.Log("hit something!");
        GameObject hitspot = Instantiate(hitMarker, hit, Quaternion.identity);
        if (IsOwner)
        {
            hitspot.GetComponent<GenericHitmarker>().spawnerClientId = clientId;
        }

        hitspot.GetComponent<NetworkObject>().Spawn();
    }
}
