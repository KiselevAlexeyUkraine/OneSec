using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using Codebase.Services;

public class CatScene : MonoBehaviour
{
    [SerializeField]
    private Button[] m_Buttons;

    private AudioService _audioService;

    [Inject]
    private void Construct(AudioService audioService)
    {
        _audioService = audioService;
    }

    private void Awake()
    {
        foreach (var button in m_Buttons)
        {
            button.onClick.AddListener(() =>
            {
                _audioService.PlayClickSound();
            });

            AddHoverSound(button);
        }
    }

    private void OnDestroy()
    {
        foreach (var button in m_Buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    private void AddHoverSound(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((_) => _audioService.PlayHoverSound());
        trigger.triggers.Add(entry);
    }
}
