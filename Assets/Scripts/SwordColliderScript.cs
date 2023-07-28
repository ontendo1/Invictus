using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliderScript : MonoBehaviour
{
    GameObject _grid;
    float rotOff;
    float _gridEuler;
    bool surfArc;
    PlatformEffector2D pE;
    void Start()
    {
        _grid = GameObject.Find("Grid");
        _gridEuler = _grid.transform.eulerAngles.z;
        pE = GetComponent<PlatformEffector2D>();
        rotOff = pE.rotationalOffset;
    }
    public void rotateOffset()
    {
        if (rotOff < 260f)
        {
            rotOff += 90f;
            if (rotOff != 80f && rotOff != 260f)
            pE.rotationalOffset = rotOff;
        }
        else
        {
            rotOff = -10f;
            pE.rotationalOffset = rotOff;

        }
        surfArc = (rotOff != 80f && rotOff != 260f);
        pE.surfaceArc = surfArc ? 185 : 0; 
    }
}
