using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance;
    public List<DeliveryTask> tasks;
    private int currentTaskindex = 0;
    public RadarSystem radar;   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideAllTask();

        ActivatecurrentPickup();
    }

    private void ActivatecurrentPickup()
    {
        if (currentTaskindex >= tasks.Count) return;

        tasks[currentTaskindex].pickupZone.SetActive(true);
        tasks[currentTaskindex].dropZone.SetActive(false);
        radar.SetPickUpTarget(tasks[currentTaskindex].pickupZone.transform);

    }

    public void onPickUpCollected()
    {
        var task = tasks[currentTaskindex];
        task.pickupZone.SetActive(false);
        task.dropZone.SetActive(true);
        radar.SetDropTarget(task.dropZone.transform);
    }

    public void onDropCollected()
    {
        tasks[currentTaskindex].dropZone.SetActive(false);
        tasks[currentTaskindex].isCompleted = true;
        radar.ClearDotTarget();
        currentTaskindex++;
        if (currentTaskindex < tasks.Count)
        {
            ActivatecurrentPickup();
        }
        else
        {
           
            string currentScene = SceneManager.GetActiveScene().name;

            if(currentScene== "Tutorial")
            {
                SceneManager.LoadScene("DeliveryMain");
            }
            else
            {
                Debug.Log("All deliveries complete!");
            }
        }
    }

    public void HideAllTask()
    {
        foreach (var task in tasks)
        {
            if (task.pickupZone != null)
                task.pickupZone.SetActive(false);

            if (task.dropZone != null)
                task.dropZone.SetActive(false);
        }
    }
}
