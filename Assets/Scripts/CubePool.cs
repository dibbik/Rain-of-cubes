using System.Collections.Generic;
using UnityEngine;

public class CubePool
{
    private Queue<FallingCube> _availableCubes = new Queue<FallingCube>();
    private FallingCube _cubePrefab;
    private Transform _parentTransform;

    public CubePool(FallingCube cubePrefab, Transform parent, int initialSize)
    {
        _cubePrefab = cubePrefab;
        _parentTransform = parent;

        for (int i = 0; i < initialSize; i++)
        {
            FallingCube cube = Object.Instantiate(_cubePrefab, _parentTransform);
            cube.gameObject.SetActive(false);
            _availableCubes.Enqueue(cube);
        }
    }

    public FallingCube GetCube()
    {
        if (_availableCubes.Count == 0)
        {
            FallingCube cube = Object.Instantiate(_cubePrefab, _parentTransform);
            cube.gameObject.SetActive(false);
            _availableCubes.Enqueue(cube);
        }

        FallingCube availableCube = _availableCubes.Dequeue();
        availableCube.gameObject.SetActive(true);
        return availableCube;
    }

    public void ReturnCube(FallingCube cube)
    {
        cube.gameObject.SetActive(false);
        _availableCubes.Enqueue(cube);
    }
}