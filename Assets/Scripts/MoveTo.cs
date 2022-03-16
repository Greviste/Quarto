using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    Vector3 depart;
    public Vector3 destination;
    float _duration;
    float lerpVal = 0;
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

    void Start()
    {
        depart = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lerpVal += Time.deltaTime / duration;
        transform.position = Vector3.Lerp(depart, destination, lerpVal);
        if (lerpVal >= 1) Destroy(this);
    }
}
