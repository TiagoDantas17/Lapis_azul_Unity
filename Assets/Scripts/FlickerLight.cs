using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light luz;
    public float intensidadeMinima = 0.3f;
    public float intensidadeMaxima = 1.0f;
    public float velocidade = 6f;

    private void Start()
    {
        if (luz == null)
        {
            luz = GetComponent<Light>();
        }
    }

    private void Update()
    {
        if (luz == null) return;

        float ruido = Mathf.PerlinNoise(Time.time * velocidade, 0f);
        luz.intensity = Mathf.Lerp(intensidadeMinima, intensidadeMaxima, ruido);
    }
}