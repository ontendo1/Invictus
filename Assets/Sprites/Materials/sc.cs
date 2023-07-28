using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : MonoBehaviour
{
    void LateUpdate()
    {
        var rotationVector = transform.rotation.eulerAngles;
        if(rotationVector.z != 0)
        {
            rotationVector.z = 0;
        }
        transform.rotation = Quaternion.Euler(rotationVector);
    }
}
