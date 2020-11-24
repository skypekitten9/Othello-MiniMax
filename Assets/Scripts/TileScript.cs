using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { Black, White, Empty };
public class TileScript : MonoBehaviour
{
    TileState tileState;
    Transform pawn;
    IndexPair index;

    void Start()
    {
        pawn = gameObject.GetComponentInChildren<Transform>().Find("Pawn").transform;
        TurnTile(TileState.Empty);
    }

    public void TurnTile(TileState stateToTurn)
    {
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
