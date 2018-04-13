using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public SteamVR_TrackedObject controllerTracker;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)controllerTracker.index); }
    }

    public Transform WorldTransform;

    private Vector3 prevPosition;

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            prevPosition = controllerTracker.transform.localPosition;
        }
        else if (Controller.GetHairTrigger())
        {
            var position = controllerTracker.transform.localPosition;

            var delta = prevPosition - position;
            var delta_angle = Vector2.SignedAngle(new Vector2(prevPosition.x, prevPosition.z), new Vector2(position.x, position.z));

            WorldTransform.localPosition -= new Vector3(0, delta.y, 0);
            WorldTransform.localEulerAngles -= Vector3.up * delta_angle;
            
            prevPosition = position;
        }
    }
}
