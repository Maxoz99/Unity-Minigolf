using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FullSceneManager : MonoBehaviour
{
    public enum sceneEnum
    {
        //Customize,
        Start,
        Tutorial,
        Play,
        End,
    }

    //private sceneEnum _currentScene = sceneEnum.Customize;
    //private sceneEnum _nextScene = sceneEnum.Start;
    private sceneEnum _currentScene = sceneEnum.Start;
    private sceneEnum _nextScene = sceneEnum.Tutorial;
    
    public bool skipTutorial = false;
    public KeyCode changeSceneKey;
    public static FullSceneManager sceneManager;
    public KeyDownEvent _keyDown = KeyDownEvent.GetPooled(char "x", KeyCode keyCode, EventModifiers modifiers);

    public event Action OnSceneChange;

    public sceneEnum CurrentScene
    {
        get => _currentScene;
        set
        {
            switch (_currentScene)
            {
                //case sceneEnum.Customize:
                //    _nextScene = sceneEnum.Start;
                //    break;
                case sceneEnum.Start:
                    if (!skipTutorial) _nextScene = sceneEnum.Tutorial;
                    else
                    {
                        _nextScene = sceneEnum.Play;
                    }
                    break;
                case sceneEnum.Tutorial:
                    _nextScene = sceneEnum.Play;
                    break;
                case sceneEnum.Play:
                    _nextScene = sceneEnum.End;
                    break;
            }
        }
    }

    private void ChangeScene()
    {
        //Loads next Scene according to Build index, which needs to stay consistent with Enum int
        Debug.Log("Load next scene" + _nextScene);
        SceneManager.LoadScene((int) _nextScene);
    }

    public void Awake()
    {
        // Destroy any other existing sceneManagers
        if (sceneManager != null && sceneManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            sceneManager = this;
        }
    }
    

    public void OnEnable()
    {
        _keyDown += ChangeScene();
        OnSceneChange += ChangeScene;
    }

    public void OnDisable()
    {
        OnSceneChange -= ChangeScene;
    }
}
