using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Cinemachine;

public class Tasks : MonoBehaviour
{
	[SerializeField] private GameObject player;
	[SerializeField] private GameObject lookAroundObject;
	
	[SerializeField] private Button useButton;
	[SerializeField] private Button closeButton;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    NearTask();
    } 
    
	//////////////////////////////////FUEL ENGINES/////////////////////////////
	
	[Header("Fuel Engines")]
	[SerializeField] private GameObject storageTankRef;
	[SerializeField] private GameObject storageFuelUI;
	[SerializeField] private GameObject fuelBar;
	[SerializeField] private int tankFillTime;
	
	private void NearTask()
	{
		float distance = Vector3.Distance(player.transform.position, storageTankRef.transform.position);
		
		
		if(distance <= 4 )
		{
			useButton.interactable = true;
			if(Input.GetKey(KeyCode.Tab))
			{
				    StartFuel();
			}
			if(Input.GetKey(KeyCode.Escape))
			{
			 	CloseTaskUI();
			}
		}
		else
		{
			useButton.interactable = false;
			CloseTaskUI();
		}
	}
	
	public void FillFuel()
	{
		Vector3 scale = fuelBar.GetComponent<RectTransform>().localScale;
		if(scale.y > -1)
			fuelBar.GetComponent<RectTransform>().localScale = new Vector3(scale.x, scale.y - 0.03f, scale.z);
		else
			CloseTaskUI();
	}
	
	public void StartFuel()
	{
		Cursor.lockState = CursorLockMode.None;
		lookAroundObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_InputAxisName = "";
		
		storageFuelUI.SetActive(true);
		closeButton.gameObject.SetActive(true);
	}
	
	public void CloseTaskUI()
	{
		Cursor.lockState = CursorLockMode.Locked;
		lookAroundObject.GetComponent<CinemachineFreeLook>().m_XAxis.m_InputAxisName = "Mouse X";
		
		storageFuelUI.SetActive(false);
		closeButton.gameObject.SetActive(false);
	}
	
	//////////////////////////////////SWIPE CARD/////////////////////////////
	
	[Header("Swipe Card")]
	[SerializeField] private GameObject card;
	[SerializeField] private TMP_Text instruction;
	[SerializeField] private GameObject sliderStart;
	[SerializeField] private GameObject sliderEnd;
	[SerializeField] private Canvas canvas;
	private float cardSpeed = 450f;
	public static bool moveCardToStart = true;
	public static string instructionText = "PLEASE SWIPE CARD";
	
	public void CardClick()
	{
		//moveCardToStart = true;
		StartCoroutine("InsertCard");
	}
	
	private IEnumerator InsertCard()
	{
		
		while(moveCardToStart)
		{
			card.GetComponent<SwipeCard>().enabled = false;
			instruction.SetText(instructionText);
			
			while(Vector3.Distance(card.transform.position, sliderStart.transform.position) > 1f)
			{
			 	card.transform.position = Vector3.MoveTowards(card.transform.position, sliderStart.transform.position, Time.deltaTime * cardSpeed);
			 	yield return new WaitForSeconds(0.02f);
			}
			
			moveCardToStart = false;
			SwipeCard.startTime = Time.time;
			SwipeCard.isBadRead = false;
			card.GetComponent<SwipeCard>().enabled = true;
			yield return new WaitForSeconds(1f);
		}
	}
	
	public void CheckAcceptance()
	{
		if(SwipeCard.isBadRead)
		{
			instructionText = "BAD READ, TRY AGAIN!";
			moveCardToStart = true;
			//StartCoroutine("InsertCard");
		}
		else 
		{
			if(SwipeCard.timeTaken < 1.8f )
			{
				    instructionText = "TOO FAST! TRY AGAIN!";
				moveCardToStart = true;
				//StartCoroutine("InsertCard");
			}
			else if(SwipeCard.timeTaken > 2.4f )
			{
			 	instructionText = "TOO SLOW! TRY AGAIN!";
				moveCardToStart = true;
				//StartCoroutine("InsertCard");
			}
			else
			{
				instructionText = "ACCEPTED. THANK YOU.";
				instruction.SetText(instructionText);
			}
		}
		
		if(moveCardToStart)
			StartCoroutine("InsertCard");
	}
	
	
}