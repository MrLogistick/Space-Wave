using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class MultiGraphicButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public List<Graphic> graphicsToTint;

    private Button button;
    private Color normalColor;
    private Color highlightedColor;
    private Color pressedColor;

    private void Awake()
    {
        button = GetComponent<Button>();
        var colors = button.colors;

        normalColor = colors.normalColor;
        highlightedColor = colors.highlightedColor;
        pressedColor = colors.pressedColor;
    }

    private void SetColor(Color color)
    {
        foreach (var graphic in graphicsToTint)
        {
            if (graphic != null) graphic.color = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => SetColor(highlightedColor);
    public void OnPointerExit(PointerEventData eventData) => SetColor(normalColor);
    public void OnPointerDown(PointerEventData eventData) => SetColor(pressedColor);
    public void OnPointerUp(PointerEventData eventData) => SetColor(highlightedColor);
}
