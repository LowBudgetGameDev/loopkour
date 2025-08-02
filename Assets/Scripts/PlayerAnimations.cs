using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private Animator animator;

    private new Rigidbody2D rigidbody2D;

    private bool isRunning;
    private bool inAir;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        isRunning = false;
        inAir = false;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2D.linearVelocityX) > 0.1f && !isRunning && !inAir)
        {
            isRunning = true;
            animator.SetBool("IsRunning", true);
        }

        if (Mathf.Abs(rigidbody2D.linearVelocityX) <= 0.1f && isRunning && !inAir)
        {
            isRunning = false;
            animator.SetBool("IsRunning", false);
        }

        if (rigidbody2D.linearVelocityY > 0f && !playerMovement.isGrounded && !inAir)
        {
            animator.SetTrigger("Jump");
            inAir = true;
        }

        if (rigidbody2D.linearVelocityY < 0f && !playerMovement.isGrounded)
        {
            animator.SetTrigger("Fall");
            inAir = true;
        }

        if (rigidbody2D.linearVelocityY == 0f && !playerMovement.isGrounded)
        {
            animator.SetTrigger("Midair");
            inAir = true;
        }

        if (rigidbody2D.linearVelocityY == 0f && playerMovement.isGrounded && inAir)
        {
            animator.SetTrigger("Land");
            inAir = false;
        }
    }
}
