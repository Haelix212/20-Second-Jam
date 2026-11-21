using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MathGame : MonoBehaviour
{

    public Text equationText;

    public float lockoutTimer;

    public int NumberOfEquations;
    private int equationsCompleted;

    private int solution;
    private string playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPuzzle();
    }


    public void StartPuzzle()
    {
        playerInput = "";
        equationsCompleted = 0;
        GenerateEquation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnNumberPress()
    {
        GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Text buttonText = clickedButton.transform.GetChild(0).GetComponent<Text>();

        if (buttonText != null)
        {
            if (playerInput.Length < 2)
            {
                playerInput += buttonText.text;
                if(equationText.text.Contains("?"))
                {
                    equationText.text = equationText.text.Remove(equationText.text.IndexOf("?"));
                }
                equationText.text += buttonText.text;
            }
        }
    }
  

    public void CheckAnswer()
    {
        if(solution != 0)
        {
            if (playerInput != "" && playerInput.Length>1)
            {
                if (playerInput[0].ToString() == "0")
                {
                    playerInput = playerInput[1].ToString();
                }
            }
        }
        else
        {
            if (playerInput != "" && playerInput.Length > 1)
            {
                if (playerInput[1].ToString() == "0")
                {
                    playerInput = playerInput[0].ToString();
                }
            }
        }

        if (playerInput != "")
        {
            if (solution.ToString() == playerInput)
            {
                equationsCompleted++;
                CheckForWin();
            }
            else
            {
                equationText.transform.parent.GetComponent<Image>().color = Color.red;
                StartCoroutine(WaitAndReset());
            }
        }
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(lockoutTimer);
        equationText.transform.parent.GetComponent<Image>().color = Color.green;
        ClearPlayerInput();
        GenerateEquation(); 
    }

    private void GenerateEquation()
    {
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);

        switch(CoinFlip())
        {
            case 0:
                solution = num1 + num2;
                DisplayEquation(num1, true, num2);
                break;
            case 1:
                if(num1 >= num2)
                {
                    solution = num1 - num2;
                    DisplayEquation(num1, false,  num2);
                }
                else
                {
                    solution = num2 - num1;
                    DisplayEquation(num2,false, num1);
                }
            break;
        }
    }

    private void DisplayEquation(int num1, bool isAddition, int num2)
    {
        if (isAddition)
        {
            equationText.text = $"{num1} + {num2} = ?";
        }
        else
        {
            equationText.text = $"{num1} - {num2} = ?";
        }
    }

    private int CoinFlip()
    {
        int num = Random.Range(0, 2);
        return num;
    }

    public void ClearPlayerInput()
    {
        equationText.text = equationText.text.Remove(equationText.text.Length - playerInput.Length, playerInput.Length);
        playerInput = "";
    }

    private void CheckForWin()
    {
        if (equationsCompleted >= NumberOfEquations)
        {
            equationText.text = "WINNER";
            DisableButtons();
            //Stubbing in PuzzleManager call for puzzle complete.
            PuzzleManager.Instance.SetPuzzleCompleted("MathGame");
        }
        else
        {
            ClearPlayerInput();
            GenerateEquation();
        }
    }

    private void DisableButtons()
    {
        for (int i = 0; i < this.transform.childCount;i++)
        {
            if (transform.GetChild(i).GetComponent<Button>() != null)
            {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
}
