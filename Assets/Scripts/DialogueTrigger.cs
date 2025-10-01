using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using Ink.Runtime;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject visualCue;

    private bool playerInRange;

    public TextAsset inkJSON;

    public static event Action<TextAsset, GameObject> messageSent;
    public static event Action<GameObject> messageCancel;



    private void Awake(){
        playerInRange = false;
        visualCue.SetActive(false);
        Debug.Log(inkJSON);
    }  

    private void OnTriggerEnter(Collider collider){
        if(collider.gameObject.GetComponent("PlayerController") as PlayerController){
            visualCue.SetActive(true);
            messageSent?.Invoke(inkJSON, this.gameObject);
        }
    }
    private void OnTriggerExit(Collider collider){
        if(collider.gameObject.GetComponent("PlayerController") as PlayerController){
            visualCue.SetActive(false);
            messageCancel?.Invoke(this.gameObject);
        }
    }
}
