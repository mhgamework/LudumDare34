using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Serializable]
    public class OnPressurePlateActivatedEventHandler : UnityEvent { }
    [Serializable]
    public class OnPressurePlateDeactivatedEventHandler : UnityEvent { }

    public OnPressurePlateActivatedEventHandler OnPressurePlateActivated;
    public OnPressurePlateDeactivatedEventHandler OnPressurePlateDeactivated;

    private AudioSource audio;

    void Start()
    {
        InitialMeshHeight = MeshTransform.localPosition.y;
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (CurrentColliders.Count != PrevColliderCount)
        {
            if (CurrentColliders.Count == 0)
                DeactivatePressurePlate();
            else
                ActivatePressurePlate();

            PrevColliderCount = CurrentColliders.Count;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PressurePlateActivator>() != null)
        {
            CurrentColliders.Add(other);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        CurrentColliders.Remove(other);
        
    }

    private void ActivatePressurePlate()
    {
        if (OnPressurePlateActivated != null)
            OnPressurePlateActivated.Invoke();

        var pos = MeshTransform.localPosition;
        if (Math.Abs(MeshTransform.localPosition.y - (InitialMeshHeight - 0.1f)) > 0.001)
            audio.Play();
        MeshTransform.localPosition = new Vector3(pos.x, InitialMeshHeight - 0.1f, pos.z);
    }

    private void DeactivatePressurePlate()
    {
        if (OnPressurePlateDeactivated != null)
            OnPressurePlateDeactivated.Invoke();

        var pos = MeshTransform.localPosition;
        if (Math.Abs(MeshTransform.localPosition.y - InitialMeshHeight) > 0.001)
            audio.Play();
        MeshTransform.localPosition = new Vector3(pos.x, InitialMeshHeight, pos.z);
        
    }

    private List<Collider> CurrentColliders = new List<Collider>();
    private int PrevColliderCount;

    [SerializeField]
    private Transform MeshTransform = null;

    private float InitialMeshHeight;
}
