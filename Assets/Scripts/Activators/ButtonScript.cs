using System.Collections;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public enum ActivatorEffect { WorldReflection, MapReflection, BoxSpawner };
    [SerializeField] private ActivatorEffect activatorEffect;

    // PUBLIC
    public float pressSpeed;
    public bool reflectXAxis, reflectYAxis;
    public GameObject box;
    public Transform boxSpawnPoint;

    // PRIVATE
    private GameObject map, currentBox;
    private Vector3 _originalPos;
    private bool _isPressed, _canActivate = true, _isFlipped = false;
    [SerializeField] private float _maxDownDistance = 0.1f;

    void Start()
    {
        _originalPos = transform.localPosition;
        map = GameObject.Find("Map");
    }


    void FixedUpdate()
    {
        float direction = _isFlipped ? 1f : -1f; // Reverse movement when flipped
        float targetPosition = _isFlipped ? _originalPos.y + _maxDownDistance : _originalPos.y - _maxDownDistance; // Adjust target based on flip

        if (_isPressed && transform.localPosition.y > targetPosition)
        {
            transform.Translate(0f, direction * 0.01f, 0f, Space.Self);
        }
        else if (!_isPressed && transform.localPosition.y < _originalPos.y)
        {
            transform.Translate(0f, -direction * 0.01f, 0f, Space.Self);
        }
    }



    private IEnumerator ActivateEffect()
    {
        _canActivate = false;

        switch (activatorEffect)
        {
            case ActivatorEffect.WorldReflection:
                EventManager.instance.WorldReflection(reflectXAxis, reflectYAxis, map);
                if (reflectXAxis)
                {
                    _isFlipped = !_isFlipped;
                    _originalPos.y = -_originalPos.y;
                }
                break;
            case ActivatorEffect.MapReflection:
                EventManager.instance.MapReflection(reflectXAxis, reflectYAxis, map);
                if (reflectXAxis)
                {
                    _isFlipped = !_isFlipped;
                    _originalPos.y = -_originalPos.y;
                }
                break;
            case ActivatorEffect.BoxSpawner:
                if (currentBox != null)
                    Destroy(currentBox);
                currentBox = Instantiate(box, boxSpawnPoint.position, Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(2f);
        _canActivate = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPressed = true;
        }
        else if (other.CompareTag("Actuation Zone") && _canActivate)
        {
            StartCoroutine(ActivateEffect());
            _isFlipped = !_isFlipped;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPressed = false;
        }
    }
}