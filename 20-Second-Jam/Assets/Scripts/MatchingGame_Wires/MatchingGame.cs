using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Identifiers;
using UnityEngine.UI;

public class MatchingGame : MonoBehaviour
{

    public int NumberOfPairs;

    public GameObject slotPrefab;
    public GameObject plugPrefab;

    private GameObject[] Slots;
    private GameObject[] Plugs;


    private Vector3 plugSpawnPosition;
    private Vector3 slotSpawnPosition;
    private Vector3 spawnOffset;

    private int matchedPairs = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
   
        //Declares array lengths
        Slots = new GameObject[NumberOfPairs];
        Plugs = new GameObject[NumberOfPairs];

        spawnOffset = new Vector3 (0, -plugPrefab.GetComponent<RectTransform>().rect.height,0);

        AdjustPanelHeight();
        SetSpawnPositions();
        CreatePairs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePairs()
    {
        Vector3 initialSpawnOffset = spawnOffset;

        for (int i = 0; i < Plugs.Length; i++)
        {
            GameObject newPlug = Instantiate(plugPrefab, this.transform);
            Plugs[i] = newPlug;

            RectTransform rectTransform = Plugs[i].GetComponent<RectTransform>();

            if(i > 0)
            {
                rectTransform.anchoredPosition = plugSpawnPosition + spawnOffset;
                spawnOffset += initialSpawnOffset;
            }
            else
            {
                rectTransform.anchoredPosition = plugSpawnPosition;
            }
        }

        spawnOffset = initialSpawnOffset;

        for (int i = 0; i < Slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, this.transform);
            Slots[i] = newSlot;

            RectTransform rectTransform = Slots[i].GetComponent<RectTransform>();

            if(i > 0)
            {
                rectTransform.anchoredPosition = slotSpawnPosition + spawnOffset;
                spawnOffset += initialSpawnOffset;
            }
            else
            {
                rectTransform.anchoredPosition = slotSpawnPosition;
            }
        }

        AssignPairs();
    }

    private void AssignPairs()
    {
       if(NumberOfPairs > 4)
       {
            CreateInfinitePairs();
       }
       else
       {
            CreateSimplePairs();
       }
    }

    private void GiveColor(GameObject obj, Color identifier)
    {
        obj.GetComponent<Image>().color = identifier;
    }

    public void WireMatched()
    {
        matchedPairs++;
        if (matchedPairs >= NumberOfPairs)
        {
            this.gameObject.SetActive(false);
            Debug.Log("All wires matched!");
            //Stubbed in PuzzleManager call to complete the puzzle.
            //PuzzleManager.Instance.SetPuzzleCompleted("Wires");
        }
    }

    private void AdjustPanelHeight()
    {
        float slotHeight = slotPrefab.GetComponent<RectTransform>().rect.height;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotHeight * (NumberOfPairs + 1));
    }

    private void SetSpawnPositions()
    {
        float height = GetComponent<RectTransform>().rect.height;
        float width = GetComponent<RectTransform>().rect.width;
        plugSpawnPosition = new Vector3(-width / 3, height / 2 + spawnOffset.y, 0);
        slotSpawnPosition = new Vector3(width / 3, height / 2 + spawnOffset.y, 0);
    } 

    private void CreateInfinitePairs()
    {
        List<Color> Identifiers = new List<Color>(NumberOfPairs);
        List<Color> IdentifierCopies = new List<Color>(NumberOfPairs);

        //Genrates a random color and adds its to color array.
        for (int i = 0; i < NumberOfPairs; i++)
        {
            Identifiers.Add(new Color(Random.value, Random.value, Random.value));
            IdentifierCopies.Add(Identifiers[i]);
        }

        //Loops through each plug in the array and it assigns it a random color from the list.
        // Color is then removed from the list so it does not repeat.
        foreach (GameObject plug in Plugs)
        {
            int randomIndex = Random.Range(0, Identifiers.Count);
            Color identifier = Identifiers[randomIndex];
            GiveColor(plug, identifier);
            Identifiers.RemoveAt(randomIndex);
        }
        // Same as above but for slots.
        foreach (GameObject slot in Slots)
        {
            int randomIndex = Random.Range(0, IdentifierCopies.Count);
            Color identifier = IdentifierCopies[randomIndex];
            GiveColor(slot, identifier);
            IdentifierCopies.RemoveAt(randomIndex);
        }
    }

    // Uses old logic of using a switch statement.
    //Choices are limited but can be expanded if we want more than 4 pairs.
    private void CreateSimplePairs()
    {
        List<Color> Identifiers = new List<Color>(NumberOfPairs);
        List<Color> IdentifierCopies = new List<Color>(NumberOfPairs);

        //Fills Identifier lists with colors.
        for (int i = 0; i < NumberOfPairs; i++)
        {
            switch (i)
            {
                case 0:
                    Identifiers.Add(Color.red);
                    break;
                case 1:
                    Identifiers.Add(Color.green);
                    break;
                case 2:
                    Identifiers.Add(Color.blue);
                    break;
                case 3:
                    Identifiers.Add(Color.yellow);
                    break;
                default:
                    break;
            }
            IdentifierCopies.Add(Identifiers[i]);
        }

        // Loops through each plug in the array.  Assigns a random color and removes its from the list.
        foreach (GameObject plug in Plugs)
        {
            int randomIndex = Random.Range(0, Identifiers.Count);
            Color identifier = Identifiers[randomIndex];
            GiveColor(plug, identifier);
            Identifiers.RemoveAt(randomIndex);
        }
        //Same as above but for slots.
        foreach (GameObject slot in Slots)
        {
            int randomIndex = Random.Range(0, IdentifierCopies.Count);
            Color identifier = IdentifierCopies[randomIndex];
            GiveColor(slot, identifier);
            IdentifierCopies.RemoveAt(randomIndex);
        }

    }


}
