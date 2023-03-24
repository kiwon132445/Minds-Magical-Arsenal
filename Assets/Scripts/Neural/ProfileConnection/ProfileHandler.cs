
using UnityEngine;
using System;
using Zenject;
using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ProfileHandler : MonoBehaviour
    {

        ProfileController   _profileController;
        ProfileGroup		       _profileGroup;
        float _timerCounter_queryProfile = 0;
        const float TIME_QUERY_Profile      = 2.0f;

        void Start()
        {
            TrainingProcessing.Instance.onProfileChange      += OnProfileChanged;
            TrainingProcessing.Instance.onCurrProfileRemoved += onCurrProfileUnloaded;
        }

        private void Update()
        {
            _timerCounter_queryProfile += Time.deltaTime;
            if (_timerCounter_queryProfile > TIME_QUERY_Profile) {
                _timerCounter_queryProfile -= TIME_QUERY_Profile;
                TrainingProcessing.Instance.Process();
            }
        }

        [Inject]
        public void InjectDependency(ProfileController profileController, //ConnectionIndicatorGroup connectionIndicatorGroup, 
                                     ProfileGroup profileGroup)
                                     //ContactQualityController contactQualityController)
        {
            _profileController = profileController;
            //_connectionIndicatorGroup = connectionIndicatorGroup;
            _profileGroup             = profileGroup;
            //_contactQualityController = contactQualityController;
        }
        
        private void OnProfileChanged(object sender, EventArgs args)
        {
            ShowProfile();
        }

        private void onCurrProfileUnloaded(object sender, EventArgs args)
        {
            ShowProfileListForm();
        }

        private void ShowProfile()
        {
            _profileController.ClearProfileList ();
            
            if (BCITraining.Instance.ProfileLists.Count == 0) 
                return;

            foreach (string item in BCITraining.Instance.ProfileLists)
            {
                if (item == "")
                    continue;
                
                _profileController.AddAvailableProfile(new Profile(item));
            }
        }

        private void ShowProfileListForm()
        {
            //_contactQualityController.Deactivate();
           // _connectionIndicatorGroup.Deactivate ();

            _profileController.Refresh ();
            _profileController.Activate ();
            ShowProfile ();
        }
    }
}



