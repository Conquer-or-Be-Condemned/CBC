using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 *  원하는 웹사이트를 띄우는 스크립트입니다.
 */
public class SiteManager : MonoBehaviour
{
    public static void OpenGithub()
    {
        Application.OpenURL("https://github.com/Conquer-or-Be-Condemned/CBC");
    }

    public static void OpenAnySite(string url)
    {
        Application.OpenURL(url);
    }
}
