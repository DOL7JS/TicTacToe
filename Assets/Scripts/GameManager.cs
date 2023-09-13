using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Sprite boxCross;
    [SerializeField]
    Sprite boxCircle;
    [SerializeField]
    Text textTurn;

    int player = 0;
    int scoreToWin = 3;
    private int[,] gameboard;

    void Start()
    {
        InitGameBoard();
    }

    private void InitGameBoard()
    {
        gameboard = new int[3, 3];
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            for (int j = 0; j < gameboard.GetLength(1); j++)

            {
                gameboard[i, j] = -1;
            }
        }
    }

    public void OnClickBox()
    {
        GameObject currentBox = EventSystem.current.currentSelectedGameObject;
        int row = currentBox.GetComponent<Box>().row;
        int column = currentBox.GetComponent<Box>().column;
        int value = currentBox.GetComponent<Box>().value;

        if (value == -1)
        {
            currentBox.GetComponent<Box>().value = player;
            currentBox.GetComponent<Image>().sprite = player == 1 ? boxCross : boxCircle;
            gameboard[row, column] = player;
            if (CheckPotentialWin(row, column, player))
            {
                textTurn.text = "Player " + (player) + " wins !";
                Debug.Log("WINNER");
                return;
            }
            if (CheckPotentialEnd())
            {
                textTurn.text = "GAME OVER";
                Debug.Log("Game over");
                return;
            }

            player = player == 1 ? 0 : 1;
            textTurn.text = player == 1 ? "Turn: Player X" : "Turn: Player O";
            Debug.Log("NAME: " + currentBox.name);

        }
        else
        {
            Debug.Log("Already chosen position.");
        }


    }

    private bool CheckPotentialEnd()
    {
        int sum = 0;
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            for (int j = 0; j < gameboard.GetLength(1); j++)
            {
                if (gameboard[i, j] == -1)
                {
                    return false;
                }
            }

        }
        return true;
    }

    private bool CheckPotentialWin(int row, int column, int player)
    {

        return CheckPotentialRowWin(row, player) || CheckPotentialColumnWin(column, player);
    }

    private bool CheckPotentialColumnWin(int column, int player)
    {
        int score = 0;
        for (int i = 0; i < gameboard.GetLength(1); i++)
        {
            if (gameboard[i, column] == player)
            {
                score++;
            }
        }
        Debug.Log("ScoreC:" + score);
        return score >= scoreToWin;
    }

    private bool CheckPotentialRowWin(int row, int player)
    {
        int score = 0;
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            if (gameboard[row, i] == player)
            {
                score++;
            }
        }
        return score >= scoreToWin;
    }

    private void ShowGameBoard()
    {
        string mess = "";
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            for (int j = 0; j < gameboard.GetLength(1); j++)

            {
                mess += gameboard[i, j] + "| ";
            }
            mess += "\n";
        }

        Debug.Log(mess);
    }
}
