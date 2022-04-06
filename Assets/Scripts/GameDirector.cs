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
    public PieceSelector selectedPiece = null;
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
        if (player == iaPlayer)
        {
            var pieces = FindObjectsOfType<PieceSelector>();
            HandlePieceClick(pieces[Random.Range(0, pieces.Length)]);
        }
    }

    public void EnableAi()
    {
        ia = new IA(3, game);
        iaPlayer = Random.Range(0, 2);
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

        // MoveTo mt = selectedPiece.gameObject.AddComponent<MoveTo>();
        selectedPiece.gameObject.transform.SetParent(c.transform);
        MoveTo mt = selectedPiece.gameObject.AddComponent<MoveTo>();
        mt.SetCoordSystem(MoveTo.CoordSystem.LOCAL);
        mt.duration = 1;

        Vector3 scale = c.transform.localScale;
        Vector3 targetPos = Vector3.Scale(selectedPiece.offset, new Vector3(1.0f / scale.x, 1.0f / scale.y, 1.0f / scale.z));
        mt.destination = targetPos;
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
        mt.destination = p.transform.position + Vector3.up * 0.3f;
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
        if (player == iaPlayer && selectedPiece)
        {
            if (iaThread == null)
            {
                iaThread = new Thread(RunAi);
                iaThread.Start();
            }
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
                if (selectedPiece != null)
                {
                    Debug.LogWarning("No proper case found! Using fallback.");
                    foreach (CaseSelector c in FindObjectsOfType<CaseSelector>())
                    {
                        HandleCaseClick(c);
                        if (selectedPiece == null) break;
                    }
                }
                foreach (PieceSelector p in FindObjectsOfType<PieceSelector>())
                {
                    if (p.enabled && p.type == ia.getPieceToPlay())
                    {
                        HandlePieceClick(p);
                        break;
                    }
                }
                if (selectedPiece == null)
                {
                    Debug.LogWarning("No proper piece found! Using fallback.");
                    foreach (PieceSelector p in FindObjectsOfType<PieceSelector>())
                    {
                        if (p.enabled)
                        {
                            HandlePieceClick(p);
                            break;
                        }
                    }
                }
            }
        }
    }
}
