using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;



// Code from this tutorial: https://www.youtube.com/watch?v=SZjpm950g_c
public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField nameInputField;
    [SerializeField] public TextMeshProUGUI gameInfoText;
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] GameObject menu;

    [SerializeField] Camera menuCam;

    [SerializeField] string level;

    async void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }


        menuCam.enabled = true;
        menuCam.GetComponent<AudioListener>().enabled = false;



        Debug.Log("[UIManager] Starting!");


    }

    async void Host()
    {
        // bool success = NetworkManager.Singleton.StartHost();
        // Debug.Log("[UIManager] StartHost test...");
        // Debug.Log("[UIManager] StartHost result: " + success);
        // NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoadComplete;

        // if (!success)
        // {
        //     Debug.LogError("StartHost FAILED — check NetworkManager setup!");
        //     return;
        // }
        //menu.SetActive(false);
        try
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(3);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            var useWebSockets = Application.platform == RuntimePlatform.WebGLPlayer;
            if (useWebSockets)
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;
            }
            else
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = false;
            }
            var relayServerData = new Unity.Networking.Transport.Relay.RelayServerData(
                allocation,
                useWebSockets ? "wss" : "udp"
            );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            menuCam.enabled = false;
            gameInfoText.gameObject.SetActive(true);
            gameInfoText.text = "Press Enter to Start";

            if (NetworkManager.Singleton.StartHost())
            {
                Debug.Log("[UIManager] Loading scene: " + level);
                NetworkManager.Singleton.SceneManager.LoadScene(level, LoadSceneMode.Single);
            }
            else
            {
                Debug.LogError("[UIManager] Failed to start host.");
            }


        }
        catch (RelayServiceException e)
        {
            Debug.LogError("[UIManager] Relay Host Exception: " + e);
        }


    }

    async void Join()
    {
        try
        {
            string joinCode = nameInputField.text.Trim();

            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("[UIManager] Join code is empty!");
                return;
            }

            var allocation = await RelayService.Instance.CreateAllocationAsync(3);

            var useWebSockets = Application.platform == RuntimePlatform.WebGLPlayer;
            if (useWebSockets)
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = true;
            }
            else
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().UseWebSockets = false;
            }
            var relayServerData = new Unity.Networking.Transport.Relay.RelayServerData(
                allocation,
                useWebSockets ? "wss" : "udp"
            );

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            //menu.SetActive(false);
            menuCam.enabled = false;
            gameInfoText.gameObject.SetActive(true);
            gameInfoText.text = "Waiting for Host...";
            NetworkManager.Singleton.SceneManager.LoadScene(level, LoadSceneMode.Single);
        }
        catch (RelayServiceException e)
        {
            Debug.Log("[UIManager] Loading scene: " + level);
            Debug.LogError("[UIManager] Relay Join Exception: " + e);
        }



    }

    private void OnSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode mode)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId) return;

        Debug.Log($"[UIManager] Scene '{sceneName}' loaded for client {clientId}");

        // This is where you can:
        // - Activate player controls
        // - Enable physics
        // - Spawn UI
        // - Play a sound, etc.
    }
}
