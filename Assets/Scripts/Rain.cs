using UnityEngine;

public class RainE : MonoBehaviour
{
    public GameObject rainPrefab;
    public int maxRainDrops = 100;
    public float rainIntensity = 0.5f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;

    private GameObject[] rainDrops;

    void Start()
    {
        rainDrops = new GameObject[maxRainDrops];

        for (int i = 0; i < maxRainDrops; i++)
        {
            rainDrops[i] = Instantiate(rainPrefab, transform);
            rainDrops[i].SetActive(false);

            new Task(() =>
            {

            });
        }
    }
}