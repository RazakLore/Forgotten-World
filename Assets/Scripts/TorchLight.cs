using UnityEngine;

public class TorchLight : MonoBehaviour
{
    DungeonLightController controller;

    private void Start()
    {
        controller = FindFirstObjectByType<DungeonLightController>();
        controller.RegisterLight(transform);
    }

    private void OnDestroy()
    {
        controller?.UnregisterLight(transform);
    }
}
