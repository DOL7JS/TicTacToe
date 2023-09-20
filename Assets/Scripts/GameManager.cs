using Mirror;
using Mirror.Discovery;
using Mirror.Examples.Pong;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    Sprite boxClear;
    Sprite boxCross;
    Sprite boxCircle;
    [SerializeField]
    public Text textTurn;
    [SerializeField]
    Button buttonReset;
    [SerializeField]
    GameObject playground;

    [SyncVar(hook = nameof(OnTurnStateChanged))]
    public EnumPlayerType turn = EnumPlayerType.NONE;
    [SyncVar(hook = nameof(OnGameStateChanged))]
    string gameState = "playing";
    readonly int scoreToWin = 3;
    private Box[,] gameboard;


    void Start()
    {
        InitGameBoard();
        boxClear = Images.BoxClear;
        boxCross = Images.BoxCross;
        boxCircle = Images.BoxCircle;
    }


    public void InitGameBoard()
    {
        gameboard = new Box[3, 3];
        int boxCounter = 0;
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            for (int j = 0; j < gameboard.GetLength(1); j++)
            {
                gameboard[i, j] = playground.transform.GetChild(boxCounter++).GetComponent<Box>();
                gameboard[i, j].gameboard = gameboard;
            }
        }
        textTurn.text = "Waiting for player ...";
        turn = EnumPlayerType.NONE;
        Debug.Log("isClientOnly: " + (isClientOnly));
        if (isClientOnly)
        {
            textTurn.text = "Turn: Player X";
            TurnChanged(EnumPlayerType.CROSS);
            turn = EnumPlayerType.CROSS;
        }
        textTurn.gameObject.SetActive(true);

    }





    public void OnClickBox()
    {
        if (NetworkClient.localPlayer.gameObject.GetComponent<PlayerTicTac>().playerType != turn || turn == EnumPlayerType.NONE)
        {
            Debug.Log("Not your turn");
            return;
        }
        GameObject currentBox = EventSystem.current.currentSelectedGameObject;
        int row = currentBox.GetComponent<Box>().row;
        int column = currentBox.GetComponent<Box>().column;
        int value = currentBox.GetComponent<Box>().value;

        if (value == -1)
        {
            currentBox.GetComponent<Box>().value = (int)turn;
            SetImage(currentBox.GetComponent<Box>(), (int)turn);
            gameboard[row, column].GetComponent<Box>().value = (int)turn;
            if (CheckPotentialWin(row, column, (int)turn))
            {
                textTurn.text = "Player " + ((int)turn == 1 ? "X" : "O") + " wins !";
                GameStateChanged(textTurn.text);
                buttonReset.gameObject.SetActive(true);
                SetButtonInteractibility(false);
                return;
            }
            if (CheckPotentialEnd())
            {
                textTurn.text = "GAME OVER";
                GameStateChanged(textTurn.text);
                buttonReset.gameObject.SetActive(true);
                SetButtonInteractibility(false);
                return;
            }

            EnumPlayerType type;
            if (turn == EnumPlayerType.CROSS)
            {
                type = EnumPlayerType.CIRCLE;
            }
            else
            {
                type = EnumPlayerType.CROSS;
            }
            turn = type;
            TurnChanged(type);
            textTurn.text = turn == EnumPlayerType.CROSS ? "Turn: Player X" : "Turn: Player O";
        }
        else
        {
            Debug.Log("Already chosen position.");
        }


    }

    public void SetImage(Box box, int value)
    {
        box.GetComponent<Image>().sprite = value == 1 ? boxCross : boxCircle;
    }
    private bool CheckPotentialEnd()
    {
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            for (int j = 0; j < gameboard.GetLength(1); j++)
            {
                if (gameboard[i, j].value == -1)
                {
                    return false;
                }
            }

        }
        return true;
    }
    [Command(requiresAuthority = false)]
    public void GameStateChanged(string newState)
    {
        gameState = newState;
    }
    private void OnGameStateChanged(string oldState, string newState)
    {
        gameState = newState;
        textTurn.text = newState;
        buttonReset.gameObject.SetActive(gameState.Contains("wins") || gameState.Contains("GAME OVER"));
        SetButtonInteractibility(true);
    }

    [Command(requiresAuthority = false)]
    public void TurnChanged(EnumPlayerType newState)
    {
        turn = newState;
    }
    private void OnTurnStateChanged(EnumPlayerType oldState, EnumPlayerType newState)
    {
        turn = newState;
        textTurn.text = turn == EnumPlayerType.CROSS ? "Turn: Player X" : "Turn: Player O";
    }

    private bool CheckPotentialWin(int row, int column, int player)
    {
        return CheckPotentialRowWin(row, player)
            || CheckPotentialColumnWin(column, player)
            || CheckPotentialDiagonalWin(row, column, player)
            || CheckPotentialDiagonal2Win(row, column, player);
    }

    private bool CheckPotentialColumnWin(int column, int player)
    {
        int score = 0;
        for (int i = 0; i < gameboard.GetLength(1); i++)
        {
            if (gameboard[i, column].value == player)
            {
                score++;
            }
        }
        return score >= scoreToWin;
    }

    private bool CheckPotentialRowWin(int row, int player)
    {
        int score = 0;
        for (int i = 0; i < gameboard.GetLength(0); i++)
        {
            if (gameboard[row, i].value == player)
            {
                score++;
            }
        }
        return score >= scoreToWin;
    }

    private bool CheckPotentialDiagonalWin(int row, int column, int player)
    {
        int score = 0;
        int actualColumn = column + 1;
        for (int i = row + 1; i < gameboard.GetLength(0) && actualColumn < gameboard.GetLength(1); i++)
        {
            if (gameboard[i, actualColumn].value == player)
            {
                score++;
            }

            actualColumn++;
        }

        actualColumn = column - 1;
        for (int i = row - 1; i >= 0 && actualColumn >= 0; i--)
        {

            if (gameboard[i, actualColumn].value == player)
            {
                score++;
            }

            actualColumn--;
        }
        return score + 1 >= scoreToWin;
    }

    private bool CheckPotentialDiagonal2Win(int row, int column, int player)
    {
        int score = 0;
        int actualColumn = column - 1;
        for (int i = row + 1; i < gameboard.GetLength(0) && actualColumn >= 0; i++)
        {
            if (gameboard[i, actualColumn].value == player)
            {
                score++;
            }

            actualColumn--;
        }

        actualColumn = column + 1;
        for (int i = row - 1; i >= 0 && actualColumn < gameboard.GetLength(1); i--)
        {

            if (gameboard[i, actualColumn].value == player)
            {
                score++;
            }

            actualColumn++;
        }
        return score + 1 >= scoreToWin;
    }

    public void OnClickButtonReset()
    {
        textTurn.text = turn == EnumPlayerType.CROSS ? "Turn: Player X" : "Turn: Player O";
        GameStateChanged(textTurn.text);
        foreach (Transform box in playground.GetComponentsInChildren<Transform>())
        {
            if (box.gameObject.GetComponent<Image>() != null)
            {
                box.gameObject.GetComponent<Image>().sprite = boxClear;
                box.gameObject.GetComponent<Box>().value = -1;
                box.gameObject.GetComponent<Box>().OnBoxValueChanged();
            }

        }
        buttonReset.gameObject.SetActive(false);
        SetButtonInteractibility(true);
    }
    private void SetButtonInteractibility(bool value)
    {
        foreach (Transform box in playground.GetComponentsInChildren<Transform>())
        {
            if (box.gameObject.GetComponent<Image>() != null)
            {
                box.gameObject.GetComponent<Button>().interactable = value;
            }
        }
    }
}
