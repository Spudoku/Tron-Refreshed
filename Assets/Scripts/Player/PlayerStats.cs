using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Collider hitbox;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private PlayerName playerName;
    [SerializeField] NetworkVariable<float> maxHP;
    [SerializeField] NetworkVariable<float> curHP;
    [SerializeField] Camera cam;

    [SerializeField] private Raycast raycast;

    public override void OnNetworkSpawn()
    {
        // initialize everything?
        curHP = maxHP;
        Debug.Log($"[PlayerStats] my owner is {OwnerClientId}");
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

    private void Die()
    {

    }

    void Update()
    {
        // Firing raycasts
        if (!IsOwner) return;
        if (Input.GetButtonDown("Fire1"))
        {
            raycast.ShootRay(cam);
        }
    }

}
