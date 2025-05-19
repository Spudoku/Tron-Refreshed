
using Unity.Netcode;
using UnityEngine;

public class Frisbee : NetworkBehaviour
{

    public FireProjectile source;
    public int BouncesRemaining = 3;
    private Rigidbody rb;
    private NetworkObject networkObject;

    float startSpeed = 3.3f;


    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkObject = gameObject.GetComponent<NetworkObject>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * startSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider collision)
    {
        if (!IsOwner) return;
        if (IsServer)
        {
            NetworkObject.Despawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (BouncesRemaining < 1)
        {
            if (IsServer)
            {
                NetworkObject.Despawn();
            }
        }

        transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, 
                                                      collision.contacts[0].normal));

        rb.linearVelocity = transform.forward * rb.linearVelocity.magnitude;

    }

}

