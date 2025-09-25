using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;

    private CharacterController _controller;

    private Vector2 _movement;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void MoveAction(InputAction.CallbackContext callbackContext)
    {
        _movement = callbackContext.ReadValue<Vector2>();

        if (callbackContext.performed)
        {
            StopAllCoroutines();
            StartCoroutine(ContinousMovement());
        }

        if (callbackContext.canceled)
            StopAllCoroutines();

    }

    private IEnumerator ContinousMovement()
    {
        while (true)
        {
            MoveCharacter();
            yield return new WaitForFixedUpdate();
        }
    }

    private void MoveCharacter()
    {
        Vector3 motion = new Vector3(_movement.x * _playerSpeed * Time.deltaTime, 0f, _movement.y * _playerSpeed * Time.deltaTime);
        _controller.Move(motion);

        CharacterRotation();
    }

    private void CharacterRotation()
    {
        // transform.Rotate(transform.rotation.x, _movement.x + _movement.y, transform.rotation.z);

        Vector2 targetRotationY = new Vector2(_movement.x, _movement.y);

        transform.localRotation = Quaternion.LookRotation(new Vector3(_movement.x, 0, _movement.y));
    }
}
