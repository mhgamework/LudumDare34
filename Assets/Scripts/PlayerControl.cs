using UnityEngine;
using System.Collections;

using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]

public class PlayerControl : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    public float circleRadius;
    public float circleAngle;

    [SerializeField]
    private RobotAnimator RobotAnimator = null;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();



        circleRadius = new Vector3(transform.position.x, 0, transform.position.z).magnitude;

    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        var moveDir = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
            moveDir += -1;
        if (Input.GetKey(KeyCode.RightArrow))
            moveDir += 1;

        if (RobotAnimator != null)
        {
            if(moveDir == 0)
                RobotAnimator.PlayIdle();
            else
                RobotAnimator.PlayWalk();
        }

        var pos = transform.position;
        var currentAngle = Mathf.Atan2(pos.z, pos.x);

        // Correct for stupid animation deviations
        var corrPos = angleToPos(currentAngle);
        pos.x = corrPos.x;
        pos.z = corrPos.z;
        transform.position = pos;

        var newAngle = currentAngle + moveDir * 0.2f;

        circleAngle = newAngle;

        var newPos = angleToPos(newAngle);
        newPos.y = pos.y;

        var delta = newPos - pos;
        if (delta.magnitude < 0.001 || moveDir == 0) delta = new Vector3();


        m_Character.Move(delta, false, false);


        //        // read inputs
        //        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        //        float v = CrossPlatformInputManager.GetAxis("Vertical");
        //        bool crouch = Input.GetKey(KeyCode.C);

        //        // calculate move direction to pass to character
        //        if (m_Cam != null)
        //        {
        //            // calculate camera relative direction to move:
        //            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        //            m_Move = v * m_CamForward + h * m_Cam.right;
        //        }
        //        else
        //        {
        //            // we use world-relative directions in the case of no main camera
        //            m_Move = v * Vector3.forward + h * Vector3.right;
        //        }
        //#if !MOBILE_INPUT
        //        // walk speed multiplier
        //        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
        //#endif

        //        // pass all parameters to the character control script
        //        m_Character.Move(m_Move, crouch, m_Jump);
        //        m_Jump = false;
    }

    private Vector3 angleToPos(float newAngle)
    {
        return circleRadius * new Vector3(Mathf.Cos(newAngle), 0, Mathf.Sin(newAngle));
    }
}
