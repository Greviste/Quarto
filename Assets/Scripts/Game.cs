using System;
using System.Collections.Generic;

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

public class Game : ICloneable
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

    public object Clone()
    {
        Game clone = new Game();
        clone.mAvailable = new HashSet<PieceType>(this.mAvailable);
        clone.mData = (PieceType[])this.mData.Clone();
        return clone;
    }

    private const int HEIGHT = 4;
    private const int WIDTH = 4;
    private PieceType[] mData;
    private HashSet<PieceType> mAvailable;
    //private HashSet<PieceType> Available => mAvailable;

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

    public bool PutPiece(int i, PieceType type)
    {
        Console.WriteLine("=======> PUT PIECE : " + i);
        Console.WriteLine("=======> PUT PIECE : " + type);
        return PutPiece(i % WIDTH, i / WIDTH, type);
    }

    public PieceType ressetPosition(int i)
    {
        PieceType res = mData[i];
        mData[i] = 0;
        return res;
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

    public bool isFinished()
    {
        if (mAvailable.Count == 0) return true;
        return CheckWin();
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

    public PieceType[] getBoardState()
    {
        return mData;
    }

    public int getHeight()
    {
        return HEIGHT;
    }
    public int getWidth()
    {
        return WIDTH;
    }

    public HashSet<PieceType> getAvailablePieces()
    {
        return mAvailable;
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


public class IA
{
    private int profondeur;
    private Game gameInstance;

    private PieceType curPiecePlayed;
    private int curPosPlayed;

    public IA(int p, Game gameInstance)
    {
        profondeur = p;
        this.gameInstance = gameInstance;
    }

    public int[] getAllGoodPos(Game g)
    {
        List<int> listgood = new List<int>();
        PieceType[] board = g.getBoardState();

        int w_b = g.getWidth();
        int h_b = g.getHeight();
        for (int i = 0; i < h_b; i++)
        {
            for (int j = 0; j < w_b; j++)
            {
                if (board[i * w_b + j] == 0)
                {
                    listgood.Add(i * w_b + j);
                }
            }
        }
        return listgood.ToArray();
    }

    private string toStringIntTab(int[] gp)
    {
        string result = "TAB{";
        int i = 0;
        for (i = 0; i < gp.Length - 1; i++)
        {
            result += gp[i] + "-";
        }
        return result + gp[i] + "}";
    }

    private string toStringAvailablePieces(HashSet<PieceType> ap)
    {
        string result = "PIECES DISPOS{";
        foreach (var val in ap)
        {
            result += val + "-";
        }
        return result + "}";
    }

    public int utilite(int cur_p, Game g)
    {
        int val = (g.CheckWin()) ? 1 : 0;
        return val;
    }

    public void playATurn(PieceType piece_a_placer)
    {
        //Game intance_jeu_IA = gameInstance.Clone();
        curPiecePlayed = 0;
        curPosPlayed = -1;
        minimax(true, gameInstance, profondeur, piece_a_placer);
    }

    public int minimax(bool max, Game g, int p, PieceType piece_a_placer)
    {
        Console.WriteLine("====== INTERATION MINIMAX ======");
        Console.WriteLine("PIECE A PLACER : " + piece_a_placer);
        Console.WriteLine("PROFONDEUR : " + p);
        Console.WriteLine("MAXIMUM : " + max);
        Console.WriteLine("ETAT DU JEU : ");
        Console.WriteLine(g.ToString());



        //On enlève ensuite la pièce à placer des pièces encore jouables
        //cloned.getAvailablePieces().Remove(piece_a_placer);

        Console.WriteLine(toStringIntTab(getAllGoodPos(g)));
        Console.WriteLine(toStringAvailablePieces(g.getAvailablePieces()));


        //Si profondeur maximale est atteinte, on s'arrète
        if (p < 0)
        {
            int ut_v = utilite(p, g);
            Console.WriteLine("=======> ON S'ARRETE : " + ut_v);
            Console.WriteLine("=======> PROFONDEUR MAX ATTEINTE");
            return ut_v;
        }

        //Si la partie se finie sur le coup précédent l'appel, inutile d'aller plus loin
        /*if(g.isFinished()){
            int ut_v = utilite(p,g);
            Console.WriteLine("=======> ON S'ARRETE : " + ut_v);
            Console.WriteLine("=======> PARTIE FINIE");
            return ut_v;
        }*/
        int val;



        if (max)
        {
            //On maximise le tour du joueur
            val = Int32.MinValue;
            int val_lue = 0;
            //Premiere etape, on place la piece
            foreach (int i in getAllGoodPos(g))
            {

                Game cloned = (Game)g.Clone();
                //cloned.getBoardState()[i] = piece_a_placer;
                cloned.PutPiece(i, piece_a_placer);

                //Si la partie se finie sur ce coup, inutile d'aller plus loin
                if (cloned.isFinished())
                {
                    int ut_v = utilite(p, cloned);
                    Console.WriteLine("=======> ON S'ARRETE : " + ut_v);
                    Console.WriteLine("=======> PARTIE FINIE");
                    return ut_v;
                }

                //Puis on choisi la prochaine piece à jouer
                foreach (PieceType choix in cloned.getAvailablePieces())
                {
                    //cloned.getAvailablePieces().Remove(choix);
                    val_lue = minimax(!max, cloned, p - 1, choix);
                    if (val_lue > val)
                    {
                        val = val_lue;
                        curPosPlayed = i;
                        curPiecePlayed = choix;
                    }
                }
                Console.WriteLine("=======> JE SORS DU FOR EACH");

                //Enfin, on reset l'état de jeu courant pour rendre la case toujours jouable
                //Console.WriteLine("ON ENLEVE : " + cloned.getBoardState()[i] + " DE : " + i);
                //cloned.getBoardState()[i] = 0;
            }
        }
        else
        {
            //On minimise le tour du joueur
            val = Int32.MaxValue;
            int val_lue = 0;
            //Premiere etape, on place la piece
            foreach (int i in getAllGoodPos(g))
            {
                //cloned.getBoardState()[i] = piece_a_placer;
                Game cloned = (Game)g.Clone();
                cloned.PutPiece(i, piece_a_placer);

                //Si la partie se finie sur ce coup, inutile d'aller plus loin
                if (cloned.isFinished())
                {
                    int ut_v = utilite(p, cloned);
                    Console.WriteLine("=======> ON S'ARRETE : " + ut_v);
                    Console.WriteLine("=======> PARTIE FINIE");
                    return ut_v;
                }

                //Puis on choisi la prochaine piece à jouer
                foreach (PieceType choix in cloned.getAvailablePieces())
                {
                    //cloned.getAvailablePieces().Remove(choix);
                    val_lue = minimax(!max, cloned, p - 1, choix);
                    if (val_lue < val)
                    {
                        val = val_lue;
                    }
                }

                Console.WriteLine("=======> JE SORS DU FOR EACH");
                //Console.WriteLine("ON ENLEVE : " + cloned.getBoardState()[i] + " DE : " + i);
                //Enfin, on reset l'état de jeu courant pour rendre la case toujours jouable
                //cloned.getBoardState()[i] = 0;
                //cloned.getAvailablePieces().Add();
            }
        }

        Console.WriteLine("====== FIN ======");
        Console.WriteLine("VALEUR PRISE : " + val);
        Console.WriteLine("CUR PIECE PLAYED : " + curPiecePlayed);
        Console.WriteLine("CUR POS PLAYED : " + curPosPlayed);
        return val;
    }

    public PieceType getPieceToPlay()
    {
        return curPiecePlayed;
    }

    public int getPositionToPlay()
    {
        return curPosPlayed;
    }
}
