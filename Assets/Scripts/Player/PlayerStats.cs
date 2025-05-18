using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private Collider hitbox;

    [SerializeField] private PlayerName playerName;
    [SerializeField] NetworkVariable<float> maxHP;
    [SerializeField] NetworkVariable<float> curHP;

    public override void OnNetworkSpawn()
    {
        // initialize everything?
        curHP = maxHP;
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
}
