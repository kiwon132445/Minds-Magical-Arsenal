using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject _game;
    public GameObject _magic;
    public GameObject _options;

    private void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
    }

    public void ActivateGame(bool activate)
    {
        if (activate)
        {
            _game.SetActive(true);
            GameObject[] list = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject p in list)
            {
                p.SetActive(true);
            }
        }
        else
        {
            _game.SetActive(false);
            GameObject[] list = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject p in list)
            {
                p.SetActive(false);
            }
        }
    }

    public void ActivateMagic(bool activate)
    {
        if (activate)
        {
            _magic.SetActive(true);
        }
        else
        {
            _magic.SetActive(false);
        }
    }

    public void ActivateOptions(bool activate)
    {
        if (activate)
        {
            Time.timeScale = 0;
            _options.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _options.SetActive(false);
        }
    }
}
