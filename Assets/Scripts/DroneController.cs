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
    private float height;
    private bool up;
    private bool down;
    private Vector2 moveInputValue;
    private Vector2 rotateInputValue;
    private Camera mainCam;
    private SceneHandler sceneHandler;

    private void Start()
    {
        mainCam = GetComponentInParent<Camera>();
        sceneHandler = FindFirstObjectByType<SceneHandler>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor")) return;
        sceneHandler.Respawn();
    }


    public void MoveDrone(InputAction.CallbackContext _context)
    {
        moveInputValue = _context.ReadValue<Vector2>();
    }

    public void RotateDrone(InputAction.CallbackContext _context)
    {
        rotateInputValue = _context.ReadValue<Vector2>();
    }
    
    //Logic for Roll here 
    //Roll should use the grip buttons to switch modes allowing the right stick to affect roll rather than yaw changing the constraints preventing rotation on the x and y axis
    //Alternativly Roll could use the left grip button to roll left and the right to roll right

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
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
    }

    public void EnableRoll(InputAction.CallbackContext _context)
    {
        rollEnabled = true;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
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
        else 
        //yaw
        {
            Vector3 rotation = new Vector3(0, rotateInputValue.x, 0 );
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

    }

    private void LateUpdate()
    {
        rigidBody.linearVelocity -= decelerationSpeed*rigidBody.linearVelocity;
    }
}
