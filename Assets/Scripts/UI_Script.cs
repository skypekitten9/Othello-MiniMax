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

    public void DisplayWin(TileState winColor)
    {
        gameObject.transform.Find("BLOCK").GetComponent<Image>().enabled = true;
        gameObject.transform.Find("Blackout").GetComponent<Image>().enabled = true;
        gameObject.transform.Find("Blackout").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        gameObject.transform.Find("Blackout").GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        gameObject.transform.Find("Return").GetComponent<Image>().enabled = true;
        gameObject.transform.Find("Return").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0.1f);
        gameObject.transform.Find("Return").GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
        switch (winColor)
        {
            case TileState.Black:
                gameObject.transform.Find("BlackWins").GetComponent<Image>().enabled = true;
                gameObject.transform.Find("BlackWins").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0.1f);
                gameObject.transform.Find("BlackWins").GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
                break;
            case TileState.White:
                gameObject.transform.Find("WhiteWins").GetComponent<Image>().enabled = true;
                gameObject.transform.Find("WhiteWins").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0.1f);
                gameObject.transform.Find("WhiteWins").GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
                break;
            case TileState.Empty:
                gameObject.transform.Find("Draw").GetComponent<Image>().enabled = true;
                gameObject.transform.Find("Draw").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0.1f);
                gameObject.transform.Find("Draw").GetComponent<Image>().CrossFadeAlpha(1f, 1f, false);
                break;
            default:
                Debug.LogError("Win can only be displayed to white or black or empty.");
                break;
        }
    }

    public void HideWin()
    {
        gameObject.transform.Find("BLOCK").GetComponent<Image>().enabled = false;
        gameObject.transform.Find("Blackout").GetComponent<Image>().CrossFadeAlpha(0f, 1f, false);
        gameObject.transform.Find("Return").GetComponent<Image>().enabled = false;
        gameObject.transform.Find("BlackWins").GetComponent<Image>().enabled = false;
        gameObject.transform.Find("WhiteWins").GetComponent<Image>().enabled = false;
        gameObject.transform.Find("Draw").GetComponent<Image>().enabled = false;

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
        //gameObject.transform.Find("BlacksTurn").transform.position = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(topWorldPosition);
        //gameObject.transform.Find("BlacksTurn").transform.position = Camera.main.WorldToScreenPoint(topWorldPosition);
        //gameObject.transform.Find("WhitesTurn").transform.position = Camera.main.WorldToScreenPoint(topWorldPosition);

        //gameObject.transform.Find("PlayerVsAgent").transform.position = Camera.main.WorldToScreenPoint(bottomWorldPosition);
        //gameObject.transform.Find("PlayerVsPlayer").transform.position = Camera.main.WorldToScreenPoint(leftWorldPosition);
        //gameObject.transform.Find("AgentVsAgent").transform.position = Camera.main.WorldToScreenPoint(rightWorldPosition);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
