using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalSettings : Singleton<GlobalSettings> {

    public Dictionary<string, bool> flags { get; private set;}

    public enum AvailableFlags {
        DEBUG,
        MULTIPLE_LOCAL_PLAYERS
    };

    GlobalSettings() {
        flags = new Dictionary<string, bool>();
        flags.Add(AvailableFlags.DEBUG.ToString(), false);
        flags.Add(AvailableFlags.MULTIPLE_LOCAL_PLAYERS.ToString(), false);
    }


    public void Update()
    {
        Debug.Log("I'm not a fan of C#");
    }
}
