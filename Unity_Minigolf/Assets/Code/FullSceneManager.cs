using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// A class to hanfle changing the Scenes in the intended order
/// </summary>
public class FullSceneManager : MonoBehaviour
{
    // Enum representing all playable Scenes
    public enum sceneEnum
    {
        Start,
        Customize,
        Tutorial,
        Play,
        Feedback,
        End,
    }

    // variables to keep track of current status
    private sceneEnum _currentScene = sceneEnum.Start;
    private sceneEnum _nextScene = sceneEnum.Customize;
    
    public bool skipTutorial = false;
    public KeyCode changeSceneKey;
    public static FullSceneManager sceneManager;
    public bool keySceneChange = true;

    //private GameObject _player;

    public event Action OnSceneChange;

   // dynamically return the current and corresponding next Scene
    public sceneEnum CurrentScene
    {
        get => _currentScene;
        set
        {
            _currentScene = value;
            switch (_currentScene)
            {
                case sceneEnum.Start:
                    _nextScene = sceneEnum.Customize;
                    break;
                case sceneEnum.Customize:
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
                    //Play Mode should only end if Time is run out
                    //TODO: before uncommenting, enable timeout scene switch again
                    //keySceneChange = false;
                    _nextScene = sceneEnum.Feedback;
                    break;
                case sceneEnum.Feedback:
                    keySceneChange = true;
                    _nextScene = sceneEnum.End;
                    break;
                case sceneEnum.End:
                    // if the user chooses to, UI interaction should allow them to play another round
                    keySceneChange = false;
                    _nextScene = sceneEnum.Play;
                    break;
            }
        }
    }

    // public Function to be invokable trough button Presses in scene
    public void ChangeScene()
    {
        
        // Loads next Scene according to Build index, which needs to stay consistent with Enum int
        Debug.Log("Load next scene" + _nextScene + (int)_nextScene);
        SceneManager.LoadScene((int) _nextScene);
        // As same Player is taken through all Scenes, reset the Player to ensure starting each Scene at the correct position
        if (CurrentScene != sceneEnum.Start)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0,2,0);
        }
        CurrentScene = _nextScene;
        Debug.Log(CurrentScene);

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
            // SceneManager gameObject is mandatory for all Scenes
            DontDestroyOnLoad(gameObject);
            sceneManager = this;
        }
    }
    
    // Subscribe and Unsubscribe from the Scene Change Event
    public void OnEnable()
    {
        OnSceneChange += ChangeScene;
    }


    public void Update()
    {
        // if the chosen SceneChangeKey is pressed within any Scene, the Scene Change is initiated
        if (Input.GetKeyDown(changeSceneKey) && keySceneChange)
        {
            OnSceneChange?.Invoke();
        }
    }
    
    public void OnDisable()
    {
        OnSceneChange -= ChangeScene;
    }
}
