using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
	[SerializeField] private Vector3 targetPosition;
	[SerializeField] private RectTransform arrow;
	[SerializeField] private GameObject player;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    Vector3 toPos = targetPosition;
	    Vector3 fromPos = player.transform.position;
    }
}
