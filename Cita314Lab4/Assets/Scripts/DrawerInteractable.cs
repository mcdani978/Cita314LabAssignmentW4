using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [SerializeField] UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;

    private Transform parentTransform;
    private const string defaultLayer = "Default";

    void Start()
    {
        if (keySocket != null)
        {
            keySocket.selectEntered.AddListener(OnDrawerUnlocked);
            keySocket.selectExited.AddListener(OnDrawerLocked);
        }
        else
        {
            Debug.LogWarning("Key socket is not assigned in the Inspector.");
        }

        if (transform.parent != null)
        {
            parentTransform = transform.parent;
        }
        else
        {
            Debug.LogWarning("DrawerInteractable has no parent! Assigning to itself.");
            parentTransform = transform; // Fallback to itself if no parent
        }
    }

    private void OnDrawerLocked(SelectExitEventArgs args)
    {
        isLocked = true;
        Debug.Log("Drawer locked!");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isLocked)
        {
            transform.SetParent(parentTransform);
        }
        else
        {
            interactionLayers &= ~InteractionLayerMask.GetMask(defaultLayer); // Properly modify layers
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactionLayers = InteractionLayerMask.GetMask(defaultLayer);
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs args)
    {
        isLocked = false;
        Debug.Log("Drawer unlocked!");
    }
}
