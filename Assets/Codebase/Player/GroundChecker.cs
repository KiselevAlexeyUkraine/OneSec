using UnityEngine;

namespace Codebase.Player
{
    // ���� ����� ���������, ��������� �� ������ �� �����, ��������� �������� ����� ��������.
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Transform _groundCheck; // ���������, ����������� ����� �������� �� �����.
        [SerializeField] private LayerMask _groundMask; // ����, ������������, ��� ��������� ������.
        [SerializeField] private float _groundCheckRadius = 0.5f; // ������ �������� ��������������� � ������.

        private void Awake()
        {
            if (_groundCheck == null)
            {
                Debug.LogError("GroundCheck Transform is not assigned!");
            }
        }

        // �������� ���������� true, ���� ������ �������� ���� "�����".
        public bool IsGrounded => Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);

        // ������������� ������� �������� � ��������� Unity.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue; // ������������� ����� ���� ��� ������������.
            Gizmos.DrawSphere(_groundCheck.position, _groundCheckRadius); // ������ ����� ��������.
        }
    }
}