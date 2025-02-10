using Codebase.Player;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer; // Слой препятствий
    [SerializeField] private float verticalThreshold = 1.5f; // Максимальная разница по высоте для преследования
    [SerializeField] private float stopChaseThreshold = 0.01f; // Минимальная дистанция для остановки преследования

    private Transform player;
    private PlayerHealth _playerHealth; // Ссылка на здоровье игрока
    private LevelManager _levelManager; // Ссылка на менеджер уровня
    private EnemyMovement _movement;
    private bool _isChasing;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _levelManager = FindObjectOfType<LevelManager>();

        // Ищем игрока по тегу "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            _playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("Игрок с тегом 'Player' не найден! EnemyPatrol не сможет отслеживать игрока.");
        }
    }

    private void Update()
    {
        if (_playerHealth == null || _playerHealth.Health <= 0 || _levelManager == null || _levelManager.IsLevelCompleted)
        {
            _isChasing = false;
            return; // Если игрок мертв или уровень завершен, прекращаем преследование
        }

        DetectPlayer();
    }

    public void Patrol()
    {
        if (!_isChasing)
        {
            // Оставляем пустым, так как `EnemyMovement` сам выполняет движение
        }
    }

    public void SetChasing(bool chasing)
    {
        _isChasing = chasing;
    }

    private void DetectPlayer()
    {
        if (player == null || _playerHealth == null || _playerHealth.Health <= 0 || _levelManager == null || _levelManager.IsLevelCompleted) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);

        if (distanceToPlayer > maxChaseDistance || heightDifference > verticalThreshold || distanceToPlayer < stopChaseThreshold)
        {
            _isChasing = false;
            return;
        }

        Vector3 rayDirection = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange, playerLayer | groundLayer))
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                _isChasing = true;
                _movement.SetDirection(new Vector3(Mathf.Sign(rayDirection.x), 0f, 0f));
            }
            else if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
            {
                _isChasing = false;
            }
        }
        else
        {
            _isChasing = false;
        }

        Debug.DrawRay(transform.position, rayDirection * Mathf.Min(distanceToPlayer, detectionRange), _isChasing ? Color.green : Color.red);
    }
}
