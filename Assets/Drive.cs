﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float currentSpeed = 0;

    GameObject[] getObjectsWithTag(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Robber") 
        {
            // 1 because destroy() is called before the robber is destroyed
            if (SceneManager.GetActiveScene().name == "Steering") {
                if (getObjectsWithTag("Robber").Length == 1) {
                    SceneManager.LoadScene("Scene2");
                    return;
                }
                Destroy(collision.gameObject);
            } else if (SceneManager.GetActiveScene().name == "Scene2") {
                // destroy the player
                Debug.Log("Game Over");
                Destroy(this.gameObject);
                return;
            }
        }
    }


    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);
        currentSpeed = translation;

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
    }
}
