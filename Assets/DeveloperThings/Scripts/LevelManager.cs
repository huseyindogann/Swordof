using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using TMPro;


public class LevelManager : MonoBehaviour
{
    private int lastLevel = 0;

    // [SerializeField] private GameObject loaderCanvas;
    // [SerializeField] private Image loadProgressBar;


    private void Start()
    {
        loadLastScene();
    }
    public async void LoadScene(int sceneIndex)
    {
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;
        // loaderCanvas.SetActive(true);

        // do
        // {

        //     loadProgressBar.fillAmount = scene.progress;
        //     await Task.Yield();

        // } while (scene.progress < 0.9f);
        // await Task.Yield();
        scene.allowSceneActivation = true;
        // loaderCanvas.SetActive(false);

    }

    private void loadLastScene()
    {
        lastLevel = GameManager.Instance.GetPlayerLevel();

        if (lastLevel <= 10) SceneManager.LoadScene(lastLevel);
        else
        {
            //SceneManager.LoadScene(GameManager.Instance.GetLastSceneIndex());
            SceneManager.LoadScene(UnityEngine.Random.Range(2, 11));

        } 
        

    }



}
