using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Цель за которой движеться камера:")]
    [SerializeField] private Transform _target;

    [Space(15f)]
    [Header("Гладкость движения камеры:")]
    [SerializeField] private float _movementSmooth = 4f;





    private void FixedUpdate()
    {
        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _target.position, _movementSmooth * Time.deltaTime);
        }
    }
}
