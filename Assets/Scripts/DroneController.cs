using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;


public class DroneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float roll;
    [SerializeField] private float yaw;
    [SerializeField] private float pitch;
    
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private bool rollEnabled;
    private Vector2 moveInputValue;
    private Vector2 rotateInputValue;


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
        Vector3 upward = new Vector3(0, verticalSpeed, 0);
        _rigidbody.AddRelativeForce(upward, ForceMode.Acceleration);
        Debug.Log("up");
    }

    public void Down(InputAction.CallbackContext _context)
    {
        Vector3 downward = new Vector3(0, verticalSpeed, 0);
        _rigidbody.AddRelativeForce(-downward, ForceMode.Acceleration);
        Debug.Log("down");
    }

    public void DisableRoll(InputAction.CallbackContext _context)
    {
        rollEnabled = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
    }

    public void EnableRoll(InputAction.CallbackContext _context)
    {
        rollEnabled = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInputValue.y, 0, -moveInputValue.x);
        movement.Normalize();
        _rigidbody.AddRelativeForce(movement * speed, ForceMode.Acceleration);


        if (rollEnabled)
        {
            Vector3 rotation = new Vector3(0, 0, rotateInputValue.x  );
            rotation.Normalize();
            Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
        else
        {
            Vector3 rotation = new Vector3(0, rotateInputValue.x, 0 );
            rotation.Normalize();
            Quaternion deltaRotation = Quaternion.Euler(rotation * rotationSpeed);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
        
    }

    private void LateUpdate()
    {
        _rigidbody.linearVelocity -= decelerationSpeed*_rigidbody.linearVelocity;
    }
}
