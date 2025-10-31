using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private FallingCube _cubePrefab;
    [SerializeField] private Transform _spawnArea;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _initialPoolSize = 20;

    private CubePool _cubePool;
    private List<FallingCube> _activeCubes = new List<FallingCube>();
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        ValidateDependencies();
        _cubePool = new CubePool(_cubePrefab, transform, _initialPoolSize);
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
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        FallingCube cube = _cubePool.GetCube();
        cube.transform.position = _spawnArea != null ?
            _spawnArea.position + new Vector3(
                Random.Range(-_spawnArea.localScale.x / 2f, _spawnArea.localScale.x / 2f),
                Random.Range(0f, _spawnArea.localScale.y),
                Random.Range(-_spawnArea.localScale.z / 2f, _spawnArea.localScale.z / 2f)
            ) : transform.position;

        cube.gameObject.SetActive(true);
        cube.CubeExpired += OnCubeExpired;
        _activeCubes.Add(cube);
    }

    private void OnCubeExpired(FallingCube cube)
    {
        cube.CubeExpired -= OnCubeExpired;
        _activeCubes.Remove(cube);
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