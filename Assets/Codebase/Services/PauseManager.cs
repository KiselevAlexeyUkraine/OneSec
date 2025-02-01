using Codebase.Player;
using Codebase.Services.Inputs;
using Player;
using Codebase.Components.Ui.Pages;
using UnityEngine;

namespace Services
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField]
        private PageSwitcher _pageSwitcher;

        private CursorToggle _cursorToggle = new();

        public bool IsPaused { get; private set; }

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerCombat playerCombat;
        [SerializeField]
        private DesktopInput _desktopInput;

        private void Awake()
        {
            Play();
        }

        private void Update()
        {
            if (_desktopInput.Escape && playerMovement.IsDie == false)
            {
                SwitchState();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _cursorToggle.Enable();
            playerMovement.enabled = false;
            playerCombat.enabled = false;
        }

        public void Play()
        {
            Time.timeScale = 1f;
            _cursorToggle.Disable();
            playerMovement.enabled = true;
            playerCombat.enabled = true;
        }

        public void SwitchState()
        {
            if (IsPaused)
            {
                _pageSwitcher.Open(PageName.Stats);
                Play();
            }
            else
            {
                _pageSwitcher.Open(PageName.Pause);
                Pause();
            }

            IsPaused = !IsPaused;
        }
    }
}
