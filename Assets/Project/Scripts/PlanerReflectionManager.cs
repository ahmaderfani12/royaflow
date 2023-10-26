using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanerReflectionManager : MonoBehaviour
{
    [SerializeField] GameObject m_reflectionPlane;
    [SerializeField] Camera m_mainCamera;
    [SerializeField] Camera m_reflectionCamera;
    [SerializeField] Material m_groundMat;
    public RenderTexture m_renderTarget;

    private void Start()
    {
        m_renderTarget = new RenderTexture(Screen.width, Screen.height, 16);
        m_groundMat.SetTexture("_MainText", m_renderTarget);
    }

    void Reflection()
    {
        Vector3 cameraDirectionWordSpace = m_mainCamera.transform.forward;
        Vector3 cameraUpWorldSpace = m_mainCamera.transform.up;
        Vector3 cameraPositionWorldSpace = m_mainCamera.transform.position;

        // Transform to the floor space
        Vector3 cameraDirectionPlaneSpace = m_reflectionPlane.transform.InverseTransformDirection(cameraDirectionWordSpace);
        Vector3 cameraUpPlaneSpace = m_reflectionPlane.transform.InverseTransformDirection(cameraUpWorldSpace);
        Vector3 cameraPositionPlaneSpace = m_reflectionPlane.transform.InverseTransformPoint(cameraPositionWorldSpace);

        cameraDirectionPlaneSpace.y *= -1.0f;
        cameraUpPlaneSpace.y *= -1.0f;
        cameraPositionPlaneSpace.y *= -1.0f;

        // Transform back to world space
        cameraDirectionWordSpace = m_reflectionPlane.transform.TransformDirection(cameraDirectionPlaneSpace);
        cameraUpWorldSpace = m_reflectionPlane.transform.TransformDirection(cameraUpPlaneSpace);
        cameraPositionWorldSpace = m_reflectionPlane.transform.TransformPoint(cameraPositionPlaneSpace);

        m_reflectionCamera.transform.position = cameraPositionWorldSpace;
        m_reflectionCamera.transform.LookAt(cameraPositionWorldSpace + cameraDirectionWordSpace, cameraUpWorldSpace);


        m_reflectionCamera.targetTexture = m_renderTarget;
        m_reflectionCamera.Render();
    }

    private void LateUpdate()
    {
        Reflection();
    }
}
