using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum PieceFlags
{
    White = 1,
    Black = 2,
    Empty = 4,
    Full = 8,
    Small = 16,
    Tall = 32,
    Round = 64,
    Square = 128
}

public enum PieceType
{
    Wesr = PieceFlags.White | PieceFlags.Empty | PieceFlags.Small | PieceFlags.Round,
    Wess = PieceFlags.White | PieceFlags.Empty | PieceFlags.Small | PieceFlags.Square,
    Wetr = PieceFlags.White | PieceFlags.Empty | PieceFlags.Tall | PieceFlags.Round,
    Wets = PieceFlags.White | PieceFlags.Empty | PieceFlags.Tall | PieceFlags.Square,
    Wfsr = PieceFlags.White | PieceFlags.Full | PieceFlags.Small | PieceFlags.Round,
    Wfss = PieceFlags.White | PieceFlags.Full | PieceFlags.Small | PieceFlags.Square,
    Wftr = PieceFlags.White | PieceFlags.Full | PieceFlags.Tall | PieceFlags.Round,
    Wfts = PieceFlags.White | PieceFlags.Full | PieceFlags.Tall | PieceFlags.Square,
    Besr = PieceFlags.Black | PieceFlags.Empty | PieceFlags.Small | PieceFlags.Round,
    Bess = PieceFlags.Black | PieceFlags.Empty | PieceFlags.Small | PieceFlags.Square,
    Betr = PieceFlags.Black | PieceFlags.Empty | PieceFlags.Tall | PieceFlags.Round,
    Bets = PieceFlags.Black | PieceFlags.Empty | PieceFlags.Tall | PieceFlags.Square,
    Bfsr = PieceFlags.Black | PieceFlags.Full | PieceFlags.Small | PieceFlags.Round,
    Bfss = PieceFlags.Black | PieceFlags.Full | PieceFlags.Small | PieceFlags.Square,
    Bftr = PieceFlags.Black | PieceFlags.Full | PieceFlags.Tall | PieceFlags.Round,
    Bfts = PieceFlags.Black | PieceFlags.Full | PieceFlags.Tall | PieceFlags.Square
}

public class PieceSelector : MonoBehaviour
{
    public PieceType type;
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
