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
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);
        menuCam.enabled = true;
        menuCam.GetComponent<AudioListener>().enabled = false;
        Debug.Log("[UIManager] Starting!");

    }

    void Host()
    {
        NetworkManager.Singleton.StartHost();
        //menu.SetActive(false);
        menuCam.enabled = false;
        gameInfoText.gameObject.SetActive(true);
        gameInfoText.text = "Press Enter to Start";
        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name);
    }

    void Join()
    {
        NetworkManager.Singleton.StartClient();
        //menu.SetActive(false);
        menuCam.enabled = false;
        gameInfoText.gameObject.SetActive(true);
        gameInfoText.text = "Waiting for Host...";
        SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name);

    }
}
