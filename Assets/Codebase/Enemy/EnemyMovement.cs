using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody _rigidbody;
    private Vector3 _direction = Vector3.right;
    private bool _isGrounded;

    private float groundCheckDistance = 1f;
    private float obstacleCheckDistance = 1f;

    private EnemyPatrol _patrol;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        _patrol = GetComponent<EnemyPatrol>();
    }

    private void FixedUpdate()
    {
        CheckGroundAndObstacles();
        _patrol.Patrol(); // Выполняем патрулирование
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_direction.x * speed * Time.deltaTime, _rigidbody.velocity.y, 0f);
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    private void CheckGroundAndObstacles()
    {
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
        bool obstacleAhead = Physics.Raycast(transform.position, _direction, obstacleCheckDistance, groundLayer);

        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
        Debug.DrawRay(transform.position, _direction * obstacleCheckDistance, Color.blue);

        if (!_isGrounded || obstacleAhead)
        {
            Flip();
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        _direction = newDirection;
        transform.rotation = newDirection.x > 0 ? Quaternion.Euler(0f, 0f, 0f) : Quaternion.Euler(0f, 180f, 0f);
    }

    private void Flip()
    {
        SetDirection(-_direction);
    }
}
