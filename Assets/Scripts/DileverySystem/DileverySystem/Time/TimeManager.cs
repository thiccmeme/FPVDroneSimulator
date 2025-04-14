using System.Collections;
using TMPro;    
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float maxTime;
    private float currentTime;

    public TextMeshProUGUI timerText;
    private bool timerRunning = true;
    public GameObject LoseCanvas;
    private SceneHandler sceneHandler;

    private void Start()
    {
        sceneHandler = FindFirstObjectByType<SceneHandler>();
        LoseCanvas.SetActive(false);
        currentTime = maxTime;

    }
    private void Update()
    {
        if(!timerRunning)return;

        currentTime-= Time.deltaTime;
        currentTime = Mathf.Max(0f, currentTime);
        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            timerRunning = false;
            OnTimerEnd();
        }
    }
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    private void OnTimerEnd()
    {
        LoseCanvas.SetActive(true);
        StartCoroutine("Lose");
        Debug.Log("Time Up");
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(1.5f);
        sceneHandler.Respawn();
    }
}
