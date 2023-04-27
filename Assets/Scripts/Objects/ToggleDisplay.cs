using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDisplay : MonoBehaviour
{
    public GameObject content;

    private bool listDisplayed = false;
    public void ShowHide()
    {
        listDisplayed = !listDisplayed;
        content.SetActive(listDisplayed);
    }
    public void ShowHideManual(bool show)
    {
        if (listDisplayed == show)
            return;
        Debug.Log("Dispaly switch");
        listDisplayed = show;
        content.SetActive(show);
    }
}
