using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;

    private CharacterController _controller;

    private Vector2 _movement;

    private bool _isMoving;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void MoveAction(InputAction.CallbackContext callbackContext)
    {
        _movement = callbackContext.ReadValue<Vector2>();

        if (callbackContext.performed && _movement.magnitude > 0.1f)
        {
            _isMoving = true;

            StopAllCoroutines();
            StartCoroutine(ContinousMovement());
        }

        if (callbackContext.canceled || _movement.magnitude <= 0.1f)
        {
            _isMoving = false;

            StopAllCoroutines();
        }
    }

    private IEnumerator ContinousMovement()
    {
        while (_isMoving)
        {
            MoveCharacter();
            yield return new WaitForFixedUpdate();
        }
    }

    private void MoveCharacter()
    {
        Vector3 direction = new Vector3(_movement.x, Vector3.zero.y, _movement.y).normalized;

        Vector3 motion = _playerSpeed * direction * Time.fixedDeltaTime;

        Debug.Log(motion);

        _controller.Move(motion);

        CharacterRotation();
    }

    private void CharacterRotation()
    {
        Vector3 direction = new Vector3(_movement.x, Vector3.zero.y, _movement.y);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);        
    }
}
