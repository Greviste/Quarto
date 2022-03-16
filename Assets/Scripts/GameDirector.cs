using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public PieceSelector selectedPiece = null;

    public void Clicked(CaseSelector c)
    {
        if (!selectedPiece) return;
        MoveTo mt = selectedPiece.gameObject.AddComponent<MoveTo>();
        mt.duration = 1;
        mt.destination = c.transform.position + selectedPiece.offset;
        selectedPiece.enabled = false;
        selectedPiece = null;
    }

    public void Clicked(PieceSelector p)
    {
        if (!selectedPiece) selectedPiece = p;
    }
}
