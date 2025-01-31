using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Скорость движения
    [SerializeField] private Transform groundCheck; // Точка проверки земли
    [SerializeField] private LayerMask groundLayer; // Слой земли

    private Rigidbody _rigidbody;
    private Vector3 _direction = Vector3.right; // Начальное направление движения
    private bool _isGrounded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = true; // Включаем гравитацию
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    private void Update()
    {
        CheckGround();
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_direction.x * speed * Time.deltaTime, _rigidbody.velocity.y, 0f);
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 1f, groundLayer);
        if (!_isGrounded)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1f);
        }
    }

    private void Flip()
    {
        _direction = -_direction;
        transform.Rotate(0f, 180f, 0f);
    }
}
