using UnityEngine;

public class NewWireDragBehavior : MonoBehaviour, IDragBehavior
{
    public GameObject LongWire;
    private Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnStartDragAction()
    {
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        if (slot.tag == this.tag)
        {
            // Debug.Log("Correctly Dropped");

            // This creates the wire that is drawn between the start and end positions
            GameObject longWire = Instantiate(LongWire,this.transform);
            longWire.transform.position = slot.transform.position;
            longWire.transform.LookAt(startPos, this.transform.up);

            float distance = Vector3.Distance(startPos, slot.transform.position);
            Vector3 startScale = longWire.transform.localScale;
            longWire.transform.localScale = new Vector3(startScale.x * Random.Range(0.5f, 2f), startScale.y, startScale.z * distance);
            longWire.GetComponentInChildren<MeshRenderer>().material = this.GetComponentInChildren<MeshRenderer>().material;

            // Set puzzle as completed with the PuzzleManager
            this.transform.parent.GetComponent<NewWires>().WireMatched();
            //PuzzleManager.Instance.SetPuzzleCompleted("Puzzle3");
        }
        else
        {
            Debug.Log("Incorrectly Dropped on " + slot.tag);
            this.GetComponent<DragDrop>().ResetPosition();
        }
    }


    public void OnFailedDropAction()
    {
    }

    public void OnEndDragAction(GameObject slot)
    {
    }


}
