using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    private Fade fadeSc;

    void Awake()
    {
        fadeSc = GetComponent<Fade>();
    }

    void Start()
    {
        fadeSc.FadeOut();
    }

    public void OnClickStartBtn()
    {
        fadeSc.FadeIn();
        Invoke("LoadGameScene", 1f);
    }

    public void OnClickExitBtn()
    {
        fadeSc.FadeIn();
        Invoke("ExitGame", 1.5f);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
