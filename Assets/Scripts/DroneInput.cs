using System;
using UnityEngine;

public class DroneInput : MonoBehaviour
{
    private DroneController droneController;
    private DroneActions droneActions;

    private void Start()
    {
        droneController = FindFirstObjectByType<DroneController>();
        if (droneActions == null)
        {
            droneActions = new DroneActions();

            droneActions.DroneOnGround.Up.performed += droneController.Up;
            droneActions.DroneOnGround.Down.performed += droneController.Down;
        }
        
        droneActions.Enable();
    }

    public void DisableInput()
    {
        droneActions.Disable();
    }

    public void EnableInput()
    {
        droneActions.Enable();
    }
}
