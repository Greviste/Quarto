using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
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
    None = 0,
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

public class Game
{
    private static readonly int[] LINES = {
        // LINES
         0,  1,  2,  3,
         4,  5,  6,  7,
         8,  9, 10, 11,
        12, 13, 14, 15,
        // COLUMNS
         0,  4,  8, 12,
         1,  5,  9, 13,
         2,  6, 10, 14,
         3,  7, 11, 15,
        // DIAGONAL
         0,  5, 10, 15,
         3,  6,  9, 12
    };

    private const int HEIGHT = 4;
    private const int WIDTH = 4;
    private PieceType[] mData;
    private HashSet<PieceType> mAvailable;
    private HashSet<PieceType> Available => mAvailable;

    public Game()
    {
        mData = new PieceType[HEIGHT * WIDTH];
        // ensure 0 default value
        for (int i = 0; i < HEIGHT * WIDTH; i++) mData[i] = 0;

        mAvailable = new HashSet<PieceType>();
        foreach (PieceType i in Enum.GetValues(typeof(PieceType))) mAvailable.Add(i);
    }

    public bool PutPiece(int x, int y, PieceType type)
    {
        if (!mAvailable.Contains(type))
        {
            return false;
        }

        if (mData[y * WIDTH + x] == 0)
        {
            mData[y * WIDTH + x] = type;
            mAvailable.Remove(type);
            return true;
        }

        return false;
    }



    public bool CheckWin()
    {
        for (int i = 0; i < LINES.Length; i += 4)
        {
            int v0 = (int)mData[LINES[i]];
            int v1 = (int)mData[LINES[i + 1]];
            int v2 = (int)mData[LINES[i + 2]];
            int v3 = (int)mData[LINES[i + 3]];

            int total = v0 & v1 & v2 & v3;

            if (total > 0) return true;
        }

        return false;
    }

    public string AvailableString()
    {
        string result = "";

        foreach (PieceType t in mAvailable)
        {
            result += t + " ";
        }

        return result;
    }

    public override string ToString()
    {
        string result = "";

        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                result += string.Format("{0, 5}", mData[i * WIDTH + j]);
            }
            result += "\n";
        }

        return result;
    }
}