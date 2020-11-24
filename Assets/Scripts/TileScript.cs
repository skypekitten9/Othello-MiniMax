using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { Black, White, Empty };
public class TileScript : MonoBehaviour
{
    TileState tileState;
    Animator animator;
    Transform pawn;
    IndexPair index;

    void Start()
    {
        pawn = gameObject.GetComponentInChildren<Transform>().Find("Pawn").transform;
        animator = gameObject.GetComponentInChildren<Transform>().Find("Pawn").GetComponent<Animator>();
        PlaceTile(TileState.Empty);
    }

    private void Update()
    {
        //Debug to test animation
        if(Input.GetKeyDown(KeyCode.P) && tileState == TileState.White)
        {
            TurnTile(TileState.Black);
        }

        //Debug to test animation
        if (Input.GetKeyDown(KeyCode.O) && tileState == TileState.Black)
        {
            TurnTile(TileState.White);
        }
    }

    public void TurnTile(TileState stateToTurn)
    {
        animator.enabled = true;
        pawn.gameObject.SetActive(true);
        switch (stateToTurn)
        {
            case TileState.Black:
                animator.SetBool("flipToWhite", false);
                animator.SetBool("flipToBlack", true);
                pawn.rotation = new Quaternion(180, 0, 0, 1);
                break;
            case TileState.White:
                animator.SetBool("flipToBlack", false);
                animator.SetBool("flipToWhite", true);
                pawn.rotation = Quaternion.identity;
                break;
            case TileState.Empty:
                pawn.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        tileState = stateToTurn;
    }

    public void PlaceTile(TileState stateToTurn)
    {
        animator.enabled = false;
        pawn.gameObject.SetActive(true);
        switch (stateToTurn)
        {
            case TileState.Black:
                pawn.rotation = new Quaternion(180, 0, 0, 1);
                break;
            case TileState.White:
                pawn.rotation = Quaternion.identity;
                break;
            case TileState.Empty:
                pawn.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        tileState = stateToTurn;
    }

    public void SetIndex(int z, int x)
    {
        index = new IndexPair(z, x);
    }

    public IndexPair GetIndex()
    {
        return index;
    }

    IEnumerator TestStates()
    {
        TurnTile(TileState.Black);
        yield return new WaitForSeconds(1f);
        TurnTile(TileState.White);
        yield return new WaitForSeconds(1f);
        TurnTile(TileState.Empty);
        yield return new WaitForSeconds(1f);
        TurnTile(TileState.White);
        yield return new WaitForSeconds(1f);
    }
}
