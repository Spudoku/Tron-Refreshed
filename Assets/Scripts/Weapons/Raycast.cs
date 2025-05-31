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



        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"[ShootRay] Hit object: {hit.collider.gameObject.name}, Layer: {hit.collider.gameObject.layer}");

            SpawnHitMarkerServerRpc(hit.point, OwnerClientId);
            var playerStats = hit.collider.gameObject.GetComponentInParent<PlayerStats>();

            if (playerStats != null)
            {
                Debug.Log($"[ShootRay] hit player: {playerStats.gameObject.name}");
                playerStats.DieServerRpc();


            }
            else
            {
                Debug.Log($"[ShootRay] did not hit player");
            }

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
