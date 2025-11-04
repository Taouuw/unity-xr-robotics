using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public enum ControlMode
{
    Automatic,
    Manual,
    EndEffector,
    FineTune,
    N_SIZE
}

public class Selecting : MonoBehaviour
{
    public GameObject trackedObject;
    bool isHoldingTarget = false;
    public bool isTeleporting = true;
    public GameObject left_target;
    public GameObject right_target;
    public TeleportationAnchor teleportationAnchor;
    public int mode = 0;
    public TMP_Dropdown Dropdown;
    public Button Button;
    public XRBaseInteractor LeftInteractor;
    public XRBaseInteractor RightInteractor;

    private UnityAction<int> my_dropdown_action;
    private UnityAction my_button_action;

    private void Start()
    {

        LeftInteractor.selectEntered.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs>(grabObject));
        RightInteractor.selectEntered.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs>(grabObject));

        LeftInteractor.selectExited.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs>(releaseObject));
        RightInteractor.selectExited.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs>(releaseObject));

        my_dropdown_action += UISwitchMode;

        Dropdown.onValueChanged.AddListener(my_dropdown_action);
        foreach (ControlMode mode in Enum.GetValues(typeof(Mode)))
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(mode.ToString()));
        }

        my_button_action += UIButton;
        Button.onClick.AddListener(my_button_action);
        Button.GetComponentInChildren<TMP_Text>().text = "Reconnect";
    }

    private void Update()
    {
        left_target.SetActive(!is_tracked_object_active());
        right_target.SetActive(!is_tracked_object_active());
    }


    public void grabObject(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {

        GameObject newObject = args.interactableObject.transform.gameObject;

        foreach (XRBaseInteractor interactor in new[] { LeftInteractor, RightInteractor }) {
            if (interactor.transform.gameObject != args.interactorObject.transform.gameObject)
            {
                interactor.enabled = false;
            }
        }

        if (newObject.CompareTag("target"))
        {
            isHoldingTarget = true;
            if (newObject != trackedObject)
            {

                Destroy(trackedObject);
                trackedObject = newObject;
                trackedObject.GetComponent<CaptureTargetModel>().model.SetActive(true);
            }
        }
    }

    public void releaseObject(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    {
        if (isHoldingTarget)
        {
            isHoldingTarget = false;

            if (trackedObject.transform.parent != transform)
            {
                GameObject duplicateObject = Instantiate(trackedObject, trackedObject.transform.parent);
                duplicateObject.transform.SetParent(trackedObject.transform.parent);
                duplicateObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                duplicateObject.name = trackedObject.name;
                foreach (Transform child_transform in duplicateObject.transform)
                {
                    Destroy(child_transform.gameObject);
                }
            }


            PosePublisher posePublisher = trackedObject.GetComponent<PosePublisher>();
            posePublisher.enabled = true;
            posePublisher.topicName = trackedObject.transform.parent.gameObject.GetComponent<PoseSubscriber>().topicName + "_target";

            trackedObject.transform.SetParent(transform);
        }
    }

    public void ReleaseObject(XRBaseInteractor interactor)
    {
        if (isHoldingTarget)
        {
            isHoldingTarget = false;

            if (trackedObject.transform.parent != transform)
            {
                GameObject duplicateObject = Instantiate(trackedObject, trackedObject.transform.parent);
                duplicateObject.transform.SetParent(trackedObject.transform.parent);
                duplicateObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                duplicateObject.name = trackedObject.name;
                foreach (Transform child_transform in duplicateObject.transform)
                {
                    Destroy(child_transform.gameObject);
                }
            }


            PosePublisher posePublisher = trackedObject.GetComponent<PosePublisher>();
            posePublisher.enabled = true;
            posePublisher.topicName = trackedObject.transform.parent.gameObject.GetComponent<PoseSubscriber>().topicName + "_target";

            trackedObject.transform.SetParent(transform);
        }
    }
    public void GrabObject(XRBaseInteractor interactor)
    {

        if (interactor.interactablesSelected.Count != 1)
        {
            Debug.Log("n interactables = " + interactor.interactablesSelected.Count + " != " + 1);
        }

        GameObject newObject = interactor.interactablesSelected[0].transform.gameObject;

        if (newObject.CompareTag("target"))
        {
            isHoldingTarget = true;
            if (newObject != trackedObject)
            {

                Destroy(trackedObject);
                trackedObject = newObject;
                trackedObject.GetComponent<CaptureTargetModel>().model.SetActive(true);
            }
        }
    }
    public void try_disable_tracked_object()
    {
        if (!isHoldingTarget)
        {
            if (trackedObject)
            {
                trackedObject.SetActive(false);
            }
        }
    }

    public bool is_tracked_object_active()
    {
        if (trackedObject)
        {
            return trackedObject.activeSelf;
        }
        return false;
    }

    void UISwitchMode(int value)
    {
        mode = Dropdown.value;
        Debug.Log("mode switched to: " + Dropdown.value);
    }

    void UIButton()
    {
        try_disable_tracked_object();
        if (!is_tracked_object_active())
        {
            if (isTeleporting && !is_tracked_object_active())
            {
                teleportationAnchor.RequestTeleport();
            }

        }
    }

    public void OnActivate()
    {
        return;
        // try_enable_tracked_object();
        // if (isTeleporting && !is_tracked_object_active())
        // {
        //     teleportationAnchor.RequestTeleport();
        // }
    }
}
