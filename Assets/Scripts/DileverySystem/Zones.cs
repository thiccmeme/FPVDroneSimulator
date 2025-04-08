using UnityEngine;

public enum ZoneType { PickUp,Drop}
public class DileveryZone : MonoBehaviour
{
    public ZoneType zoneType;
    private MaterialSo materialSo;
    public GameObject PackagePrefab;
    private bool packageAvailable= true;
    private Material currentMaterial;
    private void Start()
    {
        currentMaterial = GetComponent<Material>();
        if(zoneType == ZoneType.PickUp)
        {
            currentMaterial = materialSo.pickUp;
        }
        else
        {
            currentMaterial = materialSo.destination;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Drone")) return;
        DroneController drone = other.GetComponent<DroneController>();
        if(drone != null)
        {
            drone.PickUpPackage(PackagePrefab);
            packageAvailable = false;
        }
        else
        {
            drone.DropPackage(transform.position);
        }
    }
}
