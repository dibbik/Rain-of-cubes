using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private FallingCube _cubePrefab;
    [SerializeField] private CubeConfig _cubeConfig;
    [SerializeField] private Transform _spawnArea;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _initialPoolSize = 20;

    private CubePool _cubePool;
    private float _spawnTimer;
    private List<FallingCube> _activeCubes = new List<FallingCube>();

    private void Awake()
    {
        ValidateDependencies();
        _cubePool = new CubePool(_cubePrefab, transform, _initialPoolSize);
    }

    private void Update()
    {
        ProcessSpawning();
        ProcessActiveCubes();
    }

    private void ProcessSpawning()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            SpawnCube();
            _spawnTimer = 0f;
        }
    }

    private void ProcessActiveCubes()
    {
        for (int i = _activeCubes.Count - 1; i >= 0; i--)
        {
            FallingCube cube = _activeCubes[i];

            if (cube.CurrentState == CubeState.Expired)
            {
                ReturnCubeToPool(cube);
                _activeCubes.RemoveAt(i);
            }
        }
    }

    private void SpawnCube()
    {
        FallingCube cube = _cubePool.GetCube();
        cube.Initialize(_cubeConfig);

        cube.transform.localScale = Vector3.one;

        Vector3 spawnPosition = CalculateRandomSpawnPosition();
        cube.transform.position = spawnPosition;

        _activeCubes.Add(cube);
    }

    private Vector3 CalculateRandomSpawnPosition()
    {
        if (_spawnArea == null)
        {
            return transform.position;
        }

        Vector3 areaScale = _spawnArea.localScale;
        Vector3 areaPosition = _spawnArea.position;

        float randomX = Random.Range(-areaScale.x / 2f, areaScale.x / 2f);
        float randomY = Random.Range(0f, areaScale.y);
        float randomZ = Random.Range(-areaScale.z / 2f, areaScale.z / 2f);

        Vector3 localOffset = new Vector3(randomX, randomY, randomZ);
        Vector3 worldPosition = areaPosition + localOffset;

        return worldPosition;
    }

    private void ReturnCubeToPool(FallingCube cube)
    {
        _cubePool.ReturnCube(cube);
    }

    private void ValidateDependencies()
    {
        if (_cubePrefab == null)
        {
            Debug.LogError("Префаб куба не назначен в CubeSpawner", this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_spawnArea.position, _spawnArea.localScale);
        }
    }
}