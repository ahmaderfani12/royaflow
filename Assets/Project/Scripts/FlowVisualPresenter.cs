using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using DG.Tweening;

public class FlowVisualPresenter : MonoBehaviour
{
    [SerializeField] VisualEffect _flow;
    [SerializeField] Slider _appearanceSlider;
    [SerializeField] Slider _goldenRainSlider;

    [SerializeField] float _appearanceTime = 6;
    [SerializeField] AnimationCurve _appearanceCurve;
    [SerializeField] float _goldenRainTime = 7;
    [SerializeField] AnimationCurve _goldenRainCurve;


    public void UpdateAppearance()
    {
        _flow.SetFloat("Appearance", _appearanceSlider.value);
    }

    public void UpdateGoldenRain()
    {
        _flow.SetFloat("Golden Rain", _goldenRainSlider.value);

    }

    public void AppearanceAnimation()
    {
        DOVirtual.Float(0, 3, _appearanceTime, v =>
        {
            _flow.SetFloat("Appearance", v);
            _appearanceSlider.value = v;
        }).SetEase(_appearanceCurve);
    }

    public void GoldRainAnimation()
    {
        DOVirtual.Float(0, 1, _goldenRainTime, v =>
        {
            _flow.SetFloat("Golden Rain", v);
            _goldenRainSlider.value = v;
        }).SetEase(_goldenRainCurve);
    }
}
