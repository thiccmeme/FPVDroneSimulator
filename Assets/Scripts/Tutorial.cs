using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using Unity.VRTemplate;
using UnityEngine.SceneManagement;
using System;

public class Tutorial : MonoBehaviour
{
    enum TutorialState
    {
        Tutorial1,
        Tutorial2,
        Tutorial3,
        Tutorial4,
        Tutorial5,
        Tutorial6
    }
    private TutorialState tutorialState = TutorialState.Tutorial1; // Index to track the current tutorial step
    public GameObject drone; // Reference to the tutorial panel GameObject
    private Vector3 droneStartPosition; // Variable to store the starting position of the drone
    private Vector3 droneStartRotation; // Variable to store the starting rotation of the drone
    public GameObject droneXRView;
    public GameObject personXRView;
    public GameObject xrLocomotion;
    public GameObject droneInput1;
    public GameObject droneInput2;

    public GameObject coachingCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Store initial position and rotation of the drone
        droneStartPosition = drone.transform.position;
        droneStartRotation = drone.transform.rotation.eulerAngles;
        drone.transform.DOMove(new Vector3(1.5f, 1.5f, -4f), 2f)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        tutorialState++; // Increment the tutorial index

        switch(tutorialState)
        {
            case TutorialState.Tutorial1:
                break;
            case TutorialState.Tutorial2:
                break;
            case TutorialState.Tutorial3:
                drone.transform.DOKill(); // Stop any other animations on the drone

                Rigidbody rb = drone.GetComponent<Rigidbody>();
                rb.useGravity = true; // Enable gravity on the drone's Rigidbody component

                DroneController droneController = drone.GetComponent<DroneController>();
                droneController.enabled = true; // Enable the DroneController script on the drone

                PlayerInput playerInput = drone.GetComponent<PlayerInput>();
                playerInput.enabled = true; // Enable the PlayerInput component on the drone

                xrLocomotion.SetActive(false); // Activate the XR Locomotion component
                break;
            case TutorialState.Tutorial4:
            
                break;
            case TutorialState.Tutorial5:
                drone.SetActive(false); // Deactivate the drone GameObject
                droneInput1.SetActive(false); // Deactivate the drone input GameObject
                personXRView.SetActive(false); // Deactivate the XR view GameObject for the person

                droneXRView.SetActive(true); // Activate the XR view GameObject
                droneInput2.SetActive(true); // Activate the second drone input GameObject
                break;
            case TutorialState.Tutorial6:
                StepManager stepManager = coachingCard.GetComponent<StepManager>();
                if(stepManager)
                    stepManager.Next();

                droneXRView.SetActive(false); // Deactivate the XR view GameObject
                droneInput2.SetActive(false); // Deactivate the second drone input GameObject
                personXRView.SetActive(true); // Activate the XR view GameObject for the person

                drone.transform.position = droneStartPosition; // Reset the drone's position to the starting position
                drone.transform.rotation = Quaternion.Euler(droneStartRotation); // Reset the drone's rotation to the starting rotation
                drone.SetActive(true); // Activate the drone GameObject
                droneInput1.SetActive(true); // Activate the drone input GameObject

                // Animate the drone again
                drone.transform.DOMove(new Vector3(1.5f, 1.5f, -4f), 2f)
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                break;
            default:
                // Make sure the MainMenu is added in the scene list
                SceneManager.LoadSceneAsync("MainDelivery");
                break;
        }
    }

    public void BackPressed()
    {
        if(tutorialState == TutorialState.Tutorial5)
        {
            Next();
        }
    }
}
