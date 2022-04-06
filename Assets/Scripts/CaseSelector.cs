using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseSelector : MonoBehaviour
{
    public int x;
    public int y;
    public GameDirector director;

    public Vector3 mouseOverOffset = new Vector3(0.0f, 0.1f, 0.0f);
    private Vector3 mStartPosition;
    private Vector3 mOffsetPosition;

    void Start()
    {
        if (!director) throw new System.ArgumentNullException("Director not set!");


        mStartPosition = transform.localPosition;
        mOffsetPosition = transform.localPosition + mouseOverOffset;
    }

    void OnMouseDown()
    {
        director.Clicked(this);
    }

    public void OnMouseEnter()
    {
        MoveTo mt = gameObject.GetComponent<MoveTo>();
        if (mt != null)
        {
            mt.SetCoordSystem(MoveTo.CoordSystem.LOCAL);
            mt.Reset();
            mt.destination = mOffsetPosition;
            mt.duration = 0.25f;
        }
        else
        {
            mt = gameObject.AddComponent<MoveTo>();
            mt.SetCoordSystem(MoveTo.CoordSystem.LOCAL);
            mt.destination = mOffsetPosition;
            mt.duration = 0.25f;
        }
    }

    public void OnMouseExit()
    {
        MoveTo mt = gameObject.GetComponent<MoveTo>();
        if (mt != null)
        {
            mt.SetCoordSystem(MoveTo.CoordSystem.LOCAL);
            mt.Reset();
            mt.destination = mStartPosition;
            mt.duration = 0.25f;
        }
        else
        {
            mt = gameObject.AddComponent<MoveTo>();
            mt.SetCoordSystem(MoveTo.CoordSystem.LOCAL);
            mt.destination = mStartPosition;
            mt.duration = 0.25f;
        }
    }
}
