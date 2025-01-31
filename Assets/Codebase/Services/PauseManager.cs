using Codebase.Services.Inputs;
using UI;
using UnityEngine;

namespace Services
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField]
        private PageSwitcher _pageSwitcher;

        private CursorToggle _cursorToggle;

        private DesktopInput _desktopInput = new();

        public bool IsPaused { get; private set; }

        private void Awake()
        {
            _cursorToggle = new CursorToggle();
            Play();
        }

        private void Update()
        {
            if (_desktopInput.Escape)
            {
                SwitchState();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _cursorToggle.Enable();
        }

        public void Play()
        {
            Time.timeScale = 1f;
            _cursorToggle.Disable();
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
