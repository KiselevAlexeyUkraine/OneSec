using Codebase.Player;
using Codebase.Services.Inputs;
using Player;
using Codebase.Components.Ui.Pages;
using UnityEngine;

namespace Codebase.Services

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
            if (_desktopInput.Escape)
            {
                if (playerMovement != null)
                {
                    if (playerMovement.IsDie == false)
                    {
                        SwitchState();
                    }
                }
                else
                {
                    SwitchState();
                }
            }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _cursorToggle.Enable();
            if (playerMovement != null && playerCombat != null)
            {
                playerMovement.enabled = false;
                playerCombat.enabled = false;
            }

        }

        public void Play()
        {
            Time.timeScale = 1f;
            _cursorToggle.Disable();

            if (playerMovement != null && playerCombat != null)
            {
                playerMovement.enabled = true;
                playerCombat.enabled = true;
            }

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
