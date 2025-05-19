using Unity.Netcode;
using UnityEngine;

//Script to handle spawning network projectiles
public class FireProjectile : NetworkBehaviour
{
    public GameObject projectile;
    public Transform shootTransform;
    //void Update()
    //{
    //    if (!IsOwner) return;

    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        ShootServerRpc();
    //    }
    //}

    [Rpc(SendTo.Server)]
    public void ShootServerRpc()
    {
        //if (!IsOwner) return;
        if (projectile == null) return;



        GameObject projectileSpawn = Instantiate(projectile, shootTransform.position, 
                                                    shootTransform.rotation);
        projectileSpawn.GetComponent<Frisbee>().source = this;

    }
    /*
    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void DestroyProjectileRpc(GameObject projectile)
    {

        Destroy(gameObject);
    }*/



}
