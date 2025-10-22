using UnityEngine;

public class FallingCubesSystem : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void Awake()
    {
        ValidateDependencies();
    }

    private void ValidateDependencies()
    {
        if (_cubeSpawner == null)
        {
            Debug.LogError("CubeSpawner не назначен в FallingCubesSystem", this);
        }
    }
}