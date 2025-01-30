using Codebase.Player;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // Массив точек, по которым будет двигаться платформа
    [SerializeField] private float _speed = 2f; // Скорость движения платформы
    [SerializeField] private bool _loop = true; // Будет ли платформа двигаться циклически

    private int _currentWaypointIndex = 0; // Текущая точка назначения
    private bool _isReversing = false; // Направление движения (только для нецикличного режима)
    private Vector3 _previousPosition;

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (_waypoints.Length == 0) return;

        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        Transform targetWaypoint = _waypoints[_currentWaypointIndex];
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);
        Vector3 movementDelta = newPosition - _previousPosition;

        transform.position = newPosition;
        _previousPosition = transform.position;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            GetNextWaypoint();
        }

        UpdateAttachedObjects(movementDelta);
    }

    private void GetNextWaypoint()
    {
        if (_loop)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
        }
        else
        {
            if (!_isReversing)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Length)
                {
                    _currentWaypointIndex = _waypoints.Length - 2;
                    _isReversing = true;
                }
            }
            else
            {
                _currentWaypointIndex--;
                if (_currentWaypointIndex < 0)
                {
                    _currentWaypointIndex = 1;
                    _isReversing = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().IsOnPlatform = true;
            other.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().IsOnPlatform = false;
            other.transform.SetParent(null, true);
        }
    }

    private void UpdateAttachedObjects(Vector3 movementDelta)
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                CharacterController controller = child.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.Move(movementDelta);
                }
            }
        }
    }
}
