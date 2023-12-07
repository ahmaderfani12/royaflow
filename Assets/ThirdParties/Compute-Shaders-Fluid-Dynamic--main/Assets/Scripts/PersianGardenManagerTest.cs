using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PersianGardenManagerTest : MonoBehaviour, IFluidManager
{

    // ------------------------------------------------------------------
    // VARIABLES

    //___________
    // public
    public FluidSimulater fluid_simulater;

    [Header("Fountain Managment")]
    public Transform  Tran_downLeft;
    public Transform  Tran_upRight;
    public GameObject Fountain;
    public Texture2D  boundaryTexture; 

    //___________
    // private

    private FluidGPUResources resources;

    public RenderTexture     pressure_texture;
    public RenderTexture     velocity_texture;

    private Camera            main_cam;

    // debug
    public Material VelocityDebugMat;
    public Material PressureDebugMat;
    public Material DyeDebugMat;
    public Vector4 MousePosDebug;      // real hit pos, simulation hit pos

    // ------------------------------------------------------------------
    // INITALISATION
    void Start()
    {

        main_cam = Camera.main;
        if (main_cam == null) Debug.LogError("Could not find main camera, make sure the camera is tagged as main");
  

        //--
        // Initialize the fluid simulator engine
        fluid_simulater.Initialize();
        resources = new FluidGPUResources(fluid_simulater);
        resources.Create();

        //fluid_simulater.SubmitMousePosOverrideDelegate(GetMousePosInSimulationSpaceUnitValue);

        #region Create Velocity and pressure texture

        
        // Create textures for visualizing presure or velocity
        pressure_texture = new RenderTexture((int)fluid_simulater.canvas_dimension, (int)fluid_simulater.canvas_dimension, 0)
        {
            enableRandomWrite = true,
            useMipMap         = true,
             graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R32_SFloat,
            filterMode        = FilterMode.Trilinear,
            anisoLevel        = 7,
            format            = RenderTextureFormat.RFloat,
            wrapMode          = TextureWrapMode.Clamp,
            
        };
        pressure_texture.Create();
        velocity_texture = new RenderTexture((int)fluid_simulater.canvas_dimension, (int)fluid_simulater.canvas_dimension, 0)
        {
            enableRandomWrite = true,
            useMipMap = true,
            graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R32G32_SFloat,
            filterMode = FilterMode.Trilinear,
            anisoLevel = 7,
            format = RenderTextureFormat.RGFloat,
            wrapMode = TextureWrapMode.Clamp,
        };
        velocity_texture.Create();

        #endregion

        VelocityDebugMat.SetTexture("_MainTex", velocity_texture);
        PressureDebugMat.SetTexture("_MainTex", pressure_texture);
        DyeDebugMat.SetTexture("_MainTex", fluid_simulater.visulasation_texture);

        //--
        // Build the Fluid Pipeline
        fluid_simulater.UpdateArbitaryBoundaryOffsets(boundaryTexture, resources);


        //Vector2 waterpipePosition = new Vector2(fluid_simulater.simulation_dimension / 2, fluid_simulater.simulation_dimension - fluid_simulater.simulation_dimension * 0.1f);
        //Vector2 waterPipeDirection = new Vector2(0.0f, -1.0f);
        //fluid_simulater.AddConstantForceSource(resources.velocity_buffer, waterpipePosition,
        //                                waterPipeDirection, 6.4f, fluid_simulater.simulation_dimension * 0.0025f, fluid_simulater.simulation_dimension * 0.001f);


        fluid_simulater.AddUserForce           (resources.velocity_buffer                                   );

        fluid_simulater.HandleCornerBoundaries (resources.velocity_buffer, FieldType.Velocity);
        fluid_simulater.HandleArbitaryBoundary (resources.velocity_buffer, resources.boundary_velocity_offset_buffer, FieldType.Velocity);
        fluid_simulater.Project                (resources.velocity_buffer, resources.divergence_buffer, resources.pressure_buffer, resources.boundary_pressure_offset_buffer);
        fluid_simulater.Diffuse                (resources.velocity_buffer                                   );
        fluid_simulater.HandleCornerBoundaries (resources.velocity_buffer, FieldType.Velocity               );
        fluid_simulater.HandleArbitaryBoundary (resources.velocity_buffer, resources.boundary_velocity_offset_buffer, FieldType.Velocity);
        fluid_simulater.Project                (resources.velocity_buffer, resources.divergence_buffer, resources.pressure_buffer, resources.boundary_pressure_offset_buffer);
        fluid_simulater.Advect                 (resources.velocity_buffer, resources.velocity_buffer, fluid_simulater.velocity_dissapation);
        fluid_simulater.HandleCornerBoundaries (resources.velocity_buffer, FieldType.Velocity               );
        fluid_simulater.HandleArbitaryBoundary (resources.velocity_buffer, resources.boundary_velocity_offset_buffer, FieldType.Velocity);
        fluid_simulater.Project                (resources.velocity_buffer, resources.divergence_buffer, resources.pressure_buffer, resources.boundary_pressure_offset_buffer);

        // Dye
        fluid_simulater.AddDye(resources.dye_buffer);
        fluid_simulater.Advect(resources.dye_buffer, resources.velocity_buffer, 0.992f);
        fluid_simulater.HandleCornerBoundaries(resources.dye_buffer, FieldType.Dye);
        fluid_simulater.Diffuse(resources.dye_buffer);
        fluid_simulater.HandleCornerBoundaries(resources.dye_buffer, FieldType.Dye);

        fluid_simulater.Visualiuse(resources.dye_buffer, overrideOnCamera:false);

        // End Dye

        fluid_simulater.CopyPressureBufferToTexture(pressure_texture, resources.pressure_buffer);
        fluid_simulater.CopyVelocityBufferToTexture(velocity_texture, resources.velocity_buffer);

        fluid_simulater.BindCommandBuffer();


    }

    // ------------------------------------------------------------------
    // DESTRUCTOR
    void OnDisable()
    {
        fluid_simulater.Release();
        resources      .Release();
    }

    // ------------------------------------------------------------------
    // LOOP
    void Update()
    {
        fluid_simulater.Tick(Time.deltaTime);

    }

    // ------------------------------------------------------------------
    // FUNCTIONS

    Vector2 GetMousePosInSimulationSpaceUnitValue(ref bool isInBound)
    {
        RaycastHit results;
        Ray ray = main_cam.ScreenPointToRay(Input.mousePosition);

        isInBound = false;
        if (Physics.Raycast(ray, out results, 100.0f))
        {

            if (results.collider.gameObject != Fountain) return new Vector2(-20.0f, -20.0f);

            Vector2 hitPositionInSimulationSpace = new Vector2(results.point.z, results.point.x);
                    hitPositionInSimulationSpace = hitPositionInSimulationSpace - new Vector2(Tran_downLeft.position.z, Tran_downLeft.position.x);

            Vector2 span = new Vector2(Tran_upRight.position.z, Tran_upRight.position.x) - new Vector2(Tran_downLeft.position.z, Tran_downLeft.position.x);

             hitPositionInSimulationSpace = new Vector2(hitPositionInSimulationSpace.x / Mathf.Abs(span.x), hitPositionInSimulationSpace.y / Mathf.Abs(span.y));
            isInBound = true;

            MousePosDebug = new Vector4(results.point.z, results.point.x, hitPositionInSimulationSpace.x, hitPositionInSimulationSpace.y);

            return hitPositionInSimulationSpace;

        }

        return new Vector2(-20.0f, -20.0f);
    }

    public RenderTexture GetVelocity2D()
    {
        return velocity_texture;
    }

    public RenderTexture GetPressure2D()
    {
        return pressure_texture;
    }

    public RenderTexture GetDye2D()
    {
        return fluid_simulater.visulasation_texture;
    }
}
