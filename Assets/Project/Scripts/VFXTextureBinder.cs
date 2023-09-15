using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


// Dirty Binder
public class VFXTextureBinder : MonoBehaviour
{
    private VisualEffect _vfx;
    [SerializeField] PersianGardenManagerTest _persianGardenDemoSceneMaster;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        _vfx = GetComponent<VisualEffect>();
        BindTexture();
    }

    [ContextMenu("Bind")]
    public void BindTexture()
    {
        _vfx.SetTexture("VelocityTexture", _persianGardenDemoSceneMaster.velocity_texture);
        _vfx.SetTexture("DyeTexture", _persianGardenDemoSceneMaster.fluid_simulater.visulasation_texture);
        _vfx.SetInt("IsBinded", 1);
    }
}

