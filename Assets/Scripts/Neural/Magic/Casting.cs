using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmotivUnityPlugin;

public class Casting : MonoBehaviour
{
    BCITraining _bciTraining = BCITraining.Instance;
    DataStreamManager _dsManager = DataStreamManager.Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Subscription receiving function connected");
        _dsManager.FacialExpReceived += OnFacialExpReceived;
        _dsManager.MentalCommandReceived += OnMentalCommandReceived;
        _dsManager.SysEventsReceived += OnSysEventsReceived;
    }

    private List<string> GetStreamsList() {
        List<string> _streams = new List<string> {};
        _streams.Add("sys");
        _streams.Add("fac");
        _streams.Add("com");
        return _streams;
    }

    public void Subscribe()
    {
        Debug.Log("Data Subscription request sent");
        _dsManager.SubscribeMoreData(GetStreamsList());
    }

    public void UnSubscribe()
    {
        Debug.Log("Data Unsubscription request sent");
        _dsManager.UnSubscribeData(GetStreamsList());
    }

    public void StartTrain(string action)
    {
        _bciTraining.StartTraining(action, "mentalCommand");
    }

    public void StopTrain(bool accept)
    {
        if (accept)
        {
            _bciTraining.AcceptTraining("mentalCommand");
        }
        else
        {
            _bciTraining.RejectTraining("mentalCommand");
        }
    }

    private void OnSysEventsReceived(object sender, SysEventArgs data)
    {
        Debug.Log("System event received");
        string dataText = "sys data: " + data.Detection + ", event: " + data.EventMessage + ", time " + data.Time.ToString();
        Debug.Log(dataText);
    }

    private void OnMentalCommandReceived(object sender, MentalCommandEventArgs data)
    {
        Debug.Log("Mental event received");
        string dataText = "com data: " + data.Act + ", power: " + data.Pow.ToString() + ", time " + data.Time.ToString();
        Debug.Log(dataText);
    }

    private void OnFacialExpReceived(object sender, FacEventArgs data)
    {
        Debug.Log("Facial event received");
        string dataText = "fac data: eye act " + data.EyeAct+ ", upper act: " +
                            data.UAct + ", upper act power " + data.UPow.ToString() + ", lower act: " +
                            data.LAct + ", lower act power " + data.LPow.ToString() + ", time: " + data.Time.ToString();
        // print out data to console
        Debug.Log(dataText);
    }
}