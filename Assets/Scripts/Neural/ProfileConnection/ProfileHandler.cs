
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ProfileHandler : MonoBehaviour
    {

        ProfileController   _profileController;
        ConnectionIndicatorGroup   _connectionIndicatorGroup;
        ProfileGroup		       _profileGroup;

        ContactQualityController   _contactQualityController;

        void Start()
        {
            //DataProcessing.Instance.onHeadsetChange      += OnHeadsetChanged;
            //DataProcessing.Instance.onCurrHeadsetRemoved += onCurrHeadsetRemoved;
        }

        [Inject]
        public void InjectDependency(ProfileController profileController, ConnectionIndicatorGroup connectionIndicatorGroup, 
                                     ProfileGroup profileGroup,
                                     ContactQualityController contactQualityController)
        {
            _profileController = profileController;
            _connectionIndicatorGroup = connectionIndicatorGroup;
            _profileGroup             = profileGroup;
            _contactQualityController = contactQualityController;
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
            
            if (DataProcessing.Instance.GetHeadsetList().Count == 0) 
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
            _contactQualityController.Deactivate();
            _connectionIndicatorGroup.Deactivate ();

            _profileController.Refresh ();
            _profileController.Activate ();
            ShowProfile ();
        }
    }
}



