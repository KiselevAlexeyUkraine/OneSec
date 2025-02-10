using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer; // —лой преп€тствий
    [SerializeField] private float verticalThreshold = 1.5f; // ћаксимальна€ разница по высоте дл€ преследовани€
    [SerializeField] private float stopChaseThreshold = 0.01f; // ћинимальна€ дистанци€ дл€ остановки преследовани€

    private Transform player;
    private EnemyMovement _movement;
    private bool _isChasing;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();

        // »щем игрока по тегу "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("»грок с тегом 'Player' не найден! EnemyPatrol не сможет отслеживать игрока.");
        }
    }

    private void Update()
    {
        DetectPlayer();
    }

    public void Patrol()
    {
        if (!_isChasing)
        {
            // ќставл€ем пустым, так как `EnemyMovement` сам выполн€ет движение
        }
    }

    public void SetChasing(bool chasing)
    {
        _isChasing = chasing;
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float heightDifference = Mathf.Abs(player.position.y - transform.position.y);

        // ≈сли игрок слишком далеко, слишком высоко/низко или очень близко Ч прекращаем преследование
        if (distanceToPlayer > maxChaseDistance || heightDifference > verticalThreshold || distanceToPlayer < stopChaseThreshold)
        {
            _isChasing = false;
            return;
        }

        Vector3 rayDirection = (player.position - transform.position).normalized;
        RaycastHit hit;

        // ¬ыпускаем луч в сторону игрока и провер€ем, нет ли преп€тствий (groundLayer)
        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange, playerLayer | groundLayer))
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0) // ≈сли попали в игрока
            {
                _isChasing = true;
                _movement.SetDirection(new Vector3(Mathf.Sign(rayDirection.x), 0f, 0f));
            }
            else if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0) // ≈сли на пути преп€тствие
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
