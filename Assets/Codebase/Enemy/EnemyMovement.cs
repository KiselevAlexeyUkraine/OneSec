using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;               // Скорость движения
    [SerializeField] private Transform groundCheck;          // Точка проверки земли (расположить у переднего края)
    [SerializeField] private LayerMask groundLayer;            // Слой земли (и препятствий, если требуется)

    private Rigidbody _rigidbody;
    private Vector3 _direction = Vector3.right;              // Начальное направление движения
    private bool _isGrounded;

    // Расстояния для проверки: вниз от groundCheck и вперед от врага
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
        Move();
    }

    /// <summary>
    /// Двигает врага в текущем направлении, сохраняя вертикальную скорость.
    /// </summary>
    private void Move()
    {
        Vector3 movement = new Vector3(_direction.x * speed * Time.deltaTime, _rigidbody.velocity.y, 0f);
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    /// <summary>
    /// Проверяет наличие земли под точкой groundCheck и препятствий впереди.
    /// Если впереди нет земли или есть препятствие – враг разворачивается.
    /// </summary>
    private void CheckGroundAndObstacles()
    {
        // Проверка наличия земли под groundCheck
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        // Проверка наличия препятствия впереди: луч от позиции немного выше центра врага
        Vector3 obstacleRayOrigin = transform.position;
        bool obstacleAhead = Physics.Raycast(obstacleRayOrigin, _direction, obstacleCheckDistance, groundLayer);

        // Отрисовка лучей для отладки
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);
        Debug.DrawRay(obstacleRayOrigin, _direction * obstacleCheckDistance, Color.blue);

        if (!_isGrounded || obstacleAhead)
        {
            Flip();
        }
    }

    /// <summary>
    /// Разворачивает врага, изменяя направление движения и поворачивая его.
    /// </summary>
    private void Flip()
    {
        _direction = -_direction;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // Луч вниз для проверки земли
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

        // Луч вперед для проверки препятствий
        Gizmos.color = Color.blue;
        Vector3 obstacleRayOrigin = transform.position;
        Gizmos.DrawLine(obstacleRayOrigin, obstacleRayOrigin + _direction * obstacleCheckDistance);
    }
}
