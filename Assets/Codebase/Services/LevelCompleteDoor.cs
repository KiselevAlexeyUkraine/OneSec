using UnityEngine;
using Codebase.Player; // ��������������, ��� PlayerCollector ��������� � ���� ������������ ���

public class LevelCompleteDoor : MonoBehaviour
{
    [Header("��������� ���������� ������")]
    [SerializeField] private LayerMask _playerLayerMask; // ����, ������������ ������
    [SerializeField] private Animator _doorAnimator; // �������� ����� (���� ������������ ��������)
    [SerializeField] private string _openTrigger = "Open"; // ��� �������� ��� �������� �������� �����
    private LevelManager _levelManager;
    [SerializeField] private AudioSource _doorAudio; // ����� ��� ����� �������� �����
    [SerializeField] private AudioClip _doorOpenClip; // ���� �������� �����

    private bool _isDoorOpened = false;

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
    }

    /// <summary>
    /// ��� ����� ������� � ������� ���������� ������ ���������, �������� �� �� �������.
    /// ���� � ������ ���� ���� �� ���� ���� � �� ���, ����� �����������.
    /// </summary>
    /// <param name="other">������ Collider</param>
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ����������� �� ������ ��������� ����
        if (((1 << other.gameObject.layer) & _playerLayerMask.value) != 0 && !_isDoorOpened)
        {
            // �������� �������� ���������� PlayerCollector � PlayerHealth � ������
            PlayerCollector collector = other.GetComponent<PlayerCollector>();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.Health <= 0)
            {
                Debug.Log("����� ����� � �� ����� ��������� �������!");
                return; // ���� ����� �����, ����� �� ������
            }

            if (collector != null)
            {
                if (collector.KeysCount > 0)
                {
                    // ���� � ������ ���� ����(�) - ��������� �����
                    OpenDoor();
                    _levelManager.EndLevelVictory();
                }
                else
                {
                    Debug.Log("� ������ ��� ������ ��� �������� �����!");
                }
            }
            else
            {
                Debug.LogWarning("�� ������� ������ �� ������ ��������� PlayerCollector!");
            }
        }
    }

    /// <summary>
    /// ��������� ����� (��������� �������� ��� ��������� ������ ������ ���������� ������).
    /// </summary>
    private void OpenDoor()
    {
        _isDoorOpened = true;
        if (_doorAnimator != null)
        {
            _doorAnimator.SetTrigger(_openTrigger);
        }
        else
        {
            Debug.Log("����� ������� (�������� �� ��������)!");
        }

        // ��������������� ����� �������� �����
        if (_doorAudio != null && _doorOpenClip != null)
        {
            _doorAudio.PlayOneShot(_doorOpenClip);
        }

        // �������������� ������ ���������� ������
        Debug.Log("������� ��������!");
    }
}
