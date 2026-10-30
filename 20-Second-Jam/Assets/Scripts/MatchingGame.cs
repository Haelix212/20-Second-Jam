using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGame : MonoBehaviour
{

    public int NumberOfPairs;

    public GameObject slotPrefab;
    public GameObject plugPrefab;

    public Image ParentPanel;

    private GameObject[] Slots;
    private GameObject[] Plugs;

    public Vector3 spawnOffset;
    private Vector3 spawnPosition = Vector3.zero;


    private void Awake()
    {

    }
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
            GameObject newPlug = Instantiate(plugPrefab, ParentPanel.transform);
            Plugs[i] = newPlug;

            RectTransform rectTransform = Plugs[i].GetComponent<RectTransform>();

            rectTransform.anchoredPosition = spawnPosition + spawnOffset;
            spawnOffset += initialSpawnOffset;
        }

        spawnOffset = initialSpawnOffset;

        for (int i = 0; i < Slots.Length; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, ParentPanel.transform);
            Slots[i] = newSlot;

            RectTransform rectTransform = Slots[i].GetComponent<RectTransform>();

            rectTransform.anchoredPosition = spawnPosition + spawnOffset;
            spawnOffset += initialSpawnOffset;
        }
    }

    private void AssignPairs()
    {
        List<int> Identifiers = new List<int>(NumberOfPairs);

        for (int i = 0; i < NumberOfPairs; i++)
        {
            Identifiers[i] = i;
        }
    }
}
