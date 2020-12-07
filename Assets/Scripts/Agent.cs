using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Agent
{
    public static IndexPair CalculateMove(TileState[,] board, TileState color, int depth)
    {
        List<IndexPair> availableMoves = Judge.GetPlayableTiles(board, color);
        if (availableMoves.Count == 0)
        {
            return new IndexPair(board.GetLength(0), board.GetLength(1));
        }

        IndexPair moveToReturn = availableMoves[0];
        int maxValue = MiniMax(board, color, depth);
        for (int i = 1; i < availableMoves.Count; i++)
        {
            if (MiniMax(board, color, depth) > maxValue)
            {
                moveToReturn = availableMoves[i];
            }
        }
        return availableMoves[UnityEngine.Random.Range(0, availableMoves.Count)];
    }

    public static int MiniMax(TileState[,] board, TileState color, int depth)
    {
        return 1;
    }

    public static float EvaluateBoard(TileState[,] board, TileState color)
    {
        int nrOfWhites = 0, nrOfBlacks = 0;

        for (int i = 0; i < board.GetLength(1); i++)
        {
            for (int j = 0; j < board.GetLength(0); j++)
            {
                if (board[j, i] == TileState.Black)
                {
                    nrOfBlacks++;
                }
                else if (board[j, i] == TileState.White)
                {
                    nrOfWhites++;
                }
            }
        }

        int evalValue = nrOfBlacks - nrOfWhites;

        return evalValue;
    }
}
