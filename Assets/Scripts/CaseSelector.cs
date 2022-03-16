using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseSelector : MonoBehaviour
{
    public int x;
    public int y;
    public GameDirector director;

    void Start()
    {
        if (!director) throw new System.ArgumentNullException("Director not set!");
    }

    void OnMouseDown()
    {
        director.Clicked(this);
    }
}
