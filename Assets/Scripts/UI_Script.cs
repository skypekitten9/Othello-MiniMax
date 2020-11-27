using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void DisplayTurn(TileState turnColor)
    {
        switch (turnColor)
        {
            case TileState.Black:
                gameObject.transform.Find("BlacksTurn").GetComponent<Image>().enabled = true;
                gameObject.transform.Find("WhitesTurn").GetComponent<Image>().enabled = false;
                break;
            case TileState.White:
                gameObject.transform.Find("BlacksTurn").GetComponent<Image>().enabled = false;
                gameObject.transform.Find("WhitesTurn").GetComponent<Image>().enabled = true;
                break;
            default:
                Debug.LogError("Turn can only be changed to white or black.");
                break;
        }
    }

    public void SetRefferences(Vector3 topWorldPosition, Vector3 bottomWorldPosition, Vector3 leftWorldPosition, Vector3 rightWorldPosition)
    {
        gameObject.transform.Find("BlacksTurn").transform.position = Camera.main.WorldToScreenPoint(topWorldPosition);
        gameObject.transform.Find("WhitesTurn").transform.position = Camera.main.WorldToScreenPoint(topWorldPosition);

        gameObject.transform.Find("PlayerVsAgent").transform.position = Camera.main.WorldToScreenPoint(bottomWorldPosition);
        gameObject.transform.Find("PlayerVsPlayer").transform.position = Camera.main.WorldToScreenPoint(leftWorldPosition);
        gameObject.transform.Find("AgentVsAgent").transform.position = Camera.main.WorldToScreenPoint(rightWorldPosition);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
