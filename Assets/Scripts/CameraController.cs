using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Third person camera controller
    /// Mouse X - movement of camera around player (in a circular orbit around player)
    /// Mouse Y - up and down movement of camera
    /// NOTE : we are rotating vectors (muliplying quaternions with vector to achieve it)
    /// </summary>

    [SerializeField] Transform _followTarget;
    [SerializeField] float _distanceFromTarget = 5;
    [SerializeField] float _minVerticalAngle = -45;
    [SerializeField] float _maxVerticalAngle = 45;
    [SerializeField] Vector2 _framingOffset;

    float _rotationX;
    float _rotationY;


    private void OnValidate()
    {
        if (_followTarget == null) return;
        //just to make sure camera moved if we edit distance [Edit Mode - visualisation]
        transform.position = _followTarget.position + (Vector3)_framingOffset - Vector3.forward * _distanceFromTarget;
    }

    private void Update()
    {
        //Y movement of mouse is moving camera in XY plane - up down
        _rotationX += Input.GetAxis("Mouse Y");
        _rotationX = Mathf.Clamp(_rotationX, _minVerticalAngle, _maxVerticalAngle);

        //X movement of mouse is moving camera in XZ plane - sidewise
        _rotationY += Input.GetAxis("Mouse X");

        var targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0f);

        //we add some offset to target position - for looking at head of player
        var focusPoint = _followTarget.position + (Vector3)_framingOffset;

        transform.position = focusPoint - (targetRotation * new Vector3(0,0,_distanceFromTarget));
        transform.rotation = targetRotation;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, _followTarget.position);
        //NOTE : Handles library can create problem during compilation for android therefore using #preprocessor
#if UNITY_EDITOR
        Handles.color = Color.red;
        Vector3[] points = new Vector3[] { };
        Handles.DrawDottedLines(new Vector3[] { transform.position, _followTarget.position }, 5);
        Handles.DrawWireDisc(_followTarget.position + (Vector3)_framingOffset, transform.up, _distanceFromTarget);
#endif
    }

}
