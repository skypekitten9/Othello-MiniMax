using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    IndexPair move;
    TileState[,] board;
    TileState color;
    Node parent;
    List<Node> children;
    public Node(IndexPair move, TileState[,] board, TileState color, Node parent)
    {
        children = new List<Node>();
        this.move = move;
        this.board = board;
        this.color = color;
        this.parent = parent;
    }

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    public bool IsLeaf()
    {
        if (children.Count == 0) return true;
        else return false;
    }

    public int EvaluateBoard(TileState[,] board, TileState color)
    {
        int whiteTiles = 0;
        int blackTiles = 0;
        int emptyTiles = 0;

        for (int i = 0; i < board.GetLength(1); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                switch (board[j, i])
                {
                    case TileState.Black:
                        blackTiles++;
                        break;
                    case TileState.White:
                        whiteTiles++;
                        break;
                    case TileState.Empty:
                        emptyTiles++;
                        break;
                    default:
                        Debug.LogError("Tried to calculate score of empty tile. Not allowed.");
                        break;
                }
            }
        }

        switch (color)
        {
            case TileState.Black:
                return blackTiles - whiteTiles;

            case TileState.White:
                return whiteTiles - blackTiles;

            default:
                Debug.LogError("Calculated score of empty tile, not allowed.");
                return 0;
        }
    }
}
