using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

// Code from this tutorial: https://www.youtube.com/watch?v=SZjpm950g_c
public class PlayerName : NetworkBehaviour
{

    [SerializeField] private TextMeshPro playerName;

    public NetworkVariable<FixedString32Bytes> networkPlayerName =
        new NetworkVariable<FixedString32Bytes>("<<UNKNOWN>>",
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public event Action<string> OnNameChanged;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            string inputName = "hiiiii";

            networkPlayerName.Value = new FixedString32Bytes(inputName);
        }

        playerName.text = networkPlayerName.Value.ToString();
        networkPlayerName.OnValueChanged += NetworkPlayerName_OnValueChanged;
        OnNameChanged?.Invoke(networkPlayerName.Value.ToString());
    }

    private void NetworkPlayerName_OnValueChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        playerName.text = newValue.Value;
        OnNameChanged?.Invoke(newValue.Value);
    }

    public string GetPlayerName()
    {
        return networkPlayerName.Value.ToString();
    }



}
