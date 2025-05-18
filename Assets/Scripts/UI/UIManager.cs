using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Code from this tutorial: https://www.youtube.com/watch?v=SZjpm950g_c
public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField nameInputField;
    [SerializeField] public TextMeshProUGUI gameInfoText;
    [SerializeField] Button hostButton;
    [SerializeField] Button joinButton;
    [SerializeField] GameObject menu;

    [SerializeField] Camera menuCam;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);
        menuCam.enabled = true;
        menuCam.GetComponent<AudioListener>().enabled = false;
        Debug.Log("[UIManager] Starting!");

    }

    void Host()
    {
        bool success = NetworkManager.Singleton.StartHost();
        Debug.Log("[UIManager] StartHost test...");
        Debug.Log("[UIManager] StartHost result: " + success);

        if (!success)
        {
            Debug.LogError("StartHost FAILED — check NetworkManager setup!");
            return;
        }
        //menu.SetActive(false);
        menuCam.enabled = false;
        gameInfoText.gameObject.SetActive(true);
        gameInfoText.text = "Press Enter to Start";
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    void Join()
    {
        bool success = NetworkManager.Singleton.StartClient();
        Debug.Log("[UIManager] StartClient result: " + success);

        if (!success)
        {
            Debug.LogError("StartClient FAILED — check NetworkManager setup!");
            return;
        }

        //menu.SetActive(false);
        menuCam.enabled = false;
        gameInfoText.gameObject.SetActive(true);
        gameInfoText.text = "Waiting for Host...";
        NetworkManager.Singleton.SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

    }
}
