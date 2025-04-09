using UnityEngine;

public enum ZoneType { PickUp,Drop}
public class DileveryZone : MonoBehaviour
{
    public ZoneType zoneType;
    [SerializeField]private MaterialSo materialSo;
    public GameObject PackagePrefab;
    private bool packageAvailable= true;
    private Material currentMaterial;
    private MeshRenderer _renderer;
    public GameObject particles;
    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        currentMaterial = GetComponent<Material>();
        if(zoneType == ZoneType.PickUp)
        {
            currentMaterial = materialSo.pickUp;
            _renderer.material = currentMaterial;
        }
        else
        {
            currentMaterial = materialSo.destination;
            _renderer.material = currentMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            DroneController drone = other.GetComponent<DroneController>();
            if(drone != null && zoneType == ZoneType.PickUp)
            {
                drone.PickUpPackage(PackagePrefab);
                packageAvailable = false;
                Debug.Log("fuck");  
            }
        }
        else if (zoneType == ZoneType.Drop && other.CompareTag("Package"))
        {
            //drone.DropPackage(transform.position);
            var newParticles = Instantiate(particles, transform);
        }
    }
}
