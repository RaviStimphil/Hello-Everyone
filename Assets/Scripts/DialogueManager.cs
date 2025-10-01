using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;  

    private Story currentStory;

    public bool dialogueIsPlaying;

    public GameObject inkJSONSource; 
    public TextAsset inkJSON;
    public static event Action dialogueBegins;
    public static event Action dialogueEnds;

    public GameObject[] choices;
    


    private TextMeshProUGUI[] choicesText;

    void OnEnable(){
        DialogueTrigger.messageSent += UpdateJSON;
        DialogueTrigger.messageCancel += ClearOutJSON;
        PlayerController.interactPressed += StoryControl;
    }
    void OnDisable(){
        DialogueTrigger.messageSent -= UpdateJSON;
        DialogueTrigger.messageCancel -= ClearOutJSON;
        PlayerController.interactPressed -= StoryControl;
    }
    void Awake(){
        inkJSONSource = new GameObject();
        inkJSON = new TextAsset();
        dialogueIsPlaying = false;
        //dialoguePanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices){
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StoryControl(){
        if(!dialogueIsPlaying){
            EnterDialogueMode();
        }  else{
            ContinueStory();
        }
    }
    public void EnterDialogueMode(){
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueBegins?.Invoke();
        ContinueStory();
    }
    public void ExitDialogueMode(){
        dialogueEnds?.Invoke();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    public void ContinueStory(){
        if(currentStory.canContinue){
            dialogueText.text = currentStory.Continue();
            
        }else{
            ExitDialogueMode();
        }
        DisplayChoices();
    }

    public void UpdateJSON(TextAsset doc, GameObject source){
        inkJSONSource = source;
        inkJSON = doc;
    }
    public void ClearOutJSON(GameObject source){
        if(source == inkJSONSource){
            inkJSONSource = null;
            inkJSON = null;
        }
    }
    private void DisplayChoices(){
        List<Choice> currentChoices = currentStory.currentChoices;
        if(choices == null){
            Debug.LogError("Current choices are null");
        }
        if(currentChoices.Count > choices.Length){
            Debug.LogError("TOO MANY CHOICES");
        }

        int index = 0;
        foreach(Choice choice in currentChoices){
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for(int i = index; i < choices.Length; i++){
            choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex){
        
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        
    }
}
