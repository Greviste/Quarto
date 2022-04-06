using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public enum CoordSystem
    {
        LOCAL,
        GLOBAL
    };

    Vector3 depart;
    Vector3 localDepart;
    public Vector3 destination;
    float _duration;
    float lerpVal = 0;
    CoordSystem mSystem = CoordSystem.GLOBAL;
    public float duration
    {
        get
        {
            return _duration;
        }
        set
        {
            _duration = value;
            lerpVal = 0;
        }
    }

    public void SetCoordSystem(CoordSystem system)
    {
        mSystem = system;
    }

    void Start()
    {
        depart = transform.position;
        localDepart = transform.localPosition;
    }

    public void Reset()
    {
        depart = transform.position;
        localDepart = transform.localPosition;
        lerpVal = 0.0f;
    }

    void Update()
    {
        lerpVal += Time.deltaTime / duration;
        switch (mSystem)
        {
            case CoordSystem.LOCAL:
                transform.localPosition = Vector3.Lerp(localDepart, destination, lerpVal);
                break;
            case CoordSystem.GLOBAL:
                transform.position = Vector3.Lerp(depart, destination, lerpVal);
                break;
        }
        if (lerpVal >= 1) Destroy(this);
    }
}
