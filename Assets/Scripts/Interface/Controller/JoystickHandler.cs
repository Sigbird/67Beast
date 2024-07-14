using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handler to capture touch and drag inputs on the UI element and returns it as Horizontal and Vertical float values.
/// </summary>
public class JoystickHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform _joystickBackground;
    private RectTransform _joystickHandle;
    private Vector2 _inputVector;

    private void Start()
    {
        _joystickBackground = GetComponent<RectTransform>();
        _joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();
    }
    
    #region CONVERTING/MOVING THE HANDLER
    /// <summary>
    /// Called by the OnPointer Down to conver the Screen pointer data to local point on the Input vector and Update the Ui element to follow it.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / _joystickBackground.sizeDelta.x);
            position.y = (position.y / _joystickBackground.sizeDelta.y);

            _inputVector = new Vector2(position.x * 2 - 1, position.y * 2 - 1);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            _joystickHandle.anchoredPosition = new Vector2(_inputVector.x * (_joystickBackground.sizeDelta.x / 2), _inputVector.y * (_joystickBackground.sizeDelta.y / 2));
        }
    }
    #endregion

    #region START/STOP CAPTURING TOUCH
    /// <summary>
    /// Start the on drag event in case the player start touching the handler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    /// <summary>
    /// Resets the handler and input vector in case the player stop touching the handler.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _joystickHandle.anchoredPosition = Vector2.zero;
    }
    #endregion

    #region  RETURNING THE VALUES
    /// <summary>
    /// Return the horizontal movement captured by the drag on the Ui element
    /// </summary>
    public float Horizontal()
    {
        return _inputVector.x;
    }

    /// <summary>
    /// Return the vertical movement captured by the drag on the Ui element
    /// </summary>
    public float Vertical()
    {
        return _inputVector.y;
    }
    #endregion
}
