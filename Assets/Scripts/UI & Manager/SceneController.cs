using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *  Scene을 String으로 인해 변하게 할 수 있게 하는 스크립트입니다.
 *  따로 Object를 만들지 않고 사용할 수 있도록, 모든 methods를 static으로 선언합니다.
 */
public class SceneController : MonoBehaviour
{
    private float delay = 3f;
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

    //  딜레이를 주는 함수만 Instance를 만들어야 실행이 가능합니다.
    public void ChangeSceneWithDelay(string sceneName)
    {
        StartCoroutine(ChangeSceneCoroutine(sceneName));
    }
    
    // public void ChangeSceneWithDelayAndNext(string sceneName, string nextName, float time)
    // {
    //     StartCoroutine(ChangeSceneCoroutine(sceneName,time));
    // }

    private IEnumerator ChangeSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
