using UnityEngine;
using System.Collections;

public class RobotGirlController : MonoBehaviour
{
    //todo

    void Update()
    {
        TargetHeight = CenterStairController.Get.GetCurrentHeight(); //the height of the last spawned step

        //update animator clip
        if(IsMoving)
            AnimationController.Play("Walk");
        else
            AnimationController.Play("Idle");
    }

    [SerializeField]
    private Animator AnimationController = null;

    private bool IsMoving;
    private float TargetHeight;
}
