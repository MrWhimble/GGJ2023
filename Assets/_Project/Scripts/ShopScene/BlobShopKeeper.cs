using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlobShopKeeper : MonoBehaviour
{
    public TMP_Text blobDialogue;
    public bool firstTimeTalking = true;

    public List<startingDialogueStruct> startingDialogue;
    public List<startingDialogueStruct> Dialogue;

    public GameObject playButton;

    [System.Serializable]
    public struct startingDialogueStruct 
    {
        public string dialogueText;
        public float timeBetweenSentences;

    }

    [System.Serializable]
    public struct DialogueStruct
    {
        public string dialogueText;
        public float timeBetweenSentences;

    }


    public

    // Start is called before the first frame update
    void Start()
    {
        int firstTime = PlayerPrefs.GetInt("firstTime");

        if (firstTimeTalking && firstTime != 1)
        {
            StartCoroutine(FirstTimeTalking());
            playButton.SetActive(false);
        }
        else
        {
            StartCoroutine(Talking());
        }

        
        
    }

    IEnumerator FirstTimeTalking() 
    {
        for (int i = 0; i < startingDialogue.Count; i++)
        {
            blobDialogue.text = startingDialogue[i].dialogueText;
            yield return new WaitForSeconds(startingDialogue[i].timeBetweenSentences);
        }

        PlayerPrefs.SetInt("firstTime", 1);
        firstTimeTalking = false;
        playButton.SetActive(true);
    }

    IEnumerator Talking() 
    {
        int randomSentence = Random.Range(0, Dialogue.Count);

        blobDialogue.text = Dialogue[randomSentence].dialogueText;
        yield return new WaitForSeconds(startingDialogue[randomSentence].timeBetweenSentences);
        blobDialogue.text = "";

    }

    public void ClearPlayerPrefs() 
    {
        PlayerPrefs.DeleteAll();
    }

}
