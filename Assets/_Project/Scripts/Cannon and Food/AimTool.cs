using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AimTool : MonoBehaviour
{
    // Start is called before the first frame update
    public float ThetaScale = 0.01f;
    public float radius = 3f;
    private int Size;
    private LineRenderer lineRenderer;
    private float Theta = 0f;
    private float i = 0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        
        DrawCircle(Size, i);
        i+=0.1f;
        if (i > 50)
            i = 0;
        //lineRenderer.positionCount =Size;
        //for (int i = 0; i < Size; i++)
        //{
        //    Theta += (2.0f * Mathf.PI * ThetaScale);
        //    float x = radius * Mathf.Cos(Theta);
        //    float y = radius * Mathf.Sin(Theta);
        //    lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        //}
    }

    void DrawCircle(int steps, float radius) {
        
        
        lineRenderer.positionCount = steps;
        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            //float circumferenceProgress = (float)currentStep / steps;
            //float currentRadian = circumferenceProgress * 2.0f * Mathf.PI;
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float xScaled = Mathf.Cos(Theta);
            float yScaled = Mathf.Sin(Theta);
            float x = radius * xScaled;
            float y = radius * yScaled;

            lineRenderer.SetPosition(currentStep, new Vector3(x, 0, y));

        }
    }
}
