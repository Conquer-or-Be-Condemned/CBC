using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //  원하는 씬으로 갈 때 이 함수를 호출
    public static void ChangeScene(string sceneName)
    {
        Debug.Log("Go to " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public static void ExitProgram()
    {
        Application.Quit();
    }
}
