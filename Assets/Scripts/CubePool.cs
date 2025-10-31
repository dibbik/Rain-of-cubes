using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FallingCube))]
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
            CreateNewCube();
        }
    }

    public FallingCube GetCube()
    {
        return _availableCubes.Count > 0 ? _availableCubes.Dequeue() : CreateNewCube();
    }

    public void ReturnCube(FallingCube cube)
    {
        cube.gameObject.SetActive(false);
        _availableCubes.Enqueue(cube);
    }

    private FallingCube CreateNewCube()
    {
        FallingCube cube = Object.Instantiate(_cubePrefab, _parentTransform);
        cube.gameObject.SetActive(false);
        return cube;
    }
}