using JetBrains.Annotations;
using NUnit.Framework.Constraints;
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
    None,
    Automatic,
    Follow,
    Mixed,
    EndEffector,
    Fine,
    N_SIZE
}

public class Controller : MonoBehaviour
{
    public GameObject trackedObject;
    bool isHoldingTarget = false;
    public GameObject left_target;
    public GameObject right_target;
    public GameObject player;
    public TeleportationAnchor teleportationAnchor;


    public XRBaseInteractor LeftInteractor;
    public XRBaseInteractor RightInteractor;

    
    private TMP_Dropdown mode_dropdown;
    /*
    public Button Button;
    public Button TeleportButton;
    public Toggle RotationToggle;
    public Toggle PositionToggle;
    public Toggle DynamicToggle;
    public Slider GraspSlider;
    public Slider ArmSpreadSlider;
    public Slider ArmBearingSlider;
    */

    private ControlMode mode;
    private bool trackPosition = true;
    private bool trackRotation = true;
    private bool dynamicAttach = true;
    private TentacleArmTargetPublisher tentacleArmTargetPublisher;


    private void Start()
    {
        mode = getMode();

        ControlPanel controlPanel = GetComponent<ControlPanel>();
        controlPanel.addButton("Kill", new UnityAction(UIButton));
        controlPanel.addButton("Teleport", new UnityAction(UITeleportButton));
        controlPanel.addSlider("Arm Bearing", new UnityAction<float>(UIArmBearingSlider));
        controlPanel.addSlider("Arm Spread", new UnityAction<float>(UIArmSpreadSlider));
        controlPanel.addSlider("Grasp", new UnityAction<float>(UIGraspSlider));
        mode_dropdown = controlPanel.addDropdown("Mode", new UnityAction<int>(UISwitchMode), Enum.GetNames(typeof(ControlMode)), (int)mode);
        controlPanel.addToggle("Position", new UnityAction<bool>(UIPositionToggle), trackPosition);
        controlPanel.addToggle("Rotation", new UnityAction<bool>(UIRotationToggle), trackRotation);
        controlPanel.addToggle("Dynamic", new UnityAction<bool>(UIDynamicToggle), dynamicAttach);


        tentacleArmTargetPublisher = gameObject.GetComponent<TentacleArmTargetPublisher>();

        LeftInteractor.selectEntered.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs>(grabObject));
        RightInteractor.selectEntered.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs>(grabObject));

        LeftInteractor.selectExited.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs>(releaseObject));
        RightInteractor.selectExited.AddListener(new UnityAction<UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs>(releaseObject));
        

        /*
        my_dropdown_action += UISwitchMode;
        Dropdown.onValueChanged.AddListener(my_dropdown_action);
        for(int i = 0; i<(int)ControlMode.N_SIZE; i++)
        {
            Dropdown.options.Add(new TMP_Dropdown.OptionData(((ControlMode)i).ToString()));
        }
        mode = getMode();
        UISwitchMode((int)ControlMode.None);
        Dropdown.transform.parent.GetComponentInChildren<TMP_Text>().text = "Mode";

        my_button_action += UIButton;
        Button.onClick.AddListener(my_button_action);
        Button.GetComponentInChildren<TMP_Text>().text = "Kill";

        my_teleport_button_action += UITeleportButton;
        TeleportButton.onClick.AddListener(my_teleport_button_action);
        TeleportButton.GetComponentInChildren<TMP_Text>().text = "Teleport To Drone";

        my_rotation_toggle_action += UIRotationToggle;
        RotationToggle.onValueChanged.AddListener(my_rotation_toggle_action);
        RotationToggle.transform.parent.parent.GetComponentInChildren<TMP_Text>().text = "Rotation";
        RotationToggle.isOn = trackRotation;


        my_position_toggle_action += UIPositionToggle;
        PositionToggle.onValueChanged.AddListener(my_position_toggle_action);
        PositionToggle.transform.parent.parent.GetComponentInChildren<TMP_Text>().text = "Position";
        PositionToggle.isOn = trackPosition;

        my_dynamic_toggle_action += UIDynamicToggle;
        DynamicToggle.onValueChanged.AddListener(my_dynamic_toggle_action);
        DynamicToggle.transform.parent.parent.GetComponentInChildren<TMP_Text>().text = "Dynamic Attach";
        DynamicToggle.isOn = dynamicAttach;

        ArmBearingSlider.onValueChanged.AddListener(new UnityAction<float>(UIArmBearingSlider));
        ArmBearingSlider.transform.parent.GetComponentInChildren<TMP_Text>().text = "Arm Bearing";
        ArmBearingSlider.minValue = -1f;
        ArmBearingSlider.maxValue = 1f;

        ArmSpreadSlider.onValueChanged.AddListener(new UnityAction<float>(UIArmSpreadSlider));
        ArmSpreadSlider.transform.parent.GetComponentInChildren<TMP_Text>().text = "Arm Spread";
        ArmBearingSlider.minValue = -1f;
        ArmBearingSlider.maxValue = 1f;

        my_grasp_slider_action += UIGraspSlider;
        GraspSlider.onValueChanged.AddListener(my_grasp_slider_action);
        GraspSlider.transform.parent.GetComponentInChildren<TMP_Text>().text = "Grasp";
        GraspSlider.minValue = -1f;
        */
    }

    public void grabObject(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        // disable all other interactors so you don't grab two things at the same time
        foreach (XRBaseInteractor interactor in new[] { LeftInteractor, RightInteractor }) {
            if (interactor.transform.gameObject != args.interactorObject.transform.gameObject)
            {
                interactor.enabled = false;
            }
        }


        GameObject newObject = args.interactableObject.transform.gameObject;

        if (newObject.CompareTag("target"))
        {
            newObject.GetComponent<XRGrabInteractable>().trackRotation = trackRotation;
            newObject.GetComponent<XRGrabInteractable>().trackPosition = trackPosition;
            newObject.GetComponent<XRGrabInteractable>().useDynamicAttach = dynamicAttach;

            isHoldingTarget = true;
            if (newObject != trackedObject)
            {

                Destroy(trackedObject);
                trackedObject = newObject;
                trackedObject.GetComponent<PoseTarget>().child.SetActive(true);
            }

            UISwitchMode((int)ControlMode.Automatic);

        }
    }

    public void releaseObject(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    {
        if (isHoldingTarget)
        {
            isHoldingTarget = false;

            // if the trackedobject was not held before, create duplicate at original parent, move held one to controller transform
            if (trackedObject.transform.parent != transform)
            {
                // create duplicate at original parent
                GameObject duplicateObject = Instantiate(trackedObject, trackedObject.transform.parent);
                duplicateObject.transform.SetParent(trackedObject.transform.parent);
                duplicateObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                duplicateObject.name = trackedObject.name;

                // destroy all XR interactor residual objects
                foreach (Transform child_transform in duplicateObject.transform)
                {
                    Destroy(child_transform.gameObject);
                }

                trackedObject.GetComponentInChildren<PosePublisher>().enabled = true;
                trackedObject.transform.SetParent(transform);
            }


            // move the tracked object to the transform of its child which may obey certain behaviours
            Transform trackedChildTransform = trackedObject.GetComponent<PoseTarget>().child.transform;
            trackedObject.transform.SetPositionAndRotation(trackedChildTransform.position, trackedChildTransform.rotation);
            trackedChildTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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

    private ControlMode getMode()
    {
        if (is_tracked_object_active())
        {
            return ControlMode.Automatic;
        }
        else if (left_target.activeInHierarchy && right_target.activeInHierarchy)
        {
            return ControlMode.Follow;
        }
        else
        {
            return ControlMode.None;
        }
    }

    void UISwitchMode(int value)
    {
        ControlMode newMode = (ControlMode)value;
        
        if (newMode == mode)
        {
            return;
        }

        if (newMode != ControlMode.Automatic)
        {
            try_disable_tracked_object();
        }

        if (newMode == ControlMode.Follow)
        {
            left_target.SetActive(true);
            right_target.SetActive(true);
        } else
        {
            left_target.SetActive(false);
            right_target.SetActive(false);
        }



        ControlMode actualMode = getMode();

        if (actualMode != newMode)
        {
            Debug.LogWarning("Failed to switch from " + mode + " to " + newMode + "!   Actual mode = " +  actualMode);
        } else
        {
            // Debug.Log("switched from " + mode + " to " + newMode);
        }

        mode = actualMode;
        mode_dropdown.value = (int)actualMode;
    }

    void UIButton()
    {
        Debug.Log("button");
    }

    void UITeleportButton()
    {
        //player.transform.rotation = teleportationAnchor.transform.rotation;
        teleportationAnchor.RequestTeleport();
    }

    void UIRotationToggle(bool value)
    {
        trackRotation = value;
    }

    void UIPositionToggle(bool value)
    {
        trackPosition = value;
    }

    void UIDynamicToggle(bool value)
    {
        dynamicAttach = value;
    }

    void UIArmBearingSlider(float value)
    {
        tentacleArmTargetPublisher.SetArmBearing(value);
    }

    void UIArmSpreadSlider(float value)
    {
        tentacleArmTargetPublisher.SetArmSpread(value);
    }

    void UIGraspSlider(float value)
    {
        tentacleArmTargetPublisher.SetTentacleSpread(value);
    }
}
