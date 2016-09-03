using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManagerHelper {

    public SceneManagerHelper()
    {
        scenes = new List<string>();
        ReadNames();
    }

    public List<string> scenes { get; private set; }

    private void ReadNames()
    {

        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                scenes.Add(name);
            }
        }
    }
}
