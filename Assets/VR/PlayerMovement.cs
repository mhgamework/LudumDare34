using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Collections;
using Valve.VR;

[DefaultExecutionOrder(1)]
public class PlayerMovement : MonoBehaviour
{
    public SteamVR_TrackedObject controllerTracker;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)controllerTracker.index); }
    }

    public PlayerControl PlayerControl;

    [SerializeField]
    private int moveDir;

    void Start()
    {
        moveDir = 0;
    }

    void Update()
    {
        if (GetDPadPressDown(Controller, EVRButtonId.k_EButton_DPad_Left))
        {
            if (moveDir == -1)
                moveDir = 0;
            else
                moveDir = -1;
        }
        else if (GetDPadPressDown(Controller, EVRButtonId.k_EButton_DPad_Right))
        {
            if (moveDir == 1)
                moveDir = 0;
            else
                moveDir = 1;
        }

        PlayerControl.SetMoveDir(moveDir);
    }

    /*
    *  You might expect that pressing one of the edges of the SteamVR controller touchpad could
    *  be detected with a call to device.GetPress( EVRButtonId.k_EButton_DPad_* ), but currently this always returns false.
    *  Not sure whether this is SteamVR's design intent, not yet implemented, or a bug.
    *  The expected behaviour can be achieved by detecting overall Touchpad press, with Touch-Axis comparison to an edge threshold.
    */
    public static bool GetDPadPressDown(SteamVR_Controller.Device device, EVRButtonId dPadButtonId, float threshold = 0.05f)
    {
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))  // Is any DPad button pressed down?
        {
            var touchpad_axis = device.GetAxis();

            if (dPadButtonId == EVRButtonId.k_EButton_DPad_Left)
                return touchpad_axis.x < -threshold;
            if (dPadButtonId == EVRButtonId.k_EButton_DPad_Right)
                return touchpad_axis.x > threshold;
            if (dPadButtonId == EVRButtonId.k_EButton_DPad_Up)
                return touchpad_axis.y > threshold;
            if (dPadButtonId == EVRButtonId.k_EButton_DPad_Down)
                return touchpad_axis.y < -threshold;

            /*
            if (touchpadAxis.y > (1.0f - threshold)) { return dPadButtonId == EVRButtonId.k_EButton_DPad_Up; }
            else if (touchpadAxis.y < threshold) { return dPadButtonId == EVRButtonId.k_EButton_DPad_Down; }
            else if (touchpadAxis.x > (1.0f - threshold)) { return dPadButtonId == EVRButtonId.k_EButton_DPad_Right; }
            else if (touchpadAxis.x < threshold) { return dPadButtonId == EVRButtonId.k_EButton_DPad_Left; }*/
        }

        return false;
    }
}
