using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{

    public Vector4 ControlParam;

    [SerializeField] Vector2 _zoomLimit;
    [SerializeField] AnimationCurve _speedCurve;
    [SerializeField] float _rotationSpeed = 2;
    [SerializeField] float _defaultYRotation = -27;
    [SerializeField] float _defalutZoom = -6.75f;

    private void FixedUpdate()
    {
        UICameraControl();
    }

    private void UICameraControl()
    {
        // Rotation
        transform.Rotate(Vector3.up, _speedCurve.Evaluate(Mathf.Abs(ControlParam.x - ControlParam.y)) * _rotationSpeed
    * Mathf.Sign(ControlParam.y - ControlParam.x));


        // Zoom
        if (Camera.main.transform.localPosition.z > _zoomLimit.x && Camera.main.transform.localPosition.z < _zoomLimit.y)
        {
            float zoomFactor = (ControlParam.z - ControlParam.w);
            float zoomSign = Mathf.Sign(zoomFactor);
            Camera.main.transform.localPosition = new Vector3(0.0f,
                Camera.main.transform.localPosition.y,
                Camera.main.transform.localPosition.z + _speedCurve.Evaluate(Mathf.Abs(zoomFactor)) * zoomSign * 0.06f);
        }
        else
        {
            float mid = (_zoomLimit.x + _zoomLimit.y) / 2;
            if (Camera.main.transform.localPosition.z > mid)
            {
                Camera.main.transform.localPosition = new Vector3(0.0f,
                Camera.main.transform.localPosition.y,
                Camera.main.transform.localPosition.z - 0.01f);
            }
            else
            {
                Camera.main.transform.localPosition = new Vector3(0.0f,
                Camera.main.transform.localPosition.y,
                Camera.main.transform.localPosition.z + 0.01f);
            }
        }
    }


    [ContextMenu("Reset")]
    public void ResetCamera()
    {
        transform.DORotate(new Vector3(0, -28, 0), 2);
        Camera.main.transform.DOLocalMoveZ(_defalutZoom, 2);
    }
}
