using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class FallingCube : MonoBehaviour
{
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Rigidbody _rigidbody;

    private const float MinLifeTime = 2f;
    private const float MaxLifeTime = 5f;

    private bool _hasChangedColor = false;
    private Coroutine _lifeTimeCoroutine;

    public event System.Action<FallingCube> CubeExpired;

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        Cleanup();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Platform") || _hasChangedColor) return;
        ProcessTouch();
    }

    private void Initialize()
    {
        _hasChangedColor = false;
        _cubeRenderer.material.color = Color.white;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void Cleanup()
    {
        if (_lifeTimeCoroutine != null)
        {
            StopCoroutine(_lifeTimeCoroutine);
            _lifeTimeCoroutine = null;
        }
    }

    private void ProcessTouch()
    {
        _hasChangedColor = true;
        _cubeRenderer.material.color = Random.ColorHSV();

        if (_lifeTimeCoroutine != null)
            StopCoroutine(_lifeTimeCoroutine);

        _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        float lifeTime = Random.Range(MinLifeTime, MaxLifeTime);
        yield return new WaitForSeconds(lifeTime);
        CubeExpired?.Invoke(this);
    }
}