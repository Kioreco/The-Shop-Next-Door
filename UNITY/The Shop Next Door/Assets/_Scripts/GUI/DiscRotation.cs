using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
