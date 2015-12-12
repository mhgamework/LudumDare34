using UnityEngine;
using System.Collections;

public class Grate : MonoBehaviour
{

    public void Open()
    {
        IsOpen = true;
        StopCoroutine("UpdateMove");
        StartCoroutine("UpdateMove", EndHeightOffset + InitialHeight);
    }

    public void Close()
    {
        IsOpen = false;
        StopCoroutine("UpdateMove");
        StartCoroutine("UpdateMove", InitialHeight);
    }

    public void ToggleOpenState()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    void Start()
    {
        InitialHeight = MeshTransform.localPosition.y;
    }

    IEnumerator UpdateMove(float target_height)
    {
        var target_pos = new Vector3(MeshTransform.localPosition.x, target_height, MeshTransform.localPosition.z);
        var start_pos = MeshTransform.localPosition;

        var elapsed = 0f;
        while (elapsed < AnimationTime)
        {
            MeshTransform.localPosition = EasingFunctions.Ease(EasingType, elapsed / AnimationTime, start_pos, target_pos);
            elapsed += Time.deltaTime;
            yield return null;
        }

        MeshTransform.localPosition = target_pos;
    }


    [SerializeField]
    private Transform MeshTransform = null;
    [SerializeField]
    private float AnimationTime = 1f;
    [SerializeField]
    private EasingFunctions.TYPE EasingType = EasingFunctions.TYPE.Out;
    [SerializeField]
    private float EndHeightOffset = 2f;

    private float InitialHeight;

    private bool IsOpen;

}
