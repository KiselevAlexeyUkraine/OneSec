using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform raycastOrigin; // Точка выхода луча
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer; // Слой препятствий

    private Rigidbody _rigidbody;
    private Vector3 _direction = Vector3.right;
    private bool _isGrounded;
    private bool _isChasing = false;

    private float groundCheckDistance = 1f;
    private float obstacleCheckDistance = 1f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void Update()
    {
        CheckGroundAndObstacles();
        if (Vector3.Distance(transform.position, player.position) <= maxChaseDistance)
        {
            DetectPlayer();
        }
        else
        {
            _isChasing = false;
        }
        Move();
    }

    private void Move()
    {
        if (_isChasing && player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            _direction = new Vector3(Mathf.Sign(directionToPlayer.x), 0f, 0f);
        }

        // Разворот только по оси Y
        if (_direction.x > 0)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (_direction.x < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        Vector3 movement = new Vector3(_direction.x * speed * Time.deltaTime, _rigidbody.velocity.y, 0f);
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    private void CheckGroundAndObstacles()
    {
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        Vector3 obstacleRayOrigin = transform.position;
        bool obstacleAhead = Physics.Raycast(obstacleRayOrigin, _direction, obstacleCheckDistance, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
        Debug.DrawRay(obstacleRayOrigin, _direction * obstacleCheckDistance, Color.blue);

        if (!_isGrounded || obstacleAhead)
        {
            Flip();
        }
    }

    private void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 rayDirection = (player.position - raycastOrigin.position).normalized;
        float distanceToPlayer = Vector3.Distance(raycastOrigin.position, player.position);

        if (Physics.Raycast(raycastOrigin.position, rayDirection, out hit, detectionRange, playerLayer | obstacleLayer))
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0) // Если попали в игрока
            {
                _isChasing = true;
            }
            else
            {
                _isChasing = false;
            }
        }
        else
        {
            _isChasing = false;
        }

        Debug.DrawRay(raycastOrigin.position, rayDirection * Mathf.Min(distanceToPlayer, detectionRange), _isChasing ? Color.green : Color.yellow);
    }

    private void Flip()
    {
        if (!_isChasing)
        {
            _direction = -_direction;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        Gizmos.color = Color.blue;
        Vector3 obstacleRayOrigin = transform.position;
        Gizmos.DrawLine(obstacleRayOrigin, obstacleRayOrigin + _direction * obstacleCheckDistance);
    }
}
