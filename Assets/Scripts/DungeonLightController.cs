using System.Collections.Generic;
using UnityEngine;

public class DungeonLightController : MonoBehaviour
{
    [SerializeField] private Material darknessMaterial;
    [SerializeField] private int maxLights = 16;
    [SerializeField] private float lightRadius = 5f;

    private List<Transform> lights = new List<Transform>();

    private void Start()
    {
        if (darknessMaterial == null)
        {
            var sr = GetComponent<SpriteRenderer>();
            darknessMaterial = sr.sharedMaterial;
        }

        darknessMaterial.SetFloat("_LightRadius", lightRadius);
    }

    private void Update()
    {
        // Send all light positions
        darknessMaterial.SetInt("_LightCount", lights.Count);

        for (int i = 0; i < lights.Count; i++)
        {
            Vector4 pos = lights[i].position;
            darknessMaterial.SetVector("_LightPositions" + i, pos);
        }
    }

    public void RegisterLight(Transform t)
    {
        if (!lights.Contains(t))
            lights.Add(t);
    }

    public void UnregisterLight(Transform t)
    {
        if (lights.Contains(t))
            lights.Remove(t);
    }
}
