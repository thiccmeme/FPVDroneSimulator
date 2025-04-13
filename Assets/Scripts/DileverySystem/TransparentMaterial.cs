using UnityEngine;

public class TransparentMaterial : MonoBehaviour
{
    private float minAlpha = 0f;
    private float maxAlpha = .7f;
    private float minIntensity = .8f; 
    private float maxIntensity = 1.5f; 
    public float currentAlpha;
    public float currentIntensity; 
    private Color baseEmissionColor; 
    public float speed = .5f; 
    private bool fadingup; 
    private Material objectMaterial; 
    private void Start() 
    {
        minAlpha = 0f;
        currentAlpha = maxAlpha;
        objectMaterial = GetComponent<Renderer>().material; 
        baseEmissionColor = objectMaterial.GetColor("_EmissionColor"); 
        if (objectMaterial != null)
        { 
            currentAlpha = objectMaterial.color.a; 
        }
    }
    private void Update()
    { 
        if (fadingup)
        { 
           currentAlpha += speed * Time.deltaTime;
            if (currentAlpha >= maxAlpha)
            {
                currentAlpha = maxAlpha;
                fadingup = false; 
            }
        } 
        else 
        {
            currentAlpha -= speed * Time.deltaTime;
            if (currentAlpha <= minAlpha)
            { currentAlpha = minAlpha;
                fadingup = true;
            }
        } 
        currentAlpha = Mathf.Clamp(currentAlpha, minAlpha, maxAlpha);
        Color color = objectMaterial.color;
        color.a = currentAlpha;
        objectMaterial.color = color;
        float alphaT = (currentAlpha - minAlpha) / (maxAlpha - minAlpha);
        currentIntensity = Mathf.Lerp(maxIntensity, minIntensity, alphaT);
        Color finalColor = baseEmissionColor * currentIntensity; 
        if (objectMaterial.HasProperty("_EmissionColor"))
        {
            objectMaterial.SetColor("_EmissionColor", finalColor);
        
        }
    }
}
