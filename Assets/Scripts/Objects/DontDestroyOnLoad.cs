using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Don't Destroy on Load tutorial
* https://www.youtube.com/watch?v=HXaFLm3gQws
*/

public class DontDestroyOnLoad : MonoBehaviour
{
    [HideInInspector]
    public string objectID;
    private void Awake()
    {
        objectID = name + transform.position.ToString();
    }

    void Start()
    {
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroyOnLoad>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
