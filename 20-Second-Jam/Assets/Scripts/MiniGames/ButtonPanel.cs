using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{

    public GameObject buttonPrefab;

    public int numberOfRows;
    public int numberOfColumns;

    [Range(5, 25)]
    public int numberOfButtonsToClick;

    private GameObject[] Buttons;

    private float xSpawnOffset;
    private float ySpawnOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Buttons = new GameObject[numberOfRows*numberOfColumns];
        SetPanelSize();
        CreateButtons();
        PickButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateButtons()
    {
        int rowNumber = 0;
        int columnNumber = 0;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i] = Instantiate(buttonPrefab, this.transform);
            SetSpawnLocation(i, ref rowNumber, ref columnNumber, Buttons[i]);
            columnNumber++;
        }
    }
    private void AdjustPanelWidth()
    {
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        xSpawnOffset = buttonWidth;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth * (numberOfColumns + 1));
    }

    private void AdjustPanelHeight()
    {
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        ySpawnOffset= -buttonHeight;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight * (numberOfRows + 1));
    }

    private void SetPanelSize()
    {
        AdjustPanelHeight();
        AdjustPanelWidth();
    }

    private void SetSpawnLocation(int index, ref int rowNumber, ref int columnNumber, GameObject button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();

        //offsets use the length and width of the button prefab.
        // Adding half the offsets to the initial spawn will make the button centered on panel/canvas
        Vector3 initialSpawnLocation = new Vector3(xSpawnOffset/2, ySpawnOffset/2, 0);

        if (index % 5 == 0 && index != 0)
        {
            rowNumber++;
            columnNumber = 0;
        }
        rectTransform.anchoredPosition = initialSpawnLocation + new Vector3(xSpawnOffset * columnNumber, ySpawnOffset * rowNumber, 0f);
    } 

    private void PickButtons()
    {
        int[] ButtonsPicked = new int[numberOfButtonsToClick];
        for (int i = 0; i < numberOfButtonsToClick; i++)
        {
            ButtonsPicked[i] = Random.Range(0, Buttons.Length);
        }

        foreach (int button in ButtonsPicked)
        {
            Buttons[button].GetComponent<Button>().interactable = true;
        }
    }

    private bool IsThereADupe(int[] numArray)
    {
        for (int i = 0; i < numArray.Length;i++)
        {
           int valueToCheck = numArray[i];
           for (int n = 0; n<numArray.Length; n++)
           {
                if (valueToCheck == numArray[n] && i != n)
                {
                    print(i);
                    print(n);
                    return true;
                }
           }
        }
        return false;
    }
}
