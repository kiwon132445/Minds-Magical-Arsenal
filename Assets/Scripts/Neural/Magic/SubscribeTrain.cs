using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using System;
using Newtonsoft.Json.Linq;

public class SubscribeTrain : MonoBehaviour
{
    BCITraining _bciTraining = BCITraining.Instance;
    DataStreamManager _dsManager = DataStreamManager.Instance;
    TrainingHandler _tHandler = TrainingHandler.Instance;
    // Start is called before the first frame update
    public event EventHandler<MentalCommandEventArgs> MentalUpdate;
    public event EventHandler<JObject> TrainedActionUpdate;
    void Awake()
    {
        _dsManager.FacialExpReceived += OnFacialExpReceived;
        _dsManager.MentalCommandReceived += OnMentalCommandReceived;
        _dsManager.SysEventsReceived += OnSysEventsReceived;
        _tHandler.GetTrainedSignatureActions += OnGetTrainedSignatureActions;
    }

    private void Start() {
        Subscribe();
    }

    private void OnDestroy() {
        UnSubscribe();
        _dsManager.FacialExpReceived -= OnFacialExpReceived;
        _dsManager.MentalCommandReceived -= OnMentalCommandReceived;
        _dsManager.SysEventsReceived -= OnSysEventsReceived;
    }

    private List<string> GetStreamsList() {
        List<string> _streams = new List<string> {};
        // _streams.Add("sys");
        //_streams.Add("fac");
        _streams.Add("com");
        return _streams;
    }

    public void Subscribe()
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Data Subscription request sent");
        _dsManager.SubscribeMoreData(GetStreamsList());
    }

    public void UnSubscribe()
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Data Unsubscription request sent");
        _dsManager.UnSubscribeData(GetStreamsList());
    }

    public void StartTrain(string action)
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Training request sent");
        _bciTraining.StartTraining(action, "mentalCommand");
    }

    public void ResetTraining(string action)
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Reset Training request sent");
        _bciTraining.ResetTraining(action, "mentalCommand");
    }

    public void DeleteTrain(string action)
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Delete Training request sent");
        _bciTraining.EraseTraining(action, "mentalCommand");
    }
    
    public void StopTrain(bool accept)
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        if (accept)
        {
            Debug.Log("Accept Training request sent");
            _bciTraining.AcceptTraining("mentalCommand");
        }
        else
        {
            Debug.Log("Reject Training request sent");
            _bciTraining.RejectTraining("mentalCommand");
        }
    }

    public void GetTrainedActions()
    {
        if (!TrainingProcessing.Instance.IsProfileConnected())
            return;
        Debug.Log("Training Data request sent");
        _bciTraining.GetTrainedSignatureActions("mentalCommand");
    }

    public void OnGetTrainedSignatureActions(object sender, JObject data)
    {
        TrainedActionUpdate(sender, data);
    }

    private void OnSysEventsReceived(object sender, SysEventArgs data)
    {
        Debug.Log("System event received");
        string dataText = "sys data: " + data.Detection + ", event: " + data.EventMessage + ", time " + data.Time.ToString();
        Debug.Log(dataText);
    }

    private void OnMentalCommandReceived(object sender, MentalCommandEventArgs data)
    {
        MentalUpdate(sender, data);
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