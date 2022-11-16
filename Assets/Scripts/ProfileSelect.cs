using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelect : MonoBehaviour {

    public SceneStateManager sceneManager;

    public void Awake() {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneStateManager>();
    }

    public void SelectProfile(int profileNumber) {
        sceneManager.SelectProfile(profileNumber);
    }

}
