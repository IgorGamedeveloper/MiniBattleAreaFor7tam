using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Компонент PhotonView подконтрольного джойстиком персонажа")]
    [SerializeField] private PhotonView _photonView;

    [Space(35f)]
    [Header("Зона в которой джойстик может смещатся:")]
    [SerializeField] private RectTransform _joystickZone;

    [Space(10f)]
    [Header("Контур смещения джойстика:")]
    [SerializeField] private RectTransform _joystickBackground;

    [Space(10f)]
    [Header("Рукоятка джойстика:")]
    [SerializeField] private RectTransform _joystickHandler;

    [Space(20f)]
    [Header("Мертвая зона осей джойстика:")]
    [SerializeField] private float _deadSpotAxis = 0.2f;

    [Space(20f)]
    [Header("Округлять вывод данных джойстика:")]
    [SerializeField] private bool _rawInput;

    private Vector2 _joystickBackgroundStartPosition;

    private Vector2 _inputDirection;
    private Vector2 _rawInputDirection;

    public delegate void InputUpdate(Vector2 input);
    public event InputUpdate inputUpdate;

    public delegate void InputTerminated();
    public event InputTerminated inputTerminated;

    [Space(20f)]
    [Header("Считывать ввод в выбранном обновлении Unity:")]
    [SerializeField] private Updates updates = Updates.Update;

    private enum Updates
    {
        Update,
        FixedUpdate,
        LateUpdate
    }




    private void Start()
    {
        _joystickBackgroundStartPosition = _joystickBackground.anchoredPosition;
    }

    private void Update()
    {
        if (_photonView != null && _photonView.IsMine == true)
        {
            if (updates == Updates.Update)
            {
                GetInput();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_photonView != null && _photonView.IsMine == true)
        {
            if (updates == Updates.FixedUpdate)
            {
                GetInput();
            }
        }
    }

    private void LateUpdate()
    {
        if (_photonView != null && _photonView.IsMine == true)
        {
            if (updates == Updates.LateUpdate)
            {
                GetInput();
            }
        }
    }

    private void GetInput()
    {
        if (_inputDirection != Vector2.zero)
        {
            if (_rawInput == true)
            {
                if (_inputDirection.x > 0 && _inputDirection.x > _deadSpotAxis)
                {
                    _rawInputDirection.x = 1.0f;
                }
                else if (_inputDirection.x < 0 && _inputDirection.x < -_deadSpotAxis)
                {
                    _rawInputDirection.x = -1.0f;
                }

                if (_inputDirection.y > 0 && _inputDirection.y > _deadSpotAxis)
                {
                    _rawInputDirection.y = 1.0f;
                }
                else if (_inputDirection.y < 0 && _inputDirection.y < -_deadSpotAxis)
                {
                    _rawInputDirection.y = -1.0f;
                }

                inputUpdate?.Invoke(_rawInputDirection);

            }
            else
            {
                inputUpdate?.Invoke(_inputDirection);
            }
        }
        else
        {
            inputTerminated?.Invoke();
        }
    }




    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickZone, eventData.position, null, out Vector2 joystickBackgroundPosition) == true)
        {
            _joystickBackground.anchoredPosition = joystickBackgroundPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground, eventData.position, null, out Vector2 joystickHandlerPosition) == true)
        {
            joystickHandlerPosition = (joystickHandlerPosition * 2 / _joystickBackground.sizeDelta);
        }

        if (joystickHandlerPosition.magnitude > 1.0f)
        {
            _inputDirection = joystickHandlerPosition.normalized;
        }
        else
        {
            _inputDirection = joystickHandlerPosition;
        }

        _joystickHandler.anchoredPosition = _inputDirection * (_joystickBackground.sizeDelta / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.anchoredPosition = _joystickBackgroundStartPosition;
        _inputDirection = Vector2.zero;
        _joystickHandler.anchoredPosition = Vector2.zero;
    }
}
