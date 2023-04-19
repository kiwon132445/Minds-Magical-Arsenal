using System.Collections.Generic;
using System;
using UnityEngine;

//Primarily handles the ProfileElement Prefab implementations
namespace dirox.emotiv.controller
{
    public class ProfileAdapter
    {
        private List<Profile> profileList;
        private ProfileElement.Factory factory;

        public event Action<ProfileElement> onNewItemReceived;
        public event Action<ProfileElement> onRemoveItem;
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
                onNewItemReceived.Invoke (factory.Create ().WithInformationAdd (profile));
        }

        public void RemoveProfile (ProfileElement profile)
        {
            int count = 0;
            string profileName = profile.GetName();
            foreach (Profile p in profileList)
            {
                if (p.ProfileID == profile.GetName())
                {
                    profileList.RemoveAt(count);
                    if (onRemoveItem != null)
                        onRemoveItem.Invoke(profile);
                    // profile.DestroySelf();
                    return;
                }
                count++;
            }
        }

        public void ClearProfileList ()
        {
            profileList.Clear();
            if (onClearItems != null)
                onClearItems.Invoke (null);
        }
    }
}