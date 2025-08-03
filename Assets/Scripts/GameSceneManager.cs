using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    public enum Scene
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Ending,
        Loading
    }

    private static Action onSceneCallback;

    public static void ChangeScene(Scene scene)
    {
        TransitionManager.Instance.StartTransition();

        FunctionTimer.Create(() =>
        {
            onSceneCallback = () =>
            {
                SceneManager.LoadScene(scene.ToString());
            };

            SceneManager.LoadScene(Scene.Loading.ToString());
        }, 1f);
    }

    public static void ReloadScene()
    {
        TransitionManager.Instance.StartTransition();

        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        FunctionTimer.Create(() =>
        {
            onSceneCallback = () =>
            {
                SceneManager.LoadScene(buildIndex);
            };

            SceneManager.LoadScene(Scene.Loading.ToString());
        }, 1f);
    }

    public static void NextScene()
    {
        TransitionManager.Instance.StartTransition();

        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        FunctionTimer.Create(() =>
        {
            onSceneCallback = () =>
            {
                SceneManager.LoadScene(buildIndex);
            };

            SceneManager.LoadScene(Scene.Loading.ToString());
        }, 1f);
    }

    public static void SceneCallback()
    {
        if (onSceneCallback != null)
        {
            onSceneCallback();
            onSceneCallback = null;
        }
    }
}
