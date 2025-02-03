using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Components.Ui
{
    public class LinkButton : MonoBehaviour
    {
        [SerializeField]
        public Button _button;
        [SerializeField]
        public string _url;

        private void Awake()
        {
            _button.onClick.AddListener(() => Application.OpenURL(_url));
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}