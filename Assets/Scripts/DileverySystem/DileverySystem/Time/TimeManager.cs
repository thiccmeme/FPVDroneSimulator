using TMPro;    
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    private float maxTime;
    private float currentTime;

    public TextMeshProUGUI timerText;
    private bool timerRunning = true;

    private void Start()
    {
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
        Debug.Log("Time Up");
    }
}
