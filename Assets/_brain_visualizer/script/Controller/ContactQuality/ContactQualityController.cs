using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ContactQualityController : BaseCanvasView
    {
        [SerializeField] private ContactQualityBaseManager insight;
        [SerializeField] private ContactQualityBaseManager epoc;
        [SerializeField] private float updateCQInterval = 0.5f;

        ConnectedDevice connectedDevice;
        HeadsetGroup headsetGroup;
        ContactQualityBaseManager  activeDevice;
        ConnectionIndicatorGroup   connectionIndicatorGroup;

        ProfileGroup _profileGroup;
        ProfileController _profileController;
        //DataSubscriber dataSubscriber;
        //ExamplesBoard examplesBoard;

                
        public Text displayText;

        [Inject]
        public void SetDependencies (ConnectedDevice device, HeadsetGroup headsetGroup,
                                     ConnectionIndicatorGroup connectionIndicatorGroup/*,
                                     ExamplesBoard board*/, ProfileController profileController,
                                     ProfileGroup profileGroup)
        {
            this.connectedDevice  = device;
            this.headsetGroup     = headsetGroup;
            this.connectionIndicatorGroup = connectionIndicatorGroup;
            this._profileGroup = profileGroup;
            this._profileController = profileController;
            // dataSubscriber = subscriber;
            //examplesBoard = board;
        }

        public override void Activate ()
        {
            Debug.Log("ContactQualityController Active");
            // deactive other screen if have
            //examplesBoard.Deactivate();

            headsetGroup.Activate();
            connectionIndicatorGroup.Activate ();

            activeDevice = Utils.IsInsightType(connectedDevice.Information.HeadsetType) ? insight : epoc;
            activeDevice.gameObject.SetActive(true);
            base.Activate ();

            StartCoroutine(RunCoroutineDisplayColor(updateCQInterval));
        }
        
        public override void Deactivate ()
        {
            _profileGroup.Activate();
            _profileController.Activate();
            headsetGroup.Deactivate ();
            base.Deactivate ();

            if (activeDevice != null)
                activeDevice.gameObject.SetActive(false);
            
        }

        public void onButtonDone()
        {
            Deactivate();
        }
            
        public void QuickOpen() {
            Activate();
        }

        public void DisplayContactQualityColor() {
            activeDevice.SetContactQualityColor(DataProcessing.Instance.GetContactQuality());
        }
                    
        IEnumerator RunCoroutineDisplayColor(float timeInteval) {
            while(this.IsActive) {
                DisplayContactQualityColor();
                yield return new WaitForSeconds(timeInteval);
            }
        }
    }
}