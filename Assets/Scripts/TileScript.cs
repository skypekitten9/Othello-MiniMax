using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { Black, White, Empty };
public class TileScript : MonoBehaviour
{
    public Material highlightMaterial;
    Material groundMaterial;
    TileState tileState;
    Animator animator;
    Transform pawn;
    Renderer groundRenderer;
    IndexPair index;

    void Awake()
    {
        pawn = gameObject.GetComponentInChildren<Transform>().Find("Pawn").transform;
        groundRenderer = gameObject.GetComponentInChildren<Transform>().Find("Ground").GetComponent<Renderer>();
        animator = gameObject.GetComponentInChildren<Transform>().Find("Pawn").GetComponent<Animator>();
        groundMaterial = groundRenderer.material;
        PlaceTile(TileState.Empty);
    }

    private void OnMouseOver()
    {
        if(tileState == TileState.Empty)
        {
            groundRenderer.material = highlightMaterial;
        }
        else
        {
            groundRenderer.material = groundMaterial;
        }
    }

    private void OnMouseExit()
    {
        groundRenderer.material = groundMaterial;
    }

    public void TurnTile(TileState stateToTurn)
    {

        pawn.gameObject.SetActive(true);
        switch (stateToTurn)
        {
            case TileState.Black:
                animator.Play("FlipToBlack");
                break;
            case TileState.White:
                animator.Play("FlipToWhite");
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
        pawn.gameObject.SetActive(true);
        switch (stateToTurn)
        {
            case TileState.Black:
                animator.Play("PlaceBlack");
                break;
            case TileState.White:
                animator.Play("PlaceWhite");
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

    public TileState GetTileState()
    {
        return tileState;
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
