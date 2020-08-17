using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform cameraTransform;
    private bool isFollowing;

	void Start()
	{

	}

	void Update()
	{
		if (isFollowing) {
			Follow ();
		}
	}

	public void OnStartFollowing()
	{	      
		cameraTransform = Camera.main.transform;
		isFollowing = true;
	}

	void Follow()
	{	
		cameraTransform.position = this.transform.position - new Vector3(0, 0, 10);
	}
}
