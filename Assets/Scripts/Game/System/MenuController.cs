using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //this makes us load game scene
    public void OnPlay() {
        SceneManager.LoadScene("Main");
        //whenever you make games for any platform, you must
        //add the scenes to the build settings

        //in editor, the lighting might be built weirdly/
        //make sure to disable the auto checkbox and hit 'build' in lighting under scene.
    }

    void Update() {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("enter") || Input.GetKey("return")) {
            SceneManager.LoadScene("Main");
        }
    }
}
