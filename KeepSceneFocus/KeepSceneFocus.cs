#if UNITY_EDITOR
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class KeepSceneFocus : EditorWindow
{
    public static bool KSFEnable;
    public static bool VRCSDKBypass;
    private bool Activated;


    //Create a tab at the top of Unity
    [MenuItem("KSF/Keep Scene Focus")]
    public static void Init()
    { 
        //When tab button clicked, launch Window
        GetWindow<KeepSceneFocus>("Keep Scene Focus");
    }


    //KSF Window:
    void OnGUI()
    {
        GUILayout.Label("Public Build v1.0");
        GUILayout.Space(40);


        GUILayout.Label("Press to activate script");
        //Button that creates the GameObject and checks for a duplicate
        if (GUILayout.Button("Create GameObject"))
        {
            if (GameObject.Find("Keep Scene Focus") == null)
            {
                GameObject KSF = new GameObject("Keep Scene Focus");
                KSF.AddComponent<KeepSceneFocusWindow>();
                //Default Toggled ON
                (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().KSFEnable) = true;
                (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().VRCSDKBypass) = true;
                Activated = true;
            }
            else
            {
                //GameObject already exists
                GUILayout.Space(10);
                Debug.Log("<color=#3366ff>KSF: </color><color=orange>GameObject already exists.</color>");
            }
        }
        GUILayout.Space(50);


        if (Activated == true && !GameObject.Find("Keep Scene Focus")) //If the GameObject gets renamed or deleted, Bypass GUI window settings to avoid errors
        {
            Activated = false;
            Debug.Log("<color=#3366ff>KSF:</color> <color=orange>GameObject was renamed or deleted? Ignore this if intentional.</color>");
        }

        //Only proceed if button was pressed or GameObject already exists
        if (Activated == true || GameObject.Find("Keep Scene Focus"))
        {
            //Update variables from GameObject
            KSFEnable = (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().KSFEnable);
            VRCSDKBypass = (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().VRCSDKBypass);
            //Update variables from GameObject


            //Toggles
            GUILayout.Label("Settings:");
            GUILayout.Space(10);

            GUILayout.Label("KSF Toggle");
            KSFEnable = EditorGUILayout.Toggle(" Enable", KSFEnable);
            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(KSFEnable == false); // If KSF is disabled, Bypass button is untouchable
            GUILayout.Label("       VRC SDK Bypass");
            VRCSDKBypass = EditorGUILayout.Toggle("         Enable", VRCSDKBypass);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(25);


            //Update variables to GameObject
            (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().KSFEnable) = KSFEnable;
            (GameObject.Find("Keep Scene Focus").GetComponent<KeepSceneFocusWindow>().VRCSDKBypass) = VRCSDKBypass;
            //Update variables to GameObject
        }
    }
}
#endif

//GameObject Script
public class KeepSceneFocusWindow : MonoBehaviour
{
    public bool KSFEnable;
    public bool VRCSDKBypass;

    private bool DoNotLoop = false;


    void LateUpdate()
    {
        //Check if Script is enabled by user, if the editor is active and if it's the first loop
        if (KSFEnable && Application.isEditor && !DoNotLoop)
        {
            //Debug.Log("<color=#3366ff>KSF: KSFEnable Check.</color>");
            DoNotLoop = true;

            //Check if Bypass is enabled and if VRCSDK exists in the hierarchy (aka: "Build & Publish" was pressed)
            if (VRCSDKBypass && (GameObject.Find("VRCSDK") != null))
            {
                //If VRCSDK exists and Bypass is enabled, then bypass
                Debug.Log("<color=#3366ff>KSF:</color> Bypassed.");
            }
            else
            {
                //If either Bypass is disabled or VRCSDK does not exists, then Keep Scene view focused
                #if UNITY_EDITOR
                UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
                Debug.Log("<color=#3366ff>KSF:</color> Completed.");
                #endif
            }
        }
    }
}
#endif