using UnityEngine;
using System.Collections;

public class RobotAnimator : MonoBehaviour
{

    public void PlayIdle()
    {
        Animator.Play("Idle");
    }

    public void PlayWalk()
    {
        Animator.Play("Walk");
    }

    [SerializeField]
    private Animator Animator = null;
}
