using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{

    public GameObject buttonPrefab;
    public int numberOfRows;
    public int numberOfColumns;

    [Range(1, 25)]
    public int numberOfButtonsToClick;

    private GameObject[] Buttons;
    private int ActiveButtons;

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
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width * 1.5f;
        xSpawnOffset = buttonWidth;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth * (numberOfColumns + 1));
    }

    private void AdjustPanelHeight()
    {
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height * 1.5f;
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

        if (index % numberOfColumns == 0 && index != 0)
        {
            rowNumber++;
            columnNumber = 0;
        }
        Vector3 newLocation = initialSpawnLocation + new Vector3(xSpawnOffset * columnNumber, ySpawnOffset * rowNumber, 0f);
        rectTransform.anchoredPosition = newLocation;
        // GameObject mesh = Instantiate(buttonMesh, newLocation, Quaternion.Euler(0f, 0f, -1f));
        // GameObject mesh = Instantiate(buttonMesh, rectTransform);
        // mesh.transform.localPosition += new Vector3(25f, -25f, 0);
    } 

    private void PickButtons()
    {
        int[] ButtonsPicked = new int[numberOfButtonsToClick];
        for (int i = 0; i < numberOfButtonsToClick; i++)
        {
            int rand = Random.Range(0, Buttons.Length);
            while (IsThereADupe(ButtonsPicked, rand))
            {
                rand = Random.Range(0, Buttons.Length);
                // Debug.Log("Rerolling new random number = " + rand);
            }
            ButtonsPicked[i] = rand;
            // Debug.Log("Random button num: " + rand);
        }

        foreach (int button in ButtonsPicked)
        {
            Button pickedButton = Buttons[button].GetComponent<Button>();
            pickedButton.transform.GetComponentInChildren<Light>().color = Color.red;
            pickedButton.onClick.RemoveAllListeners();
            pickedButton.onClick.AddListener(delegate { ButtonPressed(pickedButton.transform.GetComponentInChildren<Light>()); } );
            pickedButton.onClick.AddListener(delegate { pickedButton.interactable = false; } );
            pickedButton.interactable = true;

            ActiveButtons++;
            // Debug.Log("Active buttons = " + ActiveButtons);
        }
    }

    private bool IsThereADupe(int[] numArray, int k)
    {
        for (int i = 0; i < numArray.Length;i++)
        {
            if (numArray[i] == k) return true;
        }
        return false;
    }    

    public void ButtonPressed(Light buttonLight)
    {
        if (buttonLight != null) buttonLight.color = Color.green;
        
        ActiveButtons--;
        Debug.Log("Buttons Remaining: " + ActiveButtons);
        if (ActiveButtons <= 0) PuzzleManager.Instance.SetPuzzleCompleted("ButtonPanel");
    }
}
