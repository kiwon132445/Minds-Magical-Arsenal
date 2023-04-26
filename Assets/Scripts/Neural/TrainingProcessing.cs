using UnityEngine;
using EmotivUnityPlugin;
using System;
using System.Collections.Generic;

public class TrainingProcessing
{
    static TrainingProcessing _instance = null;
    static readonly object _object = new object();
    Dictionary<string, dirox.emotiv.controller.Profile> _profileList = new Dictionary<string, dirox.emotiv.controller.Profile>();
    //Dictionary<string, dirox.emotiv.controller.Profile> _deletedProfileList = new Dictionary<string, dirox.emotiv.controller.Profile>();

    public event EventHandler onProfileChange;
    public event EventHandler onCurrProfileChanged;

    private BCITraining _bciTraining = BCITraining.Instance;
    dirox.emotiv.controller.Profile _curProfileConnected = null;

    bool _enableQueryProfile  = false;
    bool _isConnect           = false;
    public static Headset headset;

    public Headset StaticHeadset   // property
  {
    get { return headset; }   // get method
    set { headset = value; }  // set method
  }
    
    ~TrainingProcessing()
    {
        // CortexClient.Instance.DeleteProfileOK -= DeleteProfileOK;
        // BCITraining.Instance.InformLoadUnLoadProfileDone -= OnInformLoadUnLoadProfileDone;
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

    public int CreateProfile(string profileID)
    {
        if (_profileList.ContainsKey(profileID)) {
            Debug.Log("Profile with the name " + profileID + " exists, delete existing profile before creating a new profile");
            return -1;
        }
        Debug.Log("Profile: " + profileID + ". created");
        //BCITraining.Instance.WantedProfileName = profileID;
        _bciTraining.CreateProfile(profileID);
        // if (_deletedProfileList.ContainsKey(profileID))
        //     _deletedProfileList.Remove(profileID);
        Process();
        return 0;
    }

    public void LoadProfileWithHeadset(string profileID, string headsetID)
    {
        lock (_object) {
            _bciTraining.LoadProfileWithHeadset(profileID, headsetID);
        }
    }

    public void UnloadProfile(string profileID, string headsetID)
    {
        lock (_object) {
            _bciTraining.UnLoadProfile(profileID, headsetID);
        }
    }

    public void DeleteProfile(string profileName)
    {
        lock (_object) {
            string cortexToken = Authorizer.Instance.CortexToken;
            CortexClient.Instance.SetupProfile(cortexToken, profileName, "delete");
            //_deletedProfileList[profileName] = new dirox.emotiv.controller.Profile(profileName);
        }
    }

    public void SaveCurProfile(string headsetID)
    {
        lock (_object) {
            _bciTraining.SaveProfile(_curProfileConnected.ProfileID, headsetID);
        }
    }

    public void SetConnectedProfile (dirox.emotiv.controller.Profile profile)
    {
        lock (_object) {
            // Debug.Log("======== SetConnectedProfile " + profile.ProfileID);
            _curProfileConnected = profile;
        }
    }

    public void UnsetConnectedProfile (dirox.emotiv.controller.Profile profile)
    {
        lock (_object) {
            // Debug.Log("======== SetConnectedProfile " + profile.ProfileID);
            _curProfileConnected = null;
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

    public Dictionary<string, dirox.emotiv.controller.Profile> GetProfileList()
    {
        lock (_object) {
            return _profileList;
        }
    }

    // return number of profile discovered
    int queryProfile ()
    {
        lock (_object) {
            if (!_enableQueryProfile)
                return 0;
            
            _bciTraining.QueryProfile();
            List<string> detectedProfile = _bciTraining.ProfileLists;

            if (detectedProfile == null)
            {
                return 0;
            }

            _profileList.Clear();
            foreach (var item in detectedProfile) {
                // if (!_deletedProfileList.ContainsKey(item))
                // {
                    _profileList[item] = new dirox.emotiv.controller.Profile(item);
                    Debug.Log(item);
                // }
            }

            // Detect the profile is loaded
            if(_curProfileConnected != null)
            {
                bool isDisconnected = false;
                if (!_profileList.ContainsKey(_curProfileConnected.ProfileID)) {
                    isDisconnected = true;
                }

                if (isDisconnected) {
                    Debug.Log("TrainingProcessing:queryProfile - Disconnected the headset");

                    if (onCurrProfileChanged != null)
                        onCurrProfileChanged(null, null);

                    _curProfileConnected = null;
                }
            }

            profileListUpdate();
            return detectedProfile.Count;
        }
    }

    bool checkProfileConnected()
    {
        lock (_object) {
            if (_curProfileConnected == null || _curProfileConnected.ProfileID == "") {
                _isConnect = false;
            } else {
                _isConnect = true;
            }

            return _isConnect;
        }
    }

    public void Process()
    {
        //Debug.Log("Training Process");
        lock (_object) {
            //Debug.Log("Query");
            if (queryProfile () <= 0)
                return;
            
            if (!checkProfileConnected()) {
                return;
            }
        }
    }
}