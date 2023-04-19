using UnityEngine;
using Zenject;
using dirox.emotiv.controller;

namespace dirox.emotiv
{
    public class GameControllerInstaller : MonoInstaller<GameControllerInstaller>
    {
        [Header ("Cortex")]
        [SerializeField] UI_ConnectingToCortex connectingToCortex;
        [SerializeField] UI_InstallEmotivApp   installEmotivApp;
        [SerializeField] UI_LoginViaEmotivApp  loginViaEmotivApp;
        [SerializeField] UI_TrialExpired       trialExpried;
        [SerializeField] UI_OfflineUseLimit    offlineUseLimit;

        [Header ("Connect Headset")]
        [SerializeField] HeadsetGroup headsetGroup;
        [SerializeField] ConnectHeadsetController connectHeadsetController;
        [SerializeField] ConnectHeadsetElement headsetElementPrefab;

        [Header ("Contact Quality")]
        [SerializeField] ContactQualityController contactQualityController;
        [SerializeField] ContactQualityColorSetting contactQualityColorSetting;

        [Header ("Connection Quality Indicator")]
        [SerializeField] ConnectionIndicatorGroup connectionIndicatorGroup;
        [SerializeField] ConnectionQualityIndicator connectionQualityIndicator;
        [SerializeField] BatteryIndicator batteryIndicator;

        [Header ("Profile Loader")]
        [SerializeField] ProfileGroup profileGroup;
        [SerializeField] ProfileController profileController;
        [SerializeField] ProfileElement profileElementPrefab;


        // [Header ("Data Subscriber")]
        // [SerializeField] DataSubscriber dataSubscriber;

        // [Header ("Examples Board")]
        // [SerializeField] ExamplesBoard examplesBoard;

        public override void InstallBindings ()
        {
            bindConnectHeadset ();
            bindContactQuality ();
            bindIndicators ();
            bindCortexGroup();
            bindProfileLoader();
        }

        private void bindConnectHeadset ()
        {
            Container.BindInstance (headsetGroup);
            Container.BindInstance (connectHeadsetController);
            Container.BindFactory<ConnectHeadsetElement, ConnectHeadsetElement.Factory> ().FromComponentInNewPrefab (headsetElementPrefab);
            Container.Bind<ConnectHeadsetAdapter> ().FromNew ().AsSingle ();
        }

        private void bindContactQuality ()
        {
            var connectedDevice = new ConnectedDevice ();
            Container.BindInstance (contactQualityController);
            Container.BindInstance (connectedDevice);
            bindContactQualityNodes ();
        }

        private void bindProfileLoader ()
        {
            Container.BindInstance (profileGroup);
            Container.BindInstance (profileController);
            Container.BindFactory<ProfileElement, ProfileElement.Factory> ().FromComponentInNewPrefab (profileElementPrefab);
            Container.Bind<ProfileAdapter> ().FromNew ().AsSingle ();
        }
            
        /// <summary>
        /// To use any of this, do like this: [Inject(Id = "AF3")] ContactQualityNodeView AF3;
        /// </summary>
        private void bindContactQualityNodes ()
        {
            Container.Bind<ContactQualityNodeView> ().WithId ("AF3").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("AF4").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("CMS").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("DRL").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("F3").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("F4").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("F7").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("F8").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("FC5").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("FC6").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("O1").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("O2").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("P7").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("P8").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("PZ").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("T7").FromNew ().AsCached  ();
            Container.Bind<ContactQualityNodeView> ().WithId ("T8").FromNew ().AsCached  ();
            Container.BindInstance (contactQualityColorSetting);
        }

        private void bindIndicators ()
        {
            Container.BindInstance (connectionIndicatorGroup);
            Container.BindInstance (connectionQualityIndicator);
            Container.BindInstance (batteryIndicator);
        }

        private void bindCortexGroup()
        {
            Container.BindInstance (connectingToCortex);
            Container.BindInstance (installEmotivApp);
            Container.BindInstance (loginViaEmotivApp);
            Container.BindInstance (trialExpried);
            Container.BindInstance (offlineUseLimit);
        }
    }
}