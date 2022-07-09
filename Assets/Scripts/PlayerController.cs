using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float _moveSpeed = 5f;
    [SerializeField]float _rotationSpeed = 500f;

    CameraController _cameraController;

    Quaternion _targetRotation;
    private void Awake()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        var inputMagnitude = Mathf.Abs(PlayerInput.Horizontal) + Mathf.Abs(PlayerInput.Vertical);
        //normalise the vector so it is unit vector in all direction
        //and player doesnt move fast diagonally
        var moveInput = new Vector3(PlayerInput.Horizontal, 0, PlayerInput.Vertical).normalized;
        //moving based on camera rotation
        var moveDir =  _cameraController.PlanerRotation * moveInput;
        //we should only move and rotate when we have some move input
        if (inputMagnitude > 0) {
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
            _targetRotation = Quaternion.LookRotation(moveDir);
        }
        //smoothly lerping from one rotation to other with time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation,
            _rotationSpeed * Time.deltaTime);


    }
}
