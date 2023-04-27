using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using EmotivUnityPlugin;
using TMPro;

namespace dirox.emotiv.controller
{
    public class ProfileCreate : MonoBehaviour
    {
      [SerializeField]
      GameObject profileInputField;

      [SerializeField]
      GameObject ErrorText;

      public void CreateProfile()
      {
        int result;
        //Debug.Log(idInputField.GetComponent<TMP_Text>().text);
        if (profileInputField.GetComponent<TMP_InputField>().text == "")
        {
          result = TrainingProcessing.Instance.CreateProfile(profileInputField.GetComponent<TMP_InputField>().transform.Find("Text Area").transform.Find("Placeholder").GetComponent<TMP_Text>().text);
        }
        else
        {
          result = TrainingProcessing.Instance.CreateProfile(profileInputField.GetComponent<TMP_InputField>().text);
        }

        if (result < 0)
        {
          ErrorText.SetActive(true);
        }
        else
        {
          ErrorText.SetActive(false);
        }
      }
    }
}