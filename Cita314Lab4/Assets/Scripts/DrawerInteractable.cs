using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor keySocket;
    [SerializeField] private bool isLocked;

    private Transform parentTransform;
    private const string defaultLayer = "Default";
    private const string grabLayer = "Grab";

    private Vector3 limitPositions;
    [SerializeField] private Vector3 limitDistances = new Vector3(.02f, .02f, 0);

    private bool isGrabbed; // Added missing variable

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

        limitPositions = transform.localPosition; // Corrected from drawerTransform
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
            isGrabbed = true;
        }
        else
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        ChangeLayerMask(grabLayer);
        interactionLayers = InteractionLayerMask.GetMask(grabLayer); // Restore proper grab layer
        isGrabbed = false;
        transform.localPosition = limitPositions; // Prevents unwanted movement
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs args)
    {
        isLocked = false;
        Debug.Log("Drawer unlocked!");
    }

    void Update()
    {
        if (isGrabbed)
        {
            transform.localPosition = new Vector3(transform.localPosition.x,
                transform.localPosition.y, transform.localPosition.z);

            CheckLimits();
        }
    }

    private void CheckLimits()
    {
        if (transform.localPosition.x >= limitPositions.x + limitDistances.x ||
            transform.localPosition.x <= limitPositions.x - limitDistances.x ||
            transform.localPosition.y >= limitPositions.y + limitDistances.y ||
            transform.localPosition.y <= limitPositions.y - limitDistances.y)
        {
            ChangeLayerMask(defaultLayer);
        }
    }

    private void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
