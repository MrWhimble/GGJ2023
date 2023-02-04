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
        if (firstTimeTalking)
        {
            StartCoroutine(FirstTimeTalking());
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

        firstTimeTalking = false;
    }

    IEnumerator Talking() 
    {
        int randomSentence = Random.Range(0, Dialogue.Count);

        blobDialogue.text = Dialogue[randomSentence].dialogueText;
        yield return new WaitForSeconds(startingDialogue[randomSentence].timeBetweenSentences);
        blobDialogue.text = "";

    }

}
