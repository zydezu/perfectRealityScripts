using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class playerMovement : MonoBehaviour
{
    [SerializeField] Vector2 input;
    private Vector2 velocity = Vector2.zero;
    public float speedMultiplier = 3f;
    private float acceleration = 0.6f; // this is how fast you speed up and slow down ( how SLIPPY the player feels to control ), must be below 0
    private bool direction = true;
    public float inputDeadzone;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update() //input is registered every frame
    {
        if (Global.playerMovementActive)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // get input of both axis to form one vector
            if (input.sqrMagnitude > 1f) input.Normalize();

            if (Mathf.Abs(input.x) < inputDeadzone) input.x = 0; //deadzone - to stop drift (for controller sticks)
            if (Mathf.Abs(input.y) < inputDeadzone) input.y = 0; //deadzone - stop drifting on keyboard too ???

            velocity = new Vector2((velocity.x + input.x) * acceleration, (velocity.y + input.y) * acceleration);

            CheckDirection();

            if (Input.GetKeyDown("left shift")) // test events
            {
                GameManager.instance.UpdateGameState(GameState.QuitToTitle);
            }
            if (Input.GetKeyDown("right shift")) // test events
            {
                if (!Global.cameraLocked) Global.cameraLocked = true;
                else Global.cameraLocked = false;
            }
        }
    }

    private void FixedUpdate() //delta
    {
        rb.MovePosition(rb.position + speedMultiplier * Time.fixedDeltaTime * velocity);
    }

    private void CheckDirection()
    {
        if (direction && input.x < 0f || !direction && input.x > 0f)
        {
            direction = !direction;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
