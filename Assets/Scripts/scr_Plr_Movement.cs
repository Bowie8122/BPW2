using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Plr_Movement : MonoBehaviour
{
    public float movespeed;
    public Rigidbody2D Rigid2D;

    private Vector2 MoveDirection;
    
    // Update is called once per frame
    void Update()
    {
        Inputs();        
    }
    void FixedUpdate()
    {
        Movement();   
    }

    void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        MoveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Movement()
    {
        Rigid2D.velocity = new Vector2(MoveDirection.x * movespeed, MoveDirection.y * movespeed);
    }
}
