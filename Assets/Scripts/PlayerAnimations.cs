using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;

    private new Rigidbody2D rigidbody2D;

    private bool isRunning;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        isRunning = false;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2D.linearVelocityX) > 0.1f && !isRunning)
        {
            isRunning = true;
            animator.SetTrigger("Run");
        }

        if (Mathf.Abs(rigidbody2D.linearVelocityX) <= 0.1f && isRunning)
        {
            isRunning = false;
            animator.SetTrigger("Stop");
        }
    }
}
