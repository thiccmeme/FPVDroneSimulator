using UnityEngine;

public class PackagePickUp : MonoBehaviour
{


    private bool pickedUp = false;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (pickedUp) return;
        if (other.gameObject.CompareTag("Drone"))
        {
            Transform dronepos = other.transform;
            transform.SetParent(dronepos);
            transform.localPosition = new Vector3(0, -1f, 0); // adjust under drone
            _rb.isKinematic = true;

            pickedUp = true;
            DeliveryManager.Instance.onPickUpCollected();
            DroneController Drone = other.transform.GetComponent<DroneController>();
            //PlayerController.instance.SetHeldPackage(this);
            Drone.SetHeldPackage(this);
        }
    }

    public void Drop()
    {
        transform.SetParent(null);
        _rb.isKinematic = false;
    }
}
