using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScenes
{
    MainMenu, Loading, SampleScene
}
public class Loader : MonoBehaviour
{
    private static GameScenes targetScene;


    public static void Load(GameScenes target)
    {
        Loader.targetScene = target;

        SceneManager.LoadScene(GameScenes.Loading.ToString());
    }
    public static IEnumerator LoaderCallback()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(targetScene.ToString());
    }
}
