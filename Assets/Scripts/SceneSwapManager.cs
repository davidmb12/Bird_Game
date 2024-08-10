using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.Instance.StartFadeIn();
    }
    public static void SwapScene(SceneField n_scene)
    {
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(n_scene));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField n_scene)
    {
        SceneFadeManager.Instance.StartFadeOut();
        while (SceneFadeManager.Instance.IsFadingOut)
        {
            yield return null;
        }
        SceneManager.LoadScene(n_scene);
    }
}
