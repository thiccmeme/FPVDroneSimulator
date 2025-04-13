using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("Scene");
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
