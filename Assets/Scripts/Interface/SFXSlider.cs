using UnityEngine;
using UnityEngine.EventSystems;

public class SFXSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] TitlePanel titleManager;

    public void OnPointerUp(PointerEventData eventData) {
        titleManager.sfxChanging = false;
    }
}