using GorillaLocomotion;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace PineappleMod.Tools
{
    public class GestureTracker : MonoBehaviour
    {
        public static GestureTracker Instance;

        public InputDevice leftController, rightController;

        public InputTracker<float>
            leftGrip, rightGrip,
            leftTrigger, rightTrigger;
        public InputTracker<bool>
            leftStick, rightStick,
            leftPrimary, rightPrimary,
            leftSecondary, rightSecondary;
        public InputTracker<Vector2>
            leftStickAxis, rightStickAxis;

        public List<InputTracker> inputs;

        public GameObject
            chest,
            leftPointerObj, rightPointerObj,
            leftHand, rightHand;

        public BodyVectors leftHandVectors, rightHandVectors, headVectors;

        public Transform leftPointerTransform, rightPointerTransform, leftThumbTransform, rightThumbTransform;

        public const string localRigPath =
            "Player Objects/Local VRRig/Local Gorilla Player";
        public const string palmPath =
            "/RigAnchor/rig/body/shoulder.{0}/upper_arm.{0}/forearm.{0}/hand.{0}/palm.01.{0}";
        public const string pointerFingerPath =
            palmPath + "/f_index.01.{0}/f_index.02.{0}/f_index.03.{0}";
        public const string thumbPath =
            palmPath + "/thumb.01.{0}/thumb.02.{0}/thumb.03.{0}";


        public struct BodyVectors
        {
            public Vector3 pointerDirection, palmNormal, thumbDirection;
        }

        void Awake()
        {
            Instance = this;
            leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            var poller = Traverse.Create(ControllerInputPoller.instance);
            var pollerExt = Traverse.Create(new ControllerInputPollerExt());

            leftGrip = new InputTracker<float>(poller.Field("leftControllerGripFloat"), XRNode.LeftHand);
            rightGrip = new InputTracker<float>(poller.Field("rightControllerGripFloat"), XRNode.RightHand);

            leftTrigger = new InputTracker<float>(poller.Field("leftControllerIndexFloat"), XRNode.LeftHand);
            rightTrigger = new InputTracker<float>(poller.Field("rightControllerIndexFloat"), XRNode.RightHand);

            leftPrimary = new InputTracker<bool>(poller.Field("leftControllerPrimaryButton"), XRNode.LeftHand);
            rightPrimary = new InputTracker<bool>(poller.Field("rightControllerPrimaryButton"), XRNode.RightHand);

            leftSecondary = new InputTracker<bool>(poller.Field("leftControllerSecondaryButton"), XRNode.LeftHand);
            rightSecondary = new InputTracker<bool>(poller.Field("rightControllerSecondaryButton"), XRNode.RightHand);

            leftStick = new InputTracker<bool>(pollerExt.Field("leftControllerStickButton"), XRNode.LeftHand);
            rightStick = new InputTracker<bool>(pollerExt.Field("rightControllerStickButton"), XRNode.RightHand);

            leftStickAxis = new InputTracker<Vector2>(pollerExt.Field("leftControllerStickAxis"), XRNode.LeftHand);
            rightStickAxis = new InputTracker<Vector2>(pollerExt.Field("rightControllerStickAxis"), XRNode.RightHand);

            inputs = new List<InputTracker>()
            {
                leftGrip, rightGrip,
                leftTrigger, rightTrigger,
                leftPrimary, rightPrimary,
                leftSecondary, rightSecondary,
                leftStick, rightStick,
                leftStickAxis, rightStickAxis
            };
        }

        void Update()
        {
            ControllerInputPollerExt.Instance.Update();
            UpdateValues();
            TrackBodyVectors();
        }

        public void UpdateValues()
        {
            foreach (var input in inputs)
                input.UpdateValues();
        }

        void TrackBodyVectors()
        {
            var left = leftHand.transform;
            leftHandVectors = new BodyVectors()
            {
                pointerDirection = left.forward,
                palmNormal = left.right,
                thumbDirection = left.up
            };
            var right = rightHand.transform;
            rightHandVectors = new BodyVectors()
            {
                pointerDirection = right.forward,
                palmNormal = right.right * -1,
                thumbDirection = right.up
            };

            var head = GTPlayer.Instance.headCollider.transform;
            headVectors = new BodyVectors()
            {
                pointerDirection = head.forward,
                palmNormal = head.right,
                thumbDirection = head.up
            };
        }

        GameObject leftPalmInteractor, rightPalmInteractor, leftPointerInteractor, rightPointerInteractor;

        void BuildColliders()
        {
            var player = GTPlayer.Instance;
            chest = new GameObject("Body Gesture Collider");
            chest.AddComponent<CapsuleCollider>().isTrigger = true;
            chest.AddComponent<Rigidbody>().isKinematic = true;
            chest.transform.SetParent(player.transform.FindChildRecursive("Body Collider"), false);
            chest.layer = LayerMask.NameToLayer("Water");
            float
                height = 1 / 8f,
                radius = 1 / 4f;
            chest.transform.localScale = new Vector3(radius, height, radius);

            var leftPalm = GameObject.Find(string.Format(localRigPath + palmPath, "L")).transform;
            leftPalmInteractor = CreateInteractor("Left Palm Interactor", leftPalm, 1 / 16f);
            leftHand = leftPalmInteractor.gameObject;
            leftHand.transform.localRotation = Quaternion.Euler(-90, -90, 0);

            var rightPalm = GameObject.Find(string.Format(localRigPath + palmPath, "R")).transform;
            rightPalmInteractor = CreateInteractor("Right Palm Interactor", rightPalm, 1 / 16f);
            rightHand = rightPalmInteractor.gameObject;
            rightHand.transform.localRotation = Quaternion.Euler(-90, 0, 0);


            leftPointerTransform = GameObject.Find(string.Format(localRigPath + pointerFingerPath, "L")).transform;
            leftPointerInteractor = CreateInteractor("Left Pointer Interactor", leftPointerTransform, 1 / 32f);
            leftPointerObj = leftPointerInteractor;

            rightPointerTransform = GameObject.Find(string.Format(localRigPath + pointerFingerPath, "R")).transform;
            rightPointerInteractor = CreateInteractor("Right Pointer Interactor", rightPointerTransform, 1 / 32f);
            rightPointerObj = rightPointerInteractor;

            leftThumbTransform = GameObject.Find(string.Format(localRigPath + thumbPath, "L")).transform;
            rightThumbTransform = GameObject.Find(string.Format(localRigPath + thumbPath, "R")).transform;
        }

        public GameObject CreateInteractor(string name, Transform parent, float scale)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent, false);
            obj.transform.localScale = Vector3.one * scale;
            return obj;
        }
        public XRController GetController(bool isLeft)
        {
            foreach (var controller in FindObjectsOfType<XRController>())
            {
                if (isLeft && controller.name.ToLowerInvariant().Contains("left"))
                {
                    return controller;
                }
                if (!isLeft && controller.name.ToLowerInvariant().Contains("right"))
                {
                    return controller;
                }
            }
            return null;
        }

        public void OnDestroy()
        {
            leftHand?.Destroy();
            rightHand?.Destroy();
            leftPointerObj?.Destroy();
            rightPointerObj?.Destroy();
            Instance = null;
        }

        public void HapticPulse(bool isLeft, float strength = .5f, float duration = .05f)
        {
            var hand = isLeft ? leftController : rightController;
            hand.SendHapticImpulse(0u, strength, duration);
        }

        public InputTracker GetInputTracker(string name, XRNode node)
        {
            switch (name)
            {
                case "grip":
                    return node == XRNode.LeftHand ? leftGrip : rightGrip;
                case "trigger":
                    return node == XRNode.LeftHand ? leftTrigger : rightTrigger;
                case "stick":
                    return node == XRNode.LeftHand ? leftStick : rightStick;
                case "primary":
                    return node == XRNode.LeftHand ? leftPrimary : rightPrimary;
                case "secondary":
                    return node == XRNode.LeftHand ? leftSecondary : rightSecondary;
                case "a/x":
                    return node == XRNode.LeftHand ? leftPrimary : rightPrimary;
                case "b/y":
                    return node == XRNode.LeftHand ? leftSecondary : rightSecondary;
                case "a":
                    return rightPrimary;
                case "b":
                    return rightSecondary;
                case "x":
                    return leftPrimary;
                case "y":
                    return leftSecondary;
            }
            return null;
        }
    }

    public abstract class InputTracker
    {
        public bool pressed, wasPressed;
        public Vector3 vector3Value;
        public Quaternion quaternionValue;
        public XRNode node;
        public string name;
        public Traverse traverse;
        public Action<InputTracker> OnPressed, OnReleased;

        public abstract void UpdateValues();
    }

    public class InputTracker<T> : InputTracker
    {
        public InputTracker(Traverse traverse, XRNode node)
        {
            this.traverse = traverse;
            this.node = node;
        }

        public T GetValue()
        {
            return traverse.GetValue<T>();
        }
        public override void UpdateValues()
        {
            wasPressed = pressed;
            if (typeof(T) == typeof(bool))
                pressed = traverse.GetValue<bool>();
            else if (typeof(T) == typeof(float))
                pressed = traverse.GetValue<float>() > .5f;

            if (!wasPressed && pressed)
                OnPressed?.Invoke(this);
            if (wasPressed && !pressed)
                OnReleased?.Invoke(this);
        }
    }

    public class ControllerInputPollerExt
    {
        public bool rightControllerStickButton, leftControllerStickButton;
        public Vector2 rightControllerStickAxis, leftControllerStickAxis;
        public static ControllerInputPollerExt Instance;

        public ControllerInputPollerExt()
        {
            Instance = this;
        }
        public void Update()
        {
            var left = GestureTracker.Instance.leftController;
            var right = GestureTracker.Instance.rightController;

            left.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftControllerStickButton);
            right.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightControllerStickButton);
            left.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftControllerStickAxis);
            right.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightControllerStickAxis);
        }
    }
}