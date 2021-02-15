using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    [SerializeField] private Vector3 V_start;
    [SerializeField] private float M;
    [SerializeField] private float G;
    [SerializeField] private Vector3 R;
    
    private const float TOLERANCE = 1000f;
    void FixedUpdate()
    {
        float distance = (R - transform.position).magnitude;

        Vector3 a = G * M * (R - transform.position).normalized / (distance * distance);

        if (a.magnitude > TOLERANCE)
        {
            return;
        }
        
        Vector3 delta = V_start * Time.fixedDeltaTime + a * (Time.fixedDeltaTime * Time.fixedDeltaTime) / 2;
        transform.Translate(delta);
        V_start += a * Time.fixedDeltaTime;  
    }
}
