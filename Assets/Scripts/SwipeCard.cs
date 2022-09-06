using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SwipeCard : MonoBehaviour, IDragHandler
{
	[SerializeField] private RectTransform dragRectTransform;
	[SerializeField] private RectTransform startPos;
	[SerializeField] private RectTransform endPos;
	[SerializeField] private Canvas canvas;
	[SerializeField] private TMP_Text instruction;
	[SerializeField] private GameObject tasksController;
	
	private float startX;
	private float endX;
	
	public static float startTime;
	public static float endTime;
	public static float timeTaken;
	
	[HideInInspector] public static bool isBadRead = false;
	[HideInInspector] public static bool isTooSlow = false;
	[HideInInspector] public static bool isTooFast = false;
	private PointerEventData eventData;
	
    // Start is called before the first frame update
    void Start()
    {
	    startX = startPos.anchoredPosition.x;
	    endX = endPos.anchoredPosition.x;
    }

    // Update is called once per frame
    void Update()
	{
		if(dragRectTransform != null)
		{
			if(!eventData.dragging && dragRectTransform.anchoredPosition.x < endX && dragRectTransform.anchoredPosition.x > startX && !Tasks.moveCardToStart)
		    {
				Tasks.instructionText = "BAD READ, TRY AGAIN!";
				//instruction.SetText(Tasks.instructionText);
				Tasks.moveCardToStart = true;
				tasksController.GetComponent<Tasks>().CardClick();
		    }
		}
    }
    
	public void OnDrag(PointerEventData eventData) 
	{
		this.eventData = eventData;
		
		float x = dragRectTransform.anchoredPosition.x;
		
		isBadRead = !isBadRead ? eventData.delta.x < 0 : true;
		
		if((x > startX && x < endX) || 
		((x == startX && eventData.delta.x > 0f) || (x == endX && eventData.delta.x < 0f)))
		{
			dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
			dragRectTransform.anchoredPosition = new Vector2(dragRectTransform.anchoredPosition.x, 34f);
		}
		else if (x > endX)
		{
			dragRectTransform.anchoredPosition = new Vector2(endX, 34f);
			endTime = Time.time;
			timeTaken = endTime - startTime;
			Debug.Log("Start: " + startTime + "\tEnd: " + endTime + "\tTime Taken: " + timeTaken);
			tasksController.GetComponent<Tasks>().CheckAcceptance();
		}
		else if (x < startX)
		{
			dragRectTransform.anchoredPosition = new Vector2(startX, 34f);
		}
		
		//if(!eventData.dragging && isBadRead)
		//	instruction.SetText("BAD READ, TRY AGAIN!");
	}
}
