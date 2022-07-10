using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 5f;
    [SerializeField]float _rotationSpeed = 500f;

    [Header("Ground Check Settings")]
    [SerializeField] Vector3 _groundCheckOffset;
    [SerializeField] float _groundCheckRadius;
    [SerializeField] LayerMask _groundLayer;


    CameraController _cameraController;
    CharacterController _characterController;
    Animator _animator;
    bool _isGrounded;
    float _fallVelocity;
    Quaternion _targetRotation;

    private void Awake()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        var inputMagnitude = Mathf.Abs(PlayerInput.Horizontal) + Mathf.Abs(PlayerInput.Vertical);
        //normalise the vector so it is unit vector in all direction
        //and player doesnt move fast diagonally
        var moveInput = new Vector3(PlayerInput.Horizontal, 0, PlayerInput.Vertical).normalized;
        //moving based on camera rotation
        var moveDir =  _cameraController.PlanerRotation * moveInput;

        //if player is ground - if not add y velocity;
        GroundCheck();
        if (_isGrounded)
        {
            _fallVelocity = -0.5f;//making sure it is in small minus value so player remain grounded    
        }
        else {
            _fallVelocity += Physics.gravity.y * Time.deltaTime; // v = at (we only consider y axis)
        }
        Vector3 velocity = moveDir * _moveSpeed;
        velocity.y = _fallVelocity;

        //using character controller to move character - handles collisions
        _characterController.Move(velocity * Time.deltaTime);

        //we should only move and rotate when we have some move input
        if (inputMagnitude > 0) {
            _targetRotation = Quaternion.LookRotation(moveDir);
        }
        //smoothly lerping from one rotation to other with time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
            _rotationSpeed * Time.deltaTime);

        _animator.SetFloat("moveAmount", Mathf.Clamp01(inputMagnitude), 0.2f, Time.deltaTime);

    }

    private void GroundCheck() {
        _isGrounded = Physics.CheckSphere(transform.TransformPoint(_groundCheckOffset), 
            _groundCheckRadius,
            _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (_isGrounded)?Color.green : Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(_groundCheckOffset),_groundCheckRadius);
    }


}
