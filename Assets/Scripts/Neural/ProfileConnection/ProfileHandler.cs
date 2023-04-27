
using UnityEngine;
using System;
using Zenject;
using EmotivUnityPlugin;
using System.Collections.Generic;

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
            TrainingProcessing.Instance.onCurrProfileChanged += onCurrProfileChanged;
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
        public void InjectDependency(ProfileController profileController, ProfileGroup profileGroup)
        {
            _profileController = profileController;
            _profileGroup             = profileGroup;
        }
        
        private void OnProfileChanged(object sender, EventArgs args)
        {
            ShowProfile();
        }

        private void onCurrProfileChanged(object sender, EventArgs args)
        {
            ShowProfileListForm();
        }

        private void ShowProfile()
        {
            //_profileController.Refresh ();
            _profileController.ClearProfileList ();
            
            if (TrainingProcessing.Instance.GetProfileList().Count == 0) 
                return;
            foreach (KeyValuePair<string, Profile> item in TrainingProcessing.Instance.GetProfileList())
            {
                if (item.Key == "")
                    continue;
                
                _profileController.AddAvailableProfile(new Profile(item.Key));
            }
        }

        private void ShowProfileListForm()
        {
            //_profileController.Refresh ();
            _profileController.Activate ();
            ShowProfile ();
        }
    }
}



