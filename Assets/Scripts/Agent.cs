﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Agent
{
    public static int nodeCount = 0;
    public static int depthReached = 10;
    public static IndexPair CalculateMove(TileState[,] board, TileState color, int depth)
    {
        Debug.Log("Starting MiniMax-search for " + color + "...");
        int alpha = int.MinValue;
        int beta = int.MaxValue;
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
                    int maxValue = MiniMax(Judge.SimulateTurn(board, availableMoves[0], color), TileState.White, depth - 1, alpha, beta);
                    if (MiniMax(Judge.SimulateTurn(board, availableMoves[i], color), TileState.White, depth - 1, alpha, beta) > maxValue)
                    {
                        moveToReturn = availableMoves[i];
                    }
                    break;
                case TileState.White:
                    int minValue = MiniMax(Judge.SimulateTurn(board, availableMoves[0], color), TileState.Black, depth - 1, alpha, beta);
                    if (MiniMax(Judge.SimulateTurn(board, availableMoves[i], color), TileState.Black, depth - 1, alpha, beta) < minValue)
                    {
                        moveToReturn = availableMoves[i];
                    }
                    break;
                default:
                    Debug.LogError("Color not valid.");
                    break;
            }
        }
        Debug.Log("Nodes examined: " + nodeCount);
        Debug.Log("Depth reached: " + depthReached);
        Debug.Log("MiniMax-search done.");
        depthReached = 10;
        nodeCount = 0;
        return moveToReturn;
    }

    public static int MiniMax(TileState[,] board, TileState color, int depth, int alpha, int beta)
    {
        nodeCount++;
        depthReached = 10 - depth;
        List<IndexPair> playableMoves = Judge.GetPlayableTiles(board, color);
        if (depth <= 0 || playableMoves.Count <= 0) return EvaluateBoard(board, color);
        switch (color)
        {
            case TileState.Black:
                int maxValue = int.MinValue;
                foreach (IndexPair move in playableMoves)
                {
                    int eval = MiniMax(Judge.SimulateTurn(board, move, color), TileState.White, depth - 1, alpha, beta);
                    maxValue = Mathf.Max(maxValue, eval);
                    alpha = Mathf.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxValue;

            case TileState.White:
                int minValue = int.MaxValue;
                foreach (IndexPair move in playableMoves)
                {
                    int eval = MiniMax(Judge.SimulateTurn(board, move, color), TileState.Black, depth - 1, alpha, beta);
                    maxValue = Mathf.Min(minValue, eval);
                    beta = Mathf.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
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
