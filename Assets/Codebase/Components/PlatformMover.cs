using Codebase.Player;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints; // ������ �����, �� ������� ����� ��������� ���������
    [SerializeField] private float _speed = 2f; // �������� �������� ���������
    [SerializeField] private bool _loop = true; // ����� �� ��������� ��������� ����������
    [SerializeField] private LayerMask _playerLayerMask; // ����, ������������, ��� ��������� �������

    private int _currentWaypointIndex = 0; // ������� ����� ����������
    private bool _isReversing = false; // ����������� �������� (��� ������������ ������)
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

    // ��������� ����� � �������, �������� �� ����
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ����������� �� ������ ��������� ���� � �������������� ������� �����
        if (((1 << other.gameObject.layer) & _playerLayerMask.value) != 0)
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.IsOnPlatform = true;
            }
            other.transform.SetParent(transform, true);
        }
    }

    // ��������� ������ �� ��������, �������� �� ����
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayerMask.value) != 0)
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.IsOnPlatform = false;
            }
            other.transform.SetParent(null, true);
        }
    }

    // ���������� �������� ��������� �������� (��������, ������)
    private void UpdateAttachedObjects(Vector3 movementDelta)
    {
        foreach (Transform child in transform)
        {
            if (((1 << child.gameObject.layer) & _playerLayerMask.value) != 0)
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
