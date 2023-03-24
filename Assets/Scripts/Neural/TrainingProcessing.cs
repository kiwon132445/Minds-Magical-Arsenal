using UnityEngine;
using EmotivUnityPlugin;
using System;
using System.Collections.Generic;

public class TrainingProcessing
{
    static TrainingProcessing _instance = null;
    static readonly object _object = new object();
    Dictionary<string, dirox.emotiv.controller.Profile> _profileList = new Dictionary<string, dirox.emotiv.controller.Profile>();

    public event EventHandler onProfileChange;
    public event EventHandler onCurrProfileRemoved;
    public event EventHandler onDetectManyProfiles;
    public event EventHandler<string> ProfileConnected;
    public event EventHandler<string> ProfileConnectFail;

    dirox.emotiv.controller.Profile _curProfileConnected = null;

    bool _enableQueryProfile  = true;
    bool _isConnect           = false;
    
    ~TrainingProcessing()
    {
    }

    static public TrainingProcessing Instance
    {
        get {
            if (_instance == null)  {
                _instance = new TrainingProcessing();
            }
            return _instance;
        }
    }

    public void SetConnectedProfile (dirox.emotiv.controller.Profile profile)
    {
        lock (_object) {
            // Debug.Log("======== SetConnectedProfile " + profile.ProfileID);
            _curProfileConnected = profile;
        }
    }

    // disable query profile while connecting to a profile
    // and enable it again after done connecting process(success or failed)
    public void EnableQueryProfile(bool enable)
    {
        lock (_object) {
            _enableQueryProfile = enable;
        }
    }

    void profileListUpdate()
    {				              
        if (onProfileChange != null)
            onProfileChange(null, null);		
    }

    public bool IsProfileConnected() {
        lock (_object) {
            return _isConnect;
        }
    }

    public Dictionary<string, dirox.emotiv.controller.Profile> GetHeadsetList()
    {
        lock (_object) {
            return _profileList;
        }
    }

    // return number of profile discovered
    int queryProfile ()
    {
        if (!_enableQueryProfile)
            return 0;
        
        BCITraining.Instance.QueryProfile();
        List<string> detectedProfile = BCITraining.Instance.ProfileLists;

        if (detectedProfile == null)
        {
            return 0;
        }

        _profileList.Clear();
        foreach (var item in detectedProfile) {
            _profileList[item] = new dirox.emotiv.controller.Profile(item);
            Debug.Log(item);
        }

        // Detect the profile is not loaded
        if(_curProfileConnected != null)
        {
            bool isDisconnected = false;
            if (!_profileList.ContainsKey(_curProfileConnected.ProfileID)) {
                isDisconnected = true;
            } else {
                //isDisconnected = (_profileList[_curProfileConnected.ProfileID].Status == HeadsetConnectionStatus.DISCOVERED);
            }

            if (isDisconnected) {
                Debug.Log("TrainingProcessing:queryProfile - Disconnected the headset");

                if (onCurrProfileRemoved != null)
                    onCurrProfileRemoved(null, null);

                _curProfileConnected = null;
            }
        }

        profileListUpdate();

        return detectedProfile.Count;
    }

    bool checkProfileConnected()
    {
        if (_curProfileConnected == null || _curProfileConnected.ProfileID == "") {
            _isConnect = false;
        } else {
            _isConnect = true;
        }

        return _isConnect;
    }

    public void Process()
    {
        //Debug.Log("Training Process");
        lock (_object) {
            if (!DataProcessing.Instance.IsHeadsetConnected())
            {
                return;
            }
            //Debug.Log("Query");
            if (queryProfile () <= 0)
                return;

            if (!checkProfileConnected()) {
                return;
            }
        }
    }

}