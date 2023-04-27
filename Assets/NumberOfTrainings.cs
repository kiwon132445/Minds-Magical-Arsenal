using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;

public class NumberOfTrainings : MonoBehaviour
{
    public TMP_Text Neutral;
    public TMP_Text Push;
    public TMP_Text Pull;
    public TMP_Text Right;
    public TMP_Text Left;

    Dictionary<string, string> text = new Dictionary<string, string>()
    {
        {"neutral", "Neutral: "},
        {"push", "Push: "},
        {"pull", "Pull: "},
        {"right", "Right: "},
        {"left", "Left: "},
    };
    public Dictionary<string, int> tmp = new Dictionary<string, int>()
    {
        {"neutral", 0},
        {"push", 0},
        {"pull", 0},
        {"right", 0},
        {"left", 0},
    };

    public void LoadTrainingData(JObject data)
    {
        foreach (JToken d in data["trainedActions"])
        {
            tmp[d["action"].ToString()] = (int) d["times"];
            ChangeValues(d["action"].ToString(), (int) d["times"]);
        }
    }

    public void ChangeValues(string action, int num)
    {
    switch(action)
        {
            case "neutral":
                Neutral.text = text[action] + num;
                break;
            case "push":
                Push.text = text[action] + num;
                break;
            case "pull":
                Pull.text = text[action] + num;
                break;
            case "right":
                Right.text = text[action] + num;
                break;
            case "left":
                Left.text = text[action] + num;
                break;
        }
    }
}
