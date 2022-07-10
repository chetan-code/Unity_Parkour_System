using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Manager Inputs from player;
    /// </summary>
    public static float MouseX { get; private set; }
    public static float MouseY { get; private set; }

    public static float Horizontal { get; private set;}
    public static float Vertical { get; private set; }

    [SerializeField] bool _invertX;
    [SerializeField] bool _invertY;

    float _invertXVal;
    float _invertYVal;

    private void Update()
    {
        _invertXVal = (_invertX) ? -1 : 1;
        _invertYVal = (_invertY) ? -1 : 1;

        MouseX = Input.GetAxis("Mouse X") * _invertXVal;
        MouseY = Input.GetAxis("Mouse Y")* _invertYVal;
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

    }

}
