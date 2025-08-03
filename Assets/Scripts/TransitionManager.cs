using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [SerializeField] private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTransition()
    {
        animator.SetTrigger("Start");
    }
}
