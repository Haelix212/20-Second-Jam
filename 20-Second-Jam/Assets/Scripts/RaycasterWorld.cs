using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycasterWorld : GraphicRaycaster
{
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        //Set middle screen pos or you can set variable on start and use it
        eventData.position = new(Screen.width / 2, Screen.height / 2);
        base.Raycast(eventData, resultAppendList);

        // Debug stuff to try and see why this isn't working with drag and drop items
        //foreach (RaycastResult result in resultAppendList)
        //{
        //    Debug.Log("Hit " + result.gameObject.name);
        //}
    }
}