using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonPressing : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("PRESS SETTINGS")]
    [Range(1f, 3f)] public float holdDuration = 2f;
    public UnityEvent onLongPress;
    public UnityEvent onClick;
    private bool isPointerDown = false;
    private bool isLongPressed = false;
    private float elapsedTime = 0f;
    private float feedbackTime = 1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    private void Update()
    {
        if (isPointerDown && !isLongPressed)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= feedbackTime && elapsedTime >= holdDuration)
            {
                    isLongPressed = true;
                    elapsedTime = 0f;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLongPressed)
        {
            onLongPress.Invoke();
            isLongPressed = false;
        }
        else
        {
            onClick.Invoke();
        }
        isPointerDown = false;
        elapsedTime = 0f;
    }
}
