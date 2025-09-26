using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _gravityForce;

    private CharacterController _controller;

    private Coroutine _movementCoroutine;
    private Coroutine _gravityCoroutine;
    
    private Vector3 direction;
    private Vector2 _movement;

    private bool _isMoving;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        _gravityCoroutine = StartCoroutine(ApplyGravity());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void MoveAction(InputAction.CallbackContext callbackContext)
    {
        _movement = callbackContext.ReadValue<Vector2>();

        if (callbackContext.performed && _movement.magnitude > 0.05)
        {
            _isMoving = true;

            if (_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);

            _movementCoroutine = StartCoroutine(ContinousMovement());
        }

        if (callbackContext.canceled || _movement.magnitude <= 0.05)
        {
            _isMoving = false;

            StopCoroutine(_movementCoroutine);
        }
    }

    private IEnumerator ContinousMovement()
    {
        while (_isMoving)
        {
            MoveCharacter();            
            yield return null;
        }
    }

    private IEnumerator ApplyGravity()
    {
        while (true)
        {
            Vector3 gravity = Vector3.down * _gravityForce;

            _controller.Move(gravity);
            yield return null;
        }
    }    

    private void MoveCharacter()
    {
        direction = new Vector3(_movement.x, Vector3.zero.y, _movement.y).normalized;   
        Vector3 motion = _playerSpeed * direction * Time.deltaTime;        
        _controller.Move(motion);

        CharacterRotation();
    }

    private void CharacterRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);        
    }
}
