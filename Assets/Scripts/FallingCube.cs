using UnityEngine;

public class FallingCube : MonoBehaviour
{
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Rigidbody _rigidbody;

    private CubeState _currentState = CubeState.Falling;

    private float _lifeTimer;
    private float _totalLifetime;

    private CubeConfig _config;

    private bool _hasChangedColor = false;

    public CubeState CurrentState => _currentState;

    public void Initialize(CubeConfig config)
    {
        _config = config;
        _currentState = CubeState.Falling;
        _hasChangedColor = false;
        _lifeTimer = 0f;
        _totalLifetime = 0f;

        ApplyColor(_config.InitialColor);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (_currentState == CubeState.Touched)
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer >= _totalLifetime)
            {
                _currentState = CubeState.Expired;
            }
        }
    }

    private void ApplyColor(Color color)
    {
        if (_cubeRenderer != null)
        {
            _cubeRenderer.material.color = color;
        }
    }

    private Color GenerateRandomColor()
    {
        return new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_currentState != CubeState.Falling) 
            return;

        _currentState = CubeState.Touched;

        if (!_hasChangedColor)
        {
            Color randomColor = GenerateRandomColor();
            ApplyColor(randomColor);
            _hasChangedColor = true;
        }

        _totalLifetime = Random.Range(_config.MinLifetime, _config.MaxLifetime);
    }

    private void Reset()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }
}