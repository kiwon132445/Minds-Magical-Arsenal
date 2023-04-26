using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public StarterAssets.FirstPersonController _controller;
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller.PlayerInputActive(true);
        GameManager.Instance.ActivateOptions(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
