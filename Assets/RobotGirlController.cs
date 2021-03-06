﻿using System;
using UnityEngine;
using System.Collections;

public class RobotGirlController : MonoBehaviour
{
    //todo

    void Update()
    {
        updateMove();


        //update animator clip
        if (IsMoving)
            AnimationController.Play("Walk");
        else
            AnimationController.Play("Idle");
    }

    private void updateMove()
    {
        TargetHeight = CenterStairController.Get.GetCurrentHeight(); //the height of the last spawned step

        var diff = TargetHeight - CurrentHeight;
        CurrentHeight +=Mathf.Sign(diff)* Mathf.Min( Mathf.Abs( diff), Time.deltaTime * MoveSpeed);

        IsMoving = Math.Abs(TargetHeight - CurrentHeight) > 0.001;

        var radius = new Vector2(transform.position.x, transform.position.z).magnitude;

        var angle = CurrentHeight * AngleScale + AngleOffset; 

        var pos = new Vector3(Mathf.Cos(angle) * radius, CurrentHeight, Mathf.Sin(angle) * radius);
        var targetPos = new Vector3(Mathf.Cos(angle)*(radius + 0.1f), CurrentHeight, Mathf.Sin(angle)*(radius + 0.1f)) + PositionOffset;
        transform.position = pos + PositionOffset;



        transform.LookAt(targetPos);
        transform.Rotate(Vector3.up, YRotation);

    }

    [SerializeField]
    private Animator AnimationController = null;

    private bool IsMoving;
    private float TargetHeight;

    public float CurrentHeight;
    public float AngleOffset;
    public float AngleScale = 1;

    public Vector3 PositionOffset;
    public float MoveSpeed = 1;

    public float YRotation;
}
