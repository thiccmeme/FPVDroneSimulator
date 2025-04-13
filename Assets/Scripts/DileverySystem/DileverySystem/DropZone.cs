using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Package")
        {
            DeliveryManager.Instance.onDropCollected();
            Destroy(other.gameObject);
        }
    }
}
