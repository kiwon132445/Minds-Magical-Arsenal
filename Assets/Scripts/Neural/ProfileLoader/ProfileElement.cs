using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ProfileElement : MonoBehaviour
    {
        #region UI

        [SerializeField] private Text   nameField;
        [SerializeField] private Text   loadingLabel;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button removeButton;

        private Profile _profileInformation;

        void Start ()
        {
            loadButton.onClick.AddListener(startLoadingProfile);
            removeButton.onClick.AddListener(startDeleteProfile);
            //TrainingProcessing.Instance.ProfileConnected += connectSuccess;
            //TrainingProcessing.Instance.ProfileConnectFail += connectFailed;
        }

        void OnDestroy()
        {
            //TrainingProcessing.Instance.ProfileConnected -= connectSuccess;
            //TrainingProcessing.Instance.ProfileConnectFail -= connectFailed;
        }
        
        public ProfileElement SetName (string name)
        {			
            this.nameField.text = name;
            return this;
        }

        public string GetName()
        {
            return this.nameField.text;
        }

        private void startLoadingProfile ()
        {
            loadingLabel.gameObject.SetActive (true);
            loadButton.gameObject.SetActive (false);
            removeButton.gameObject.SetActive (false);
            loadSuccess(this._profileInformation.ProfileID);
            //List<string> dataStreamList = new List<string>(){DataStreamName.DevInfos};
            //DataStreamManager.Instance.StartDataStream(dataStreamList, _headsetInformation.HeadsetID);
        }

        private void startDeleteProfile()
        {
            loadingLabel.gameObject.SetActive (true);
            loadButton.gameObject.SetActive (false);
            removeButton.gameObject.SetActive (false);
            removeSuccess(this._profileInformation.ProfileID);
        }
        
        private void loadSuccess(string profileID)
        {
            if (_profileInformation.ProfileID == profileID) {
                mainController.StartProfileForms (_profileInformation, () => {
                });
                TrainingProcessing.Instance.EnableQueryProfile(true);
                TrainingProcessing.Instance.SetConnectedProfile (_profileInformation);
            } else {
                Debug.Log("Another profile loaded or wrong somewhere");
            }
        }

        private void removeSuccess(string profileID)
        {
            if (_profileInformation.ProfileID == profileID) {
                TrainingProcessing.Instance.EnableQueryProfile(true);
                mainController.StartProfileDelete (this, () => {
                });
            } else {
                Debug.Log("Another profile connected or wrong somewhere");
            }
        }

        private void connectFailed(object sender, string profileID)
        {
            TrainingProcessing.Instance.EnableQueryProfile(true);
        }

        #endregion

        #region Dependency Injection

        private ProfileController mainController;

        [Inject]
        public void SetDependencies (ProfileController mainController)
        {
            this.mainController = mainController;
        }

        #endregion

        public ProfileElement WithInformationAdd (Profile info)
        {
            this._profileInformation = info;
            SetName (this._profileInformation.ProfileID);
            //SetConnectionType (this._profileInformation.ProfileConnection);
            return this;
        }

        public class Factory:PlaceholderFactory<ProfileElement>
        {
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}