using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }

public class GameDirector : MonoBehaviour
{
    private PieceSelector selectedPiece = null;
    private Game game = new Game();
    private IA ia = null;
    private int iaPlayer = -1;
    private Thread iaThread = null;
    int player = 0;
    bool locked = true;
    public IntEvent Victory = new IntEvent();

    public int ActivePlayer
    {
        get { return player + 1; }
    }

    public bool IsInSelection
    {
        get { return selectedPiece == null; }
    }

    public void Unlock()
    {
        locked = false;
        if(player == iaPlayer)
        {
            var pieces = FindObjectsOfType<PieceSelector>();
            HandlePieceClick(pieces[Random.Range(0, pieces.Length)]);
        }
    }

    public void EnableAi()
    {
        ia = new IA(3, game);
        iaPlayer = Random.Range(0,2);
    }

    public void Clicked(CaseSelector c)
    {
        if (locked) return;
        if (player == iaPlayer) return;
        HandleCaseClick(c);
    }

    private void HandleCaseClick(CaseSelector c)
    {
        if (!selectedPiece) return;
        if (!game.PutPiece(c.x, c.y, selectedPiece.type)) return;
        MoveTo mt = selectedPiece.gameObject.AddComponent<MoveTo>();
        mt.duration = 1;
        mt.destination = c.transform.position + selectedPiece.offset;
        selectedPiece = null;
        if (game.CheckWin())
        {
            Victory.Invoke(ActivePlayer);
            locked = true;
        }
    }

    public void Clicked(PieceSelector p)
    {
        if (locked) return;
        if (player == iaPlayer) return;
        HandlePieceClick(p);
    }

    private void HandlePieceClick(PieceSelector p)
    {
        if (selectedPiece) return;
        MoveTo mt = p.gameObject.AddComponent<MoveTo>();
        mt.duration = 0.1f;
        mt.destination = p.transform.position + Vector3.up * 0.1f;
        selectedPiece = p;
        selectedPiece.enabled = false;
        player = 1 - player;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RunAi()
    {
        ia.playATurn(selectedPiece.type);
    }

    void Update()
    {
        if(player == iaPlayer && selectedPiece)
        {
            if (iaThread == null) iaThread = new Thread(RunAi);
            if (!iaThread.IsAlive)
            {
                iaThread = null;
                int x = ia.getPositionToPlay() % 4;
                int y = ia.getPositionToPlay() / 4;
                foreach (CaseSelector c in FindObjectsOfType<CaseSelector>())
                {
                    if (c.x == x && c.y == y)
                    {
                        HandleCaseClick(c);
                        break;
                    }
                }
                foreach (PieceSelector p in FindObjectsOfType<PieceSelector>())
                {
                    if (p.type == ia.getPieceToPlay())
                    {
                        HandlePieceClick(p);
                        break;
                    }
                }
            }
        }
    }
}
