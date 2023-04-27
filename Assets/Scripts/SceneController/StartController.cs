using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartController : MonoBehaviour
{
    [SerializeField]
    GameObject idInputField;

    [SerializeField]
    GameObject secretInputField;
    public void LoadCortex()
    {
        LoginDetails();
        Debug.Log("Appconfig filled");
        SceneManager.LoadScene("NeuralHeadset");
    }

    void LoginDetails()
    {
        //Debug.Log(idInputField.GetComponent<TMP_Text>().text);
        if (idInputField.GetComponent<TMP_InputField>().text == "")
        {
            AppConfig.ClientId = idInputField.GetComponent<TMP_InputField>().transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text;
        }
        else
        {
            AppConfig.ClientId = idInputField.GetComponent<TMP_InputField>().text;
        }
        if (secretInputField.GetComponent<TMP_InputField>().text == "")
        {
            AppConfig.ClientSecret = secretInputField.GetComponent<TMP_InputField>().transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text;
        }
        else
        {
            AppConfig.ClientSecret = secretInputField.GetComponent<TMP_InputField>().text;
        }
    }
}
