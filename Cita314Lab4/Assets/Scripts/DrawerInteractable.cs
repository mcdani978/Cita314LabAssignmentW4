using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Correct namespace

public class DrawerInteractable : MonoBehaviour
{
    [SerializeField] UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor keySocket;
    [SerializeField] bool isLocked;

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
    }
    private void OnDrawerlocked(SelectEnterEventArgs args)
    {
        isLocked = true;
        Debug.Log("Drawer unlocked!");
    }

    private void OnDrawerUnlocked(SelectEnterEventArgs args)
    {
        isLocked = false;
        Debug.Log("Drawer unlocked!");
    }
}
