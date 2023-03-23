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
        //[SerializeField] private Button dongleButton;
        //[SerializeField] private Button bluetoothButton;
        //[SerializeField] private Button cableButton;
        [SerializeField] private Button loadButton;

        private Profile _profileInformation;

        bool _isLoaded = false; // fixed the issue get much of event from only 1 signal

        void Start ()
        {
            /*
            var dongleSprite = Resources.Load<Sprite>("btnDongle");
            dongleButton.image.sprite = dongleSprite;
            dongleButton.onClick.AddListener (startConnectToDevice);

            var btleSprite = Resources.Load<Sprite>("btnBluetooth");
            bluetoothButton.image.sprite = btleSprite;
            bluetoothButton.onClick.AddListener (startConnectToDevice);

            var cableSprite = Resources.Load<Sprite>("btnCable");
            cableButton.image.sprite = cableSprite;
            cableButton.onClick.AddListener (startConnectToDevice);
            */
            //DataProcessing.Instance.HeadsetConnected += connectSuccess;
            //DataProcessing.Instance.HeadsetConnectFail += connectFailed;
        }

        void OnDestroy()
        {
            //DataProcessing.Instance.HeadsetConnected -= connectSuccess;
            //DataProcessing.Instance.HeadsetConnectFail -= connectFailed;
        }
        
        public ProfileElement SetName (string name)
        {			
            this.nameField.text = name;
            return this;
        }

        /*
        public ProfileElement SetConnectionType (ConnectionType connectionType)
        {
            dongleButton.gameObject.SetActive (connectionType    == ConnectionType.CONN_TYPE_DONGLE);
            bluetoothButton.gameObject.SetActive (connectionType == ConnectionType.CONN_TYPE_BTLE);
            cableButton.gameObject.SetActive (connectionType     == ConnectionType.CONN_TYPE_USB_CABLE);
            return this;
        }*/

        private void startConnectToDevice ()
        {
            loadingLabel.gameObject.SetActive (true);
            /*
            dongleButton.gameObject.SetActive (false);
            bluetoothButton.gameObject.SetActive (false);
            cableButton.gameObject.SetActive (false);
            */
            loadButton.gameObject.SetActive (false);
            //List<string> dataStreamList = new List<string>(){DataStreamName.DevInfos};
            //DataStreamManager.Instance.StartDataStream(dataStreamList, _profileInformation.HeadsetID);
        }
        
        private void connectSuccess(object sender, string profileID)
        {
            if(_isLoaded)
                return;

            if (_profileInformation.ProfileID == profileID) {
                mainController.StartProfileForms (_profileInformation, () => {
                });
                //DataProcessing.Instance.EnableQueryHeadset(true);
                //DataProcessing.Instance.SetConnectedHeadset (_profileInformation);
            } else {
                Debug.Log("Another profile connected or wrong somewhere");
            }
        }

        private void connectFailed(object sender, string profileID)
        {
            //DataProcessing.Instance.EnableQueryHeadset(true);
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

        public ProfileElement WithInformation (Profile info)
        {
            this._profileInformation = info;
            SetName (this._profileInformation.ProfileID);
            //SetConnectionType (this._profileInformation.ProfileConnection);
            return this;
        }

        public class Factory:PlaceholderFactory<ProfileElement>
        {
        }
    }
}