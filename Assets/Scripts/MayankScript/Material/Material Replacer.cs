using UnityEngine;

public class MaterialReplacer : MonoBehaviour
{
    public Transform player;
    public MatSo MatSo;// scriptable object for the material made

    [SerializeField]
    private float targetDistance;

    private Renderer _renderer;
    private Light currentLight;


    public void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material = MatSo.DefaultMaterial;
        currentLight = GetComponentInChildren<Light>();
        currentLight.gameObject.SetActive(false);

    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < targetDistance)
        {
            currentLight.gameObject.SetActive(true);
            _renderer.material = MatSo.EmissiveMat;

        }
        else
        {
            // revert to original mat;
            _renderer.material = MatSo.DefaultMaterial;
            currentLight.gameObject.SetActive(false);
        }
    }
}
