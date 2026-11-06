using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGame : MonoBehaviour
{

    public int NumberOfPairs;

    public GameObject slotPrefab;
    public GameObject plugPrefab;

    private GameObject[] Slots;
    private GameObject[] Plugs;


    public Vector3 plugSpawnPosition;
    public Vector3 slotSpawnPosition;
    public Vector3 spawnOffset;

    private int matchedPairs = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Declares array lengths
        Slots = new GameObject[NumberOfPairs];
        Plugs = new GameObject[NumberOfPairs];

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
        List<int> Identifiers = new List<int>(NumberOfPairs);

        for (int i = 0; i < NumberOfPairs; i++)
        {
            Identifiers.Add(i);
        }

        foreach (GameObject plug in Plugs)
        {
            int randomIndex = Random.Range(0, Identifiers.Count);
            int identifier = Identifiers[randomIndex];
            GiveColor(plug, identifier);
            Identifiers.RemoveAt(randomIndex);
        }

        for (int i = 0; i < NumberOfPairs; i++)
        {
            Identifiers.Add(i);
        }

        foreach (GameObject slot in Slots)
        {
            int randomIndex = Random.Range(0, Identifiers.Count);
            int identifier = Identifiers[randomIndex];
            GiveColor(slot, identifier);
            Identifiers.RemoveAt(randomIndex);
        }
    }

    private void GiveColor(GameObject obj, int identifier)
    {
        switch (identifier)
        {
            case 0:
                obj.GetComponent<Image>().color = Color.red;
                break;
            case 1:
                obj.GetComponent<Image>().color = Color.blue;
                break;
            case 2:
                obj.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                obj.GetComponent<Image>().color = Color.yellow;
                break;
            default:
                Debug.LogError("Invalid identifier index: " + identifier);
                break;
        }
    }

    public void WireMatched()
    {
        matchedPairs++;
        if (matchedPairs >= NumberOfPairs)
        {
            this.gameObject.SetActive(false);
            Debug.Log("All wires matched!");
        }
    }
}
