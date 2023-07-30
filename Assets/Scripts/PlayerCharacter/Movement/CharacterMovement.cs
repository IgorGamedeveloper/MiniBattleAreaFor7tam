using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [Header("Ссылка на компонент джойстика движения:")]
    [SerializeField] private VirtualJoystick _movementJoystick;

    [Space(10f)]
    [Header("Ссылка на компонент джойстика вращения:")]
    [SerializeField] private VirtualJoystick _rotateJoystick;

    [Space(25f)]
    [Header("Скорость движения персонажа:")] 
    [SerializeField] private float _moveStepSize = 8f;






    private void OnEnable()
    {
        _movementJoystick.inputUpdate += Move;
        _movementJoystick.inputTerminated += StopMove;

        _rotateJoystick.inputUpdate += Rotate;
    }

    private void OnDisable()
    {
        _movementJoystick.inputUpdate -= Move;
        _movementJoystick.inputTerminated -= StopMove;

        _rotateJoystick.inputUpdate -= Rotate;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0.0f;
    }




    private void Move(Vector2 inputDirection)
    {
        _rigidbody.velocity = inputDirection.normalized * _moveStepSize;
    }

    private void StopMove()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void Rotate(Vector2 inputDirection)
    {
        if (inputDirection.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
