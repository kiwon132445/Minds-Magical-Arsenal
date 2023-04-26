using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicControls : MonoBehaviour
{
    [SerializeField]
    private MagicManager _magicManager;

    public void OnLock(InputValue value)
    {
        Debug.Log("OnLock");
        if(value.isPressed)
        {
            _magicManager.LockInNode();
        }
    }
    public void OnCast(InputValue value)
    {
        Debug.Log("OnCast");
        if(value.isPressed)
        {
            _magicManager.Cast();
        }
    }
    public void OnForceCast(InputValue value)
    {
        Debug.Log("OnForceCast");
    }

    public void OnResetMode(InputValue value)
    {
        Debug.Log("OnResetMode");
        if(value.isPressed)
        {
            _magicManager.ReturnToDefault();
        }
    }

    public void OnExit(InputValue value)
    {
        Debug.Log("OnExit");
        if(value.isPressed)
        {
            _magicManager.Deactivate();
        }
    }
}