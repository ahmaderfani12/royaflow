using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPresenter : MonoBehaviour
{
    [SerializeField] CameraController controller;

    [SerializeField] UIHoldButton _buttonRight;
    [SerializeField] UIHoldButton _buttonLeft;
    [SerializeField] UIHoldButton _buttonForward;
    [SerializeField] UIHoldButton _buttonBack;

    private void Update()
    {
        controller.ControlParam = new Vector4(_buttonRight.holdTime, _buttonLeft.holdTime, _buttonForward.holdTime, _buttonBack.holdTime);
    }
}
