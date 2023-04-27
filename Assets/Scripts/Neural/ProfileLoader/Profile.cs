using System.Collections.Generic;
using System;

using EmotivUnityPlugin;

namespace dirox.emotiv.controller
{
    public class Profile
    {
      public Profile(string profileID) {
        this.ProfileID = profileID;
      }
      
      public string ProfileID {get; set;}
    }
}