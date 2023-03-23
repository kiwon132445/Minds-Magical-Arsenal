using System.Collections.Generic;
using System;

using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ProfileAdapter
    {
        BCITraining _bciTraining = BCITraining.Instance;
        private List<Profile> profileList;
        private ProfileElement.Factory factory;

        public event Action<ProfileElement> onNewItemReceived;
        public event Action<ProfileElement> onClearItems;

        public ProfileAdapter (ProfileElement.Factory factory)
        {
            this.factory = factory;
            profileList = new List<Profile> ();
        }

        public void AddProfile (Profile profile)
        {
            profileList.Add (profile);
            if (onNewItemReceived != null)
                onNewItemReceived.Invoke (factory.Create ().WithInformation (profile));
        }

        public void ClearProfileList ()
        {
            profileList.Clear();
            if (onClearItems != null)
                onClearItems.Invoke (null);
        }
    }
}