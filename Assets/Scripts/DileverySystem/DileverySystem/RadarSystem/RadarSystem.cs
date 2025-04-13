using UnityEngine;

public class RadarSystem : MonoBehaviour
{
    public RectTransform radarPannel;
    private float radarRange = 50f;

    public GameObject pickDotPrefab;
    public GameObject DropDotPrefab;
    private Transform PickUpTarget;
    private Transform DropTarget;

    private GameObject currentPickupDot;
    private GameObject currentDropDot;

    public DroneController _droneController;
    private Transform Player;

    private void Start()
    {
        
        Player = _droneController.transform;
    }

    private void Update()
    {
        ClearOrUpdateDot(PickUpTarget, ref currentPickupDot, pickDotPrefab);
        if (DropTarget != null)
        {
            ClearOrUpdateDot(DropTarget, ref currentDropDot, DropDotPrefab);
        }

    }


    private void ClearOrUpdateDot(Transform target, ref GameObject dotObj, GameObject prefab)// this function will take the refrence of the pick/drop zones and show them on Radar UI
    {
        if (target == null)
        {
            if (dotObj != null)
            {
                dotObj.SetActive(false);// if the there is no target gameobject is deactivated 
                return;
            }
        }
        Vector3 direction = target.position - Player.position;// direction to player
        Vector2 flatdirection = new Vector2(direction.x, direction.z);

        if (flatdirection.magnitude > radarRange)
        {

            if (dotObj != null)
                dotObj.SetActive(false);// if object is out of range deactivate the dot object 
            return;
        }
        float playerYaw = Player.eulerAngles.y;
        Quaternion inverseRotation = Quaternion.Euler(0, -playerYaw, 0);
        Vector3 rotatedDir3D = inverseRotation * new Vector3(flatdirection.x, 0, flatdirection.y);
        Vector2 rotatedDir = new Vector2(rotatedDir3D.x, rotatedDir3D.z);

        Vector2 normalizedPos = rotatedDir / radarRange;
        float panelRadius = radarPannel.rect.width / 2f;
        Vector2 uiPosition = normalizedPos * panelRadius;

        // Clamp to edge if needed
        if (uiPosition.magnitude > panelRadius)
            uiPosition = uiPosition.normalized * panelRadius;

        // Create dot if doesn't exist
        if (dotObj == null)
        {
            dotObj = Instantiate(prefab, radarPannel);
        }

        dotObj.SetActive(true);
        RectTransform dotRect = dotObj.GetComponent<RectTransform>();
        dotRect.anchoredPosition = uiPosition;
    }

    public void SetPickUpTarget(Transform target)
    {
        Debug.Log("SetPickUpTarget");
        PickUpTarget = target.transform;
        DropTarget = null;
    }

    public void SetDropTarget(Transform target)
    {
        Debug.Log("SetDropTarget");
        DropTarget = target.transform;
        PickUpTarget = null;
    }
}
