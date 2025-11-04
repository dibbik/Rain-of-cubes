using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    private const float DefaultSpawnInterval = 1f;
    private const int DefaultPoolSize = 20;

    [SerializeField] private FallingCube _cubePrefab;
    [SerializeField] private Transform _spawnArea;
    [SerializeField] private float _spawnInterval = DefaultSpawnInterval;
    [SerializeField] private int _initialPoolSize = DefaultPoolSize;

    private CubePool _cubePool;
    private Coroutine _spawningCoroutine;
    private WaitForSeconds _spawnWait;

    private void Awake()
    {
        ValidateDependencies();
        _cubePool = new CubePool(_cubePrefab, transform, _initialPoolSize);
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    private void OnEnable()
    {
        _spawningCoroutine = StartCoroutine(SpawningCoroutine());
    }

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private IEnumerator SpawningCoroutine()
    {
        while (enabled)
        {
            yield return _spawnWait;
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        FallingCube cube = _cubePool.GetCube();

        Vector3 spawnPosition = _spawnArea != null ?
            CalculateSpawnPosition() : transform.position;

        cube.transform.position = spawnPosition;
        cube.gameObject.SetActive(true);
        cube.CubeExpired += OnCubeExpired;
    }

    private Vector3 CalculateSpawnPosition()
    {
        const float HalfDivider = 2f;

        float randomX = Random.Range(-_spawnArea.localScale.x / HalfDivider, _spawnArea.localScale.x / HalfDivider);
        float randomY = Random.Range(0f, _spawnArea.localScale.y);
        float randomZ = Random.Range(-_spawnArea.localScale.z / HalfDivider, _spawnArea.localScale.z / HalfDivider);

        return _spawnArea.position + new Vector3(randomX, randomY, randomZ);
    }

    private void OnCubeExpired(FallingCube cube)
    {
        cube.CubeExpired -= OnCubeExpired;
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