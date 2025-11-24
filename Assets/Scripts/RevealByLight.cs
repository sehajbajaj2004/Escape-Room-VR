using UnityEngine;

public class RevealByLight : MonoBehaviour
{
    public Light flashlight; // assign your point light
    public float revealRadius = 2f;
    public float maxStrength = 1f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float distance = Vector3.Distance(flashlight.transform.position, transform.position);

        if (distance < revealRadius && flashlight.enabled)
        {
            float strength = 1f - (distance / revealRadius);
            mat.SetFloat("_RevealStrength", strength * maxStrength);
        }
        else
        {
            mat.SetFloat("_RevealStrength", 0f);
        }
    }
}
