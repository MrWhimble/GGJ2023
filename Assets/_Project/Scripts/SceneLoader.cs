using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadShopScene() 
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadGameScene() 
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
