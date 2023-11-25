using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureTextureModifier : MonoBehaviour
{
    [SerializeField] ComputeShader _zeroOutShader;
    [SerializeField, Range(0, 1)] float _reductionSpeed = 0.6f;
    private RenderTexture _renderTexture;

    private int _kernelID;

    private void Start()
    {
        _kernelID = _zeroOutShader.FindKernel("CSMain");
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            _zeroOutShader.SetTexture(_kernelID, "ResultTexture", _renderTexture);
            _zeroOutShader.SetFloat("reductionSpeed", _reductionSpeed);
            _zeroOutShader.Dispatch(_kernelID, 1024 / 16, 1024 / 16, 1);
        }
    }

    public void SetRenderTexture(RenderTexture rt) => _renderTexture = rt;
}
