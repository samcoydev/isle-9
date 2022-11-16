using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;
    public GameObject loadingScreen;
    public ProgressBar loadingBar;

    private void Awake() {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive);
    }


    public void SelectProfile(int profileNumber) {
        // check if there is a file to load
        LoadGame();
    }

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private void LoadGame() {
        loadingScreen.SetActive(true);
        SceneManager.UnloadSceneAsync((int)SceneIndexes.MAIN_MENU);
        SceneManager.LoadSceneAsync((int)SceneIndexes.GAME, LoadSceneMode.Additive);

        StartCoroutine(GetSceneLoadProgress());
    }


    private float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress() {
        for (int i = 0; i < scenesLoading.Count; i++) {
            while (!scenesLoading[i].isDone) {
                totalSceneProgress = 0;

                foreach(AsyncOperation operation in scenesLoading) {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                loadingBar.current = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
    }
}
