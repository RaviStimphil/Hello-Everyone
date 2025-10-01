using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public float jumpSpeed;
    public static event Action interactPressed;
    public bool preventMove;
    

    //public InputSystem inputSystem;

    private Rigidbody rb;

    public Vector3 moveDirection;

    void OnEnable(){
        DialogueManager.dialogueBegins += DisallowMove;
        DialogueManager.dialogueEnds += AllowMove;
    }
    void OnDisable(){
        DialogueManager.dialogueBegins -= DisallowMove;
        DialogueManager.dialogueEnds -= AllowMove;
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //inputActions = new InputSystem();
    }

    // Update is called once per frame
    void Update()
    {
        if(!preventMove){
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.y);
        }
        //rb.AddForce(moveDirection, ForceMode.Impulse);
        //moveDirection = Vector3.zero;
    }
    public void OnMovement(InputValue value){
        
        moveDirection.x = value.Get<Vector2>().x  * moveSpeed;
        moveDirection.y = value.Get<Vector2>().y  * moveSpeed;
        
    }
    private void OnJump(){
        if(IsGrounded() && !preventMove){
            Debug.Log("TRE");
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }
    }
    private void OnInteract(){
        interactPressed?.Invoke();
    }
    private bool IsGrounded(){
        return true;
    }
    private void DisallowMove(){
        preventMove = true;
    }
    private void AllowMove(){
        preventMove = false;
    }

}
