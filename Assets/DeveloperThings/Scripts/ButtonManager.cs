using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{


    public void FailButton()
    {
        int failedLevelSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(failedLevelSceneIndex);
    }
    public void NextLevelButton()
    {
        if(GameManager.Instance.GetPlayerLevel()>=10)
        {
            GameManager.Instance.IncreaseLevel();
            SceneManager.LoadScene(Random.Range(2,11));
        }
        else
        {
            GameManager.Instance.IncreaseLevel();
            SceneManager.LoadScene(GameManager.Instance.GetPlayerLevel());

        }
        




    }

    

}

