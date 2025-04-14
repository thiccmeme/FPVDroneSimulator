using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;


public class DroneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private bool rollEnabled;
    [SerializeField] private bool pitchEnabled;
    [SerializeField] private GameObject ui;
    private float height;
    private bool up;
    private bool uiToggled;
    private bool down;
    private Vector2 moveInputValue;
    private Vector2 rotateInputValue;
    private Camera mainCam;
    private SceneHandler sceneHandler;
    
    public float correctionDuration = 0.5f; // Time in seconds to correct the rotation
    private Vector3 targetEulerAngles; // Target rotation angles ignoring the Y-axis
    private float elapsedTime = 0.0f; // Timer to track elapsed time
    private bool correctingRotation = false; // Flag to start correction

    private GameObject currentPackage;
    private PackagePickUp heldpackage;

    private void Start()
    {
        mainCam = GetComponentInParent<Camera>();
        sceneHandler = FindFirstObjectByType<SceneHandler>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Package")) return;
        sceneHandler.Respawn();
    }

    public void ToggleUi(InputAction.CallbackContext _context)
    {
        uiToggled = uiToggled ? false : true;
        ui.SetActive(uiToggled); 
    }


    public void MoveDrone(InputAction.CallbackContext _context)
    {
        moveInputValue = _context.ReadValue<Vector2>();
    }

    public void RotateDrone(InputAction.CallbackContext _context)
    {
        rotateInputValue = _context.ReadValue<Vector2>();
    }

    public void Up(InputAction.CallbackContext _context)
    {

        height = _context.ReadValue<float>();
        up = height > 0.01;
    }

    public void Down(InputAction.CallbackContext _context)
    {
        height = _context.ReadValue<float>();
        down = height > 0.01;
    }

    public void DisableRoll(InputAction.CallbackContext _context)
    {
        rollEnabled = false;
        pitchEnabled = false;
        targetEulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        StartRotationCorrection();
        //rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
    }

    public void EnableRoll(InputAction.CallbackContext _context)
    {
        if(rollEnabled) return;
        rollEnabled = true;
        pitchEnabled = false;
        //rigidBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    public void EnablePitch(InputAction.CallbackContext _context)
    {
        rollEnabled = false;
        pitchEnabled = true;
        targetEulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        StartRotationCorrection();
    }

    private void Update()
    {
        
        Vector3 movement = new Vector3(moveInputValue.y, 0, -moveInputValue.x);
        //movement.Normalize();
        rigidBody.AddRelativeForce(movement * speed, ForceMode.Acceleration);

        //roll
        if (rollEnabled)
        {
            Vector3 rotation = new Vector3(0, 0, rotateInputValue.x  );
            rotation.Normalize();
            Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
        else if(!rollEnabled && !pitchEnabled)
            //yaw
        {
            Vector3 rotation = new Vector3(0, rotateInputValue.x, 0 );
            rotation.Normalize();
            Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
        else
        {
            Vector3 rotation = new Vector3(rotateInputValue.y, 0, 0 );
            rotation.Normalize();
            Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed);
            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }
        
        
        //up
        if (up && !down)
        {
            Vector3 upward = new Vector3(0, verticalSpeed, 0);
            rigidBody.AddRelativeForce(upward * height, ForceMode.Acceleration);
            //Debug.Log(height);
        }
        //down
        if (down && !up)
        {
            Vector3 downward = new Vector3(0, -verticalSpeed, 0);
            rigidBody.AddRelativeForce(downward * height, ForceMode.Acceleration);
            //Debug.Log(height);
        }
        if (correctingRotation)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / correctionDuration);

            // Smoothly interpolate the rotation ignoring the Y-axis
            Vector3 newEulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetEulerAngles, progress);
            transform.rotation = Quaternion.Euler(newEulerAngles.x, transform.rotation.eulerAngles.y, newEulerAngles.z);

            // Stop correction when the target is reached
            if (progress >= 0.99f)
            {
                correctingRotation = false;
            }
        }
    }
    
    public void StartRotationCorrection()
    {
        correctingRotation = true;
        elapsedTime = 0.0f;
    }

    private void FixedUpdate()
    {
        

    }

    private void LateUpdate()
    {
        rigidBody.linearVelocity -= decelerationSpeed * rigidBody.linearVelocity;
    }

    public void Drop(InputAction.CallbackContext context) 
    { 
        if (!context.performed) return;
        if(heldpackage!= null)
        {
            heldpackage.Drop();
            heldpackage = null;
        }
    
    }
    public void SetHeldPackage(PackagePickUp package)
    {
        heldpackage=package;
    }
}
