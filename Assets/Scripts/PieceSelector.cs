using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public PieceType type;
    public GameDirector director;
    public Vector3 offset;

    void Start()
    {
        if (!director) throw new System.ArgumentNullException("Director not set!");
    }

    void OnMouseDown()
    {
        if (!enabled) return;
        director.Clicked(this);
    }

    void OnMouseEnter()
    {
        if (transform.parent != null)
        {
            transform.parent.GetComponent<CaseSelector>().OnMouseEnter();
        }
    }

    void OnMouseExit()
    {
        if (transform.parent != null)
        {
            transform.parent.GetComponent<CaseSelector>().OnMouseExit();
        }
    }
}
