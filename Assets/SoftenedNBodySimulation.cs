 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SoftenedNBodySimulation : MonoBehaviour
{
    public GameObject starPrefab;
    public int nBodies = 30;
    public float G = 1f;
    public float deltaT = 0.01f;
    public float epsilon = 0.1f;
    private GameObject[] stars;
    private Vector3[] velocities;

    int step;

    //
    float[] kineticEnergyHistory;
    float[] potentialEnergyHistory;

    void Start()
    {
        stars = new GameObject[nBodies];
        velocities = new Vector3[nBodies];

        for (int i = 0; i < nBodies; i++)
        {
            Vector3 position = new Vector3(Random.Range(0,10f), Random.Range(0,10f), Random.Range(0,10f));
            stars[i] = Instantiate(starPrefab, position, Quaternion.identity);
            velocities[i] = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f,1f));
        }

        step = 0;
    }

    void FixedUpdate()
    {

        if (step < 1000)
        {
            step++;
        }else{
            return;
        }

        Vector3[] acceleration = ComputeAcceleration();

        for (int i = 0; i < nBodies; i++)
        {
            
            velocities[i] += acceleration[i] * deltaT;
            stars[i].transform.position += velocities[i] * deltaT;
            
        }

        
    
    }

    Vector3[] ComputeAcceleration()
    {
        Vector3[] accelerations = new Vector3[nBodies];
        for (int i = 0; i < nBodies; i++)
        {
            for (int j = 0; j < nBodies; j++)
            {
                if (i != j)
                {
                    Vector3 r = stars[j].transform.position - stars[i].transform.position;
                    float r_mag = r.magnitude;
                    accelerations[i] += G * r / Mathf.Pow(r_mag*r_mag + epsilon*epsilon, 1.5f);
                }
            }
        }

        return accelerations;
    }


    // Draw a line as the velocity indicator
    void OnDrawGizmos()
    {

        for (int i = 0; i < nBodies; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(stars[i].transform.position, stars[i].transform.position + velocities[i]);
        }
    }

    // Calculate the kinetic energy of the system
    float KineticEnergy()
    {
        float KE = 0;
        for (int i = 0; i < nBodies; i++)
        {
            KE += 0.5f * velocities[i].sqrMagnitude;
        }
        return KE;
    }


    // Calculate the potential energy of the system
    float PotentialEnergy()
    {
        float PE = 0;
        for (int i = 0; i < nBodies; i++)
        {
            for (int j = 0; j < nBodies; j++)
            {
                if (i != j)
                {
                    Vector3 r = stars[j].transform.position - stars[i].transform.position;
                    float r_mag = r.magnitude;
                    PE += -G / r_mag;
                }
            }
        }
        return PE;
    }
}