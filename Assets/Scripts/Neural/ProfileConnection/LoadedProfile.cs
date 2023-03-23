
using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class ConnectedProfile
    {
        private Profile _information;

        public Profile Information { 
            get{ return _information; }
            set {
                _information = value;
                if (onProfileSelected != null)
                    onProfileSelected.Invoke (_information);
            }
        }

        public event System.Action<Profile> onProfileSelected;
    }
}