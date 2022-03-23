using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    public GameDirector director;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        if (!director) throw new System.ArgumentNullException("Director not set!");
        director.Victory += HandleVictory;
    }

    void Update()
    {
        text.text = "Player " + director.ActivePlayer + " : " + (director.IsInSelection ? "Select piece for other player" : "Select where to place piece");
    }

    void HandleVictory(int winner)
    {
        text.text = "Player " + winner + " win!!!!!!!!!!!!!!!!!!!!!!!";
        enabled = false;
    }
}
