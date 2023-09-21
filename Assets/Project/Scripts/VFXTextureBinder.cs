using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


// Dirty Binder
public class VFXTextureBinder : MonoBehaviour
{
    private VisualEffect _vfx;
    [SerializeField] GameObject _fluidObject;

    private IFluidManager _fluidManager;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        _fluidManager = _fluidObject.GetComponent<IFluidManager>();

        _vfx = GetComponent<VisualEffect>();
        BindTexture();
    }

    [ContextMenu("Bind")]
    public void BindTexture()
    {
        _vfx.SetTexture("VelocityTexture", _fluidManager.GetVelocity2D());
        _vfx.SetTexture("DyeTexture", _fluidManager.GetDye2D());
        _vfx.SetInt("IsBinded", 1);
    }
}

