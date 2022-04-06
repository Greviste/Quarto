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
    }

    public void EnableAi()
    {
        Debug.Log("Enable AI here");
    }

    public void Clicked(CaseSelector c)
    {
        if (locked) return;
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
        if (selectedPiece) return;

        selectedPiece = p;
        player = 1 - player;

        MoveTo mt = p.gameObject.AddComponent<MoveTo>();
        mt.duration = 0.1f;
        mt.destination = p.transform.position + Vector3.up * 0.1f;
        selectedPiece.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
