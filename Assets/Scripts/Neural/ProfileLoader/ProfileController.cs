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
        [SerializeField] private Image fadeImage;
        [SerializeField] Image title;
        [SerializeField] Text  loadText;
        [SerializeField] private GameObject profileInputField;
        [SerializeField] private GameObject eventSystem;

        ProfileGroup profileGroup;
        ProfileAdapter     _profileAdapter;
        ConnectedDevice    _connectedDevice;
        TrainingProcessing _trainingProcessing = TrainingProcessing.Instance;

        Color tempColor;
        RectTransform canvasRect;
        
        ConnectToCortexStates _lastState;
        //float _timerCortex_state = 0;
        const float TIME_UPDATE_CORTEX_STATE = 2f;
        //bool _enableChecking = false;

        [Inject]
        public void InjectDependencies (ProfileGroup group, ProfileAdapter adapter,
         ConnectedDevice connectedDevice)
        {
            _profileAdapter = adapter;
            _connectedDevice = connectedDevice;
            profileGroup = group;
            init ();
        }

        private void init ()
        {
            this._profileAdapter.onNewItemReceived += addNewProfile;
            this._profileAdapter.onRemoveItem      += removeProfile;
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
        private void OnDestroy()
        {
            //StartCoroutine (saveProfile());
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

        public void RefreshProfiles()
        {
            TrainingProcessing.Instance.Process();
        }
            
        private void addNewProfile (ProfileElement newProfile)
        {
            txtNote.SetActive (false);
            profileList.gameObject.SetActive (true);
            newProfile.transform.SetParent (profileList, false);
        }

        
        private void removeProfile (ProfileElement profile)
        {
            Destroy(profile.gameObject);
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
            profileGroup.Deactivate();
            eventSystem.SetActive(false);
            base.Deactivate();
            StopAllCoroutines();
            TrainingProcessing.Instance.EnableQueryProfile(false);
        }

        void showNextForm ()
        {
            Deactivate();
            //_contactQualityController.Activate ();
            SceneManager.LoadScene("Main");
        }

        public void StartProfileForms(Profile profile)
        {
            //onProgress.Invoke ();
            StartCoroutine (setProfile (profile, showNextForm));
        }

        public void StartProfileDelete(ProfileElement profile)
        {
            //onProgress.Invoke ();
            StartCoroutine (deleteProfile (profile, RefreshProfiles));
        }

        public void StartSaveProfile()
        {
            StartCoroutine (saveProfile ());
        }

        private YieldInstruction timeToWait = new WaitForSeconds (1);

        IEnumerator setProfile (Profile profileInformation, Action onConnected)
        {
            _trainingProcessing.StaticHeadset = _connectedDevice.Information;
            Debug.Log("Loading Profile: " + profileInformation.ProfileID + " into headset: " + _connectedDevice.Information.HeadsetID);
            TrainingProcessing.Instance.LoadProfileWithHeadset(profileInformation.ProfileID, _connectedDevice.Information.HeadsetID);
            yield return null;
            onConnected.Invoke ();
        }

        IEnumerator deleteProfile (ProfileElement profileInformation, Action onRemoved)
        {
            Debug.Log("Delete profile: " + profileInformation.GetName());
            TrainingProcessing.Instance.DeleteProfile(profileInformation.GetName());
            _profileAdapter.RemoveProfile(profileInformation);
            yield return null;
            onRemoved.Invoke ();
        }

        IEnumerator saveProfile ()
        {
            TrainingProcessing.Instance.SaveCurProfile(_connectedDevice.Information.HeadsetID);
            yield return null;
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
            base.Activate();
            //Refresh();
            eventSystem.SetActive(true);
            TrainingProcessing.Instance.EnableQueryProfile(true);
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
    }
}