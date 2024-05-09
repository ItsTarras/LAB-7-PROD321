﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/* This class provides some basic camera controller
 * functionality using the keyboard and mouse
 * 
 * PROD321 - Interactive Computer Graphics and Animation 
 * Copyright 2023, University of Canterbury
 * Written by Adrian Clark
 */


public class BasicCameraController : MonoBehaviour
{
    // Reference to the Frustum Cull Script
    private List<FrustumCull> frustumCulls = new List<FrustumCull>();

    // Reference to the Occlusion Cull Script
    public OcclusionFrustumCulling occlusionCull;

    private List<GameObject> enemies = new List<GameObject>();

    // Define the speed of movement and rotation
    public float moveSpeed = 10;
    public float turnSpeed = 1500;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy);
        }

        Debug.Log(enemyObjects.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get the horizontal and vertical values for movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Get the mouse movement in X axis
        float inputYaw = Input.GetAxisRaw("Mouse X");

        // Update the position of the Game Object this script is attached to
        // Multiply the vertical movement amount by the transforms forward vector
        // and multiply that by the move speed multiplied by the amount of time
        // elapsed since the last frame (Time.deltaTime). Do the same for
        // the horizontal movement, but using the transform's right vector
        Vector3 newDirection = (transform.forward * v * Time.deltaTime * moveSpeed) + (transform.right * h * Time.deltaTime * moveSpeed);
        rb.AddForce(newDirection * moveSpeed * Time.deltaTime);

        // Rotate the transform around it's up vector based on the mouse movement
        // in the horizontal direction, multiplied by the deltaTime and turn speed.
        transform.Rotate(Vector3.up, inputYaw * Time.deltaTime * turnSpeed);


        foreach (GameObject enemy in enemies)
        {

            FrustumCull frustumCull = enemy.GetComponentInChildren<FrustumCull>();
            Debug.Log(!(frustumCull == null));

            Debug.Log(occlusionCull);
            //If it's in both, kill character.
            if (frustumCull.gameObjectsInFrustum.Contains(this.gameObject) && occlusionCull.gameObjectsNotOccluded.Contains(this.gameObject))
            {
                Debug.Log("FOUND YOU!");
            }
            //If it's only in the frustumCull, set its colour to magenta.
            else if (frustumCull.gameObjectsInFrustum.Contains(this.gameObject))
            {
                Debug.Log("Blocked by wall");
            }
            //If it's only in the occlusionCull
            else if (occlusionCull.gameObjectsNotOccluded.Contains(this.gameObject))
            {
                Debug.Log("Can't see him.");
            }
            else if (!frustumCull.gameObjectsInFrustum.Contains(this.gameObject) && !occlusionCull.gameObjectsNotOccluded.Contains(this.gameObject))
            {
                Debug.Log("Can't see you.");
            }
        }
    }

    public Quaternion getRotation()
    {
        return transform.rotation;
    }
}