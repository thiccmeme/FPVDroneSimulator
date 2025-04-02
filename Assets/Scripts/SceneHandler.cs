using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    private DroneController droneController;
    [SerializeField]private float waitTime = 5.0f;
    [SerializeField] private string sceneName;
    private Camera mainCam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        droneController = FindFirstObjectByType<DroneController>();
        mainCam = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        mainCam.transform.SetParent(null);
        droneController.enabled = false;

        yield return new WaitForSeconds(waitTime);
        
        droneController.enabled = true;
        mainCam.transform.SetParent(droneController.transform);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
