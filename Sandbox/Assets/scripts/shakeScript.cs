using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeScript : MonoBehaviour
{

    [SerializeField] private float shakeMagnitude = 0.1f;
    private GameObject objectToShake;

    private Vector3 originalPosition;
    private float shakeTimer;
    private bool shaking = false;


    private void Update()
    {
        if (shaking == true)
        {
            if (shakeTimer > 0)
            {
                // Shake the object
                objectToShake.transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;

                // Decrease the timer
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Shake Done");
                objectToShake.transform.position = originalPosition;
                shaking = false;
                //Destroy(objectToShake);
                //GetComponent<gravNodeScript>().initGravNodeList();
            }
        }
        
    }

    public void shakeObj(GameObject objectToShake, float shakeDuration)
    {
        this.objectToShake = objectToShake;
        originalPosition = objectToShake.transform.position;
        if (objectToShake.transform != null)
        {
            shakeTimer = shakeDuration;
            shaking = true;
        }
    }
}
