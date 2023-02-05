using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public GameObject fadeScreen;

    private void Start()
    {
        StartCoroutine(LoadCurrentScene());
        
    }

    IEnumerator LoadCurrentScene()
    {
        fadeScreen.gameObject.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1.2f);
        fadeScreen.SetActive(false);
    }

    public void LoadShopScene()
    {
        StartCoroutine(LoadShop());
    }

    IEnumerator LoadShop() 
    {
        fadeScreen.SetActive(true);
        fadeScreen.gameObject.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Shop");
    }

    public void LoadGameScene() 
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        fadeScreen.SetActive(true);
        fadeScreen.gameObject.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Game");
    }

    public void MainMenuScene() 
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        fadeScreen.SetActive(true);
        fadeScreen.gameObject.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("MainMenu");
    }

}
