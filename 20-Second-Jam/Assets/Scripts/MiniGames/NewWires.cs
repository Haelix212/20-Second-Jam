using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Identifiers;

public class NewWires : PuzzleClass
{
    public GameObject[] wires;
    public GameObject[] slots;

    private int matchedPairs;
    private AudioSource wireSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartPuzzle();
    }

    public override void StartPuzzle()
    {
        wireSound = this.gameObject.GetComponent<AudioSource>();
        foreach (GameObject wire in wires)
        {
            wire.GetComponent<DragDrop>().StartPuzzle();
        }
        RandomizePositions(wires);
        RandomizePositions(slots);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void RandomizePositions(GameObject[] objectArray)
    {
        List<Vector3> StartingPositions = new List<Vector3>(objectArray.Length);
     
        for (int i = 0; i < objectArray.Length; i++)
        {
            StartingPositions.Add(objectArray[i].transform.position);
        }

        foreach (GameObject thing in objectArray)
        {
            int randomIndex = Random.Range(0, StartingPositions.Count);
            Vector3 newPosition = StartingPositions[randomIndex];
            thing.transform.position = newPosition;
            if(thing.GetComponent<DragDrop>() != null)
            {
                thing.GetComponent<DragDrop>().originalPosition = newPosition;
            }
            StartingPositions.RemoveAt(randomIndex);
        }
    }

    public void WireMatched()
    {
        matchedPairs++;
        if (wireSound != null) wireSound.PlayOneShot(wireSound.clip);
        if (matchedPairs >= wires.Length)
        {
            Debug.Log("All wires matched!");
            //Stubbed in PuzzleManager call to complete the puzzle.
            PuzzleManager.Instance.SetPuzzleCompleted("Wires");
        }
    }
}
