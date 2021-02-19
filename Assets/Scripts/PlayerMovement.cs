using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minJumpHeight = 1f;
    [SerializeField] private float maxJumpHeight = 2f;
    [SerializeField] private float jumpSpeed = 5f;

    private Vector2 moveInput;

    private enum JumpState {
        RESTING,
        ASCENT,
        DESCENT
    }
    private JumpState jumpState;
    private bool jumpWasPressed;
    private bool jumpPressed;

    private float currentJumpHeight;
    private float acceleration;

    private void FixedUpdate() {
        HandleJump();

        Vector2 vel = new Vector2();
        vel.x = moveInput.x * moveSpeed;

        vel.y = moveInput.y * moveSpeed;
        vel.y += GetJumpVector() * jumpSpeed;

        rb.velocity = vel;
    }

    private void HandleJump() {
        // Figure out what state we're in. This is a bit wacky, a proper state machine is a good end goal here.
        // TODO: handle jump states with an actual state machine lol.
        if (jumpWasPressed == false && jumpPressed == true && jumpState == JumpState.RESTING) {
            // Jump.
            jumpState = JumpState.ASCENT;
        } else if (jumpWasPressed == true && jumpPressed == false && jumpState == JumpState.ASCENT) {
            // Release jump early.
            jumpState = JumpState.DESCENT;
        } else if (currentJumpHeight >= maxJumpHeight && jumpState == JumpState.ASCENT) {
            // Start descent from jump apex.
            jumpState = JumpState.DESCENT;
        } else if (currentJumpHeight <= 0 && jumpState == JumpState.DESCENT) {
            // End jump, hit da ground.
            // TODO: check that we're actually on the ground. We could be falling lol.
            jumpState = JumpState.RESTING;
        }

        // Set current jump height based on state.
        if (jumpState == JumpState.ASCENT) {
            currentJumpHeight += jumpSpeed * Time.deltaTime;
        } else if (jumpState == JumpState.DESCENT) {
            currentJumpHeight -= jumpSpeed * Time.deltaTime;
        }
    }

    private float GetJumpVector() {
        switch (jumpState) {
            case JumpState.ASCENT:
                return 1f;
            case JumpState.DESCENT:
                return -1f;
            case JumpState.RESTING:
            default:
                return 0f;
        }
    }

    public void OnMove(InputValue input) {
        Debug.Log("Movement key pressed.");

        moveInput = input.Get<Vector2>();
    }

    public void OnJumpPressed() {
        Debug.Log("Jump Pressed");

        jumpWasPressed = jumpPressed;
        jumpPressed = true;
    }

    public void OnJumpReleased() {
        Debug.Log("Jump Released");

        jumpWasPressed = jumpPressed;
        jumpPressed = false;
    }
}
