using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer; // ���� �����������
    [SerializeField] private float verticalThreshold = 1.5f; // ������������ ������� �� ������ ��� �������������
    [SerializeField] private float stopChaseThreshold = 0.01f; // ����������� ��������� ��� ��������� �������������

    private Transform player;
    private EnemyMovement _movement;
    private bool _isChasing;

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();

        // ���� ������ �� ���� "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("����� � ����� 'Player' �� ������! EnemyPatrol �� ������ ����������� ������.");
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
            // ��������� ������, ��� ��� `EnemyMovement` ��� ��������� ��������
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

        // ���� ����� ������� ������, ������� ������/����� ��� ����� ������ � ���������� �������������
        if (distanceToPlayer > maxChaseDistance || heightDifference > verticalThreshold || distanceToPlayer < stopChaseThreshold)
        {
            _isChasing = false;
            return;
        }

        Vector3 rayDirection = (player.position - transform.position).normalized;
        RaycastHit hit;

        // ��������� ��� � ������� ������ � ���������, ��� �� ����������� (groundLayer)
        if (Physics.Raycast(transform.position, rayDirection, out hit, detectionRange, playerLayer | groundLayer))
        {
            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0) // ���� ������ � ������
            {
                _isChasing = true;
                _movement.SetDirection(new Vector3(Mathf.Sign(rayDirection.x), 0f, 0f));
            }
            else if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0) // ���� �� ���� �����������
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
