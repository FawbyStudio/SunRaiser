//
// ShakeCamera.cs
// Copyright (c) 2012-2017 Thinksquirrel Inc.
//
using UnityEngine;
using Thinksquirrel.CShake;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Camera")]
    [Tooltip("Shake one or more cameras, using Camera Shake.")]
    public class ShakeCamera : FsmStateAction
    {
        [Title("(Optional) Camera Shake")] [Tooltip("The camera shake component to use.")] public FsmObject cameraShakeComponent;
        [Tooltip("The maximum number of shakes to perform.")] public FsmInt numberOfShakes;
        [Tooltip("The amount to shake in each direction.")] public FsmVector3 shakeAmount;
        [Tooltip("The amount to rotate in each axis.")] public FsmVector3 rotationAmount;
        [Tooltip("The initial distance for the first shake.")] public FsmFloat distance;
        [Tooltip("The speed multiplier for the shake.")] public FsmFloat speed;
        [Tooltip("The decay speed (between 0 and 1). Higher values will stop shaking sooner.")] public FsmFloat decay;
        [Tooltip("The modifier applied to speed in order to shake the GUI.")] public FsmFloat guiShakeModifier;
        [Tooltip("If true, multiplies the shake speed by the time scale.")] public FsmBool multiplyByTimeScale;
        [Tooltip("If true, transition out of the state immediately. Otherwise, wait until the shake is finished.")] public FsmBool finishImmediately;

        public override void Reset()
        {
            // Set defaults
            cameraShakeComponent = null;
            numberOfShakes = 2;
            shakeAmount = Vector3.one;
            rotationAmount = Vector3.one;
            distance = 00.10f;
            speed = 50.00f;
            decay = 00.20f;
            guiShakeModifier = 01.00f;
            multiplyByTimeScale = true;
        }

        public override void OnEnter()
        {
            var go = cameraShakeComponent.Value as GameObject;
            CameraShake shakeComponent;

            if (cameraShakeComponent.Value)
            {
                shakeComponent = go != null ? go.GetComponent<CameraShake>() : cameraShakeComponent.Value as CameraShake;

                if (shakeComponent)
                {
                    shakeComponent.Shake(
                      shakeComponent.shakeType,
                      numberOfShakes.Value,
                      shakeAmount.Value,
                      rotationAmount.Value,
                      distance.Value,
                      speed.Value,
                      decay.Value,
                      guiShakeModifier.Value,
                      multiplyByTimeScale.Value,
                      CameraShakeCallback);
                }
                else
                {
                    LogError("No Camera Shake component found on the selected GameObject!");
                }
            }
            else
            {
                CameraShake.ShakeAll(
                  CameraShake.ShakeType.CameraMatrix,
                  numberOfShakes.Value,
                  shakeAmount.Value,
                  rotationAmount.Value,
                  distance.Value,
                  speed.Value,
                  decay.Value,
                  guiShakeModifier.Value,
                  multiplyByTimeScale.Value,
                  CameraShakeCallback);
            }
            if (finishImmediately.Value) Finish();
        }

        void CameraShakeCallback()
        {
            if (!finishImmediately.Value) Finish();
        }
    }
}