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
       
        for (int i = 1; i < availableMoves.Count; i++)
        {
            switch (color)
            {
                case TileState.Black:
                    int maxValue = MiniMax(Judge.SimulateTurn(board, availableMoves[0], color), TileState.White, --depth);
                    if (MiniMax(Judge.SimulateTurn(board, availableMoves[i], color), TileState.White, --depth) > maxValue)
                    {
                        moveToReturn = availableMoves[i];
                    }
                    break;
                case TileState.White:
                    int minValue = MiniMax(Judge.SimulateTurn(board, availableMoves[0], color), TileState.Black, --depth);
                    if (MiniMax(Judge.SimulateTurn(board, availableMoves[i], color), TileState.Black, --depth) < minValue)
                    {
                        moveToReturn = availableMoves[i];
                    }
                    break;
                default:
                    Debug.LogError("Color not valid.");
                    break;
            }
        }
        return moveToReturn;
    }

    public static int MiniMax(TileState[,] board, TileState color, int depth)
    {
        if (depth <= 0) return EvaluateBoard(board, color);

        List<IndexPair> playableMoves = Judge.GetPlayableTiles(board, color);
        switch (color)
        {
            case TileState.Black:
                int maxValue = int.MinValue;
                foreach (IndexPair move in playableMoves)
                {
                    int eval = MiniMax(Judge.SimulateTurn(board, move, color), TileState.White, --depth);
                    maxValue = Mathf.Max(maxValue, eval);
                }
                return maxValue;

            case TileState.White:
                int minValue = int.MaxValue;
                foreach (IndexPair move in playableMoves)
                {
                    int eval = MiniMax(Judge.SimulateTurn(board, move, color), TileState.Black, --depth);
                    maxValue = Mathf.Min(minValue, eval);
                }
                return minValue;

            default:
                Debug.LogError("Color not valid.");
                break;
        }
        return 1;
    }

    public static int EvaluateBoard(TileState[,] board, TileState color)
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
