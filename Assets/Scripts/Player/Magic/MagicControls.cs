using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicControls : MonoBehaviour
{
    [SerializeField]
    private MagicManager _magicManager;
    public bool lift;
    public bool drop;

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
    public void OnLift(InputValue value)
    {
        Debug.Log("OnLift");
        if(value.isPressed)
        {
            Debug.Log("OnLift");
            lift = true;
        }
        else
            lift = false;
    }
    public void OnDrop(InputValue value)
    {
        if(value.isPressed)
        {
            Debug.Log("OnDrop");
            drop = true;
        }
        else
            drop = false;
    }

    public void OnResetMode(InputValue value)
    {
        Debug.Log("OnResetMode");
        if(value.isPressed)
        {
            _magicManager.ReturnToDefault(true);
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