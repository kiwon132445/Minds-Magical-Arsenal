using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ProfileController : BaseCanvasView
    {
        [SerializeField] private Transform    profileList;
        [SerializeField] private GameObject   txtNote;
        [SerializeField] private CanvasScaler rootCanvasScaler;
        [SerializeField]
        private Image fadeImage;
        [SerializeField] Image title;
        [SerializeField] Text  loadText;

        ProfileAdapter      _profileAdapter;
        //ConnectedDevice            _connectedDevice;
        //ContactQualityController   _contactQualityController;
        //UI_ConnectingToCortex      _connectingToCortex;
        //ConnectionIndicatorGroup   _connectionIndicatorGroup;
        //ContactQualityBaseManager  activeDevice;
        //HeadsetGroup headsetGroup;

        Color tempColor;
        RectTransform canvasRect;
        
        ConnectToCortexStates _lastState;
        //float _timerCortex_state = 0;
        const float TIME_UPDATE_CORTEX_STATE = 2f;
        //bool _enableChecking = false;

        [Inject]
        public void InjectDependencies (/*UI_ConnectingToCortex connectingToCortex,*/ ProfileGroup profileGroup,
                                        ProfileAdapter adapter // ConnectedDevice connectedDevice,
                                        //ContactQualityController contactQualityController,
                                        //ConnectionIndicatorGroup connectionIndicatorGroup,
                                        /*HeadsetGroup headsetGroup*/)
        {
            _profileAdapter             = adapter;
            //_connectedDevice            = connectedDevice;
            //_contactQualityController   = contactQualityController;
            //_connectingToCortex         = connectingToCortex;
            //_connectionIndicatorGroup   = connectionIndicatorGroup;
            //this.headsetGroup     = headsetGroup;

            init ();
        }

        private void init ()
        {
            this._profileAdapter.onNewItemReceived += addNewProfile;
            this._profileAdapter.onClearItems      += clearProfileAll;
            tempColor = fadeImage.color;
            canvasRect = rootCanvasScaler.GetComponent<RectTransform>();
            if(canvasRect.rect.height< rootCanvasScaler.referenceResolution.y) {
                float ratio = canvasRect.rect.height / rootCanvasScaler.referenceResolution.y;
                float yPos  = title.rectTransform.anchoredPosition.y * ratio;

                yPos = Mathf.Clamp(yPos, loadText.rectTransform.anchoredPosition.y + loadText.rectTransform.rect.height + title.rectTransform.rect.height, title.rectTransform.anchoredPosition.y);
                title.rectTransform.anchoredPosition = new Vector2(title.rectTransform.anchoredPosition.x, yPos);
            }
        }

        public void Refresh () 
        {						
            StartCoroutine(ChangeCanvasScaleMode(FADE_IN_TIME, true));
            tempColor = fadeImage.color;
            canvasRect = rootCanvasScaler.GetComponent<RectTransform>();
            if(canvasRect.rect.height< rootCanvasScaler.referenceResolution.y) {
                float ratio = canvasRect.rect.height / rootCanvasScaler.referenceResolution.y;
                float yPos  = title.rectTransform.anchoredPosition.y * ratio;

                yPos = Mathf.Clamp(yPos, loadText.rectTransform.anchoredPosition.y + loadText.rectTransform.rect.height + title.rectTransform.rect.height, title.rectTransform.anchoredPosition.y);
                title.rectTransform.anchoredPosition = new Vector2(title.rectTransform.anchoredPosition.x, yPos);
            }
        }
            
        private void addNewProfile (ProfileElement newProfile)
        {
            txtNote.SetActive (false);
            profileList.gameObject.SetActive (true);
            newProfile.transform.SetParent (profileList, false);
        }

        private void clearProfileAll (ProfileElement newProfile)
        {
            txtNote.SetActive (true);
            foreach(Transform child in profileList.transform) {
                Destroy(child.gameObject);
            }
            profileList.DetachChildren();
            profileList.gameObject.SetActive (false);
        }

        public override void Deactivate ()
        {
            //headsetGroup.Deactivate ();
            base.Deactivate ();
            StopAllCoroutines();

            /*
            if (activeDevice != null)
                activeDevice.gameObject.SetActive(false);*/
        }

        void showNextForm ()
        {
            Deactivate ();
            //_contactQualityController.Activate ();
            SceneManager.LoadScene("Main");
        }

        public void StartProfileForms(Profile deviceInfo, Action onProgress)
        {
            onProgress.Invoke ();
            StartCoroutine (setProfile (deviceInfo, showNextForm));
        }

        private YieldInstruction timeToWait = new WaitForSeconds (1);

        /*
        IEnumerator setConnect (Profile profileInformation, Action onConnected)
        {
            _connectedDevice.Information = profileInformation;
            yield return null;
            onConnected.Invoke ();
        }
        */
        IEnumerator setProfile (Profile profileInformation, Action onConnected)
        {
            //_connectedDevice.Information = profileInformation;
            yield return null;
            onConnected.Invoke ();
        }
        public void AddAvailableProfile(Profile profileInfo) {
            // Debug.Log("AddAvailableDevice");
            _profileAdapter.AddProfile(profileInfo);
        }

        public void ClearProfileList() {
            _profileAdapter.ClearProfileList();
        }

        public override void Activate()
        {
            //_enableChecking = true;
            base.Activate();
        }

        IEnumerator ChangeCanvasScaleMode(float delayTime, bool isProfileListForm)
        {
            if (isProfileListForm) {
                rootCanvasScaler.uiScaleMode     = CanvasScaler.ScaleMode.ConstantPixelSize;
            } else {
                rootCanvasScaler.uiScaleMode     = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            }

            rootCanvasScaler.referenceResolution = new Vector2(1024, 768);
            rootCanvasScaler.screenMatchMode     = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            rootCanvasScaler.matchWidthOrHeight  = 0.5f;
            tempColor.a = 0f;
            yield return new WaitForSeconds(delayTime);
            fadeImage.DOColor(tempColor, delayTime);
        }
        
        /*
        bool updateCortexStates ()
        {
            if (!_enableChecking)
                return _enableChecking;

            _timerCortex_state += Time.deltaTime;
            if (_timerCortex_state < TIME_UPDATE_CORTEX_STATE)
                return _enableChecking;

            _timerCortex_state -= TIME_UPDATE_CORTEX_STATE;
            var curState = DataStreamManager.Instance.GetConnectToCortexState();

            if (_lastState == curState)
                return _enableChecking;

            _lastState = curState;
            switch (curState) {
                case ConnectToCortexStates.Service_connecting:
                case ConnectToCortexStates.EmotivApp_NotFound:
                case ConnectToCortexStates.Login_waiting:
                case ConnectToCortexStates.Login_notYet:
                case ConnectToCortexStates.Authorizing:
                case ConnectToCortexStates.Authorize_failed:
                case ConnectToCortexStates.LicenseExpried:
                case ConnectToCortexStates.License_HardLimited: {
                    _enableChecking = false;
                    _connectionIndicatorGroup.Deactivate ();
                    _connectingToCortex.Activate();

                    break;
                }
                case ConnectToCortexStates.Authorized:
                    break;
            }

            return _enableChecking;
        }
        */
    }
}