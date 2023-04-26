using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDisplay : MonoBehaviour
{
    public GameObject content;

    private bool listDisplayed = false;
    public void ToggleDisplay()
    {
        listDisplayed = !listDisplayed;
        content.SetActive(listDisplayed);
    }
}
