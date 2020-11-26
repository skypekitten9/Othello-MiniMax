using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Script : MonoBehaviour
{
    private static UI_Script instance = null;
    public static UI_Script Instance { get { return instance; } }

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void DisplayWin(TileState winColor)
    {

    }

    void HideWin()
    {

    }

    void DisplayTurn(TileState turnColor)
    {

    }

    void SetRefferences(Vector3 topPosition, Vector3 bottomPosition, Vector3 leftPosition, Vector3 rightPosition)
    {

    }

    void ExitGame()
    {

    }
}
