using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float attackMoveStopRadius = 5f;

	ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
	Vector3 currentClickTarget,clickPoint;

	bool isInDirectMode = false; // TODO consider making static
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

	// TODO fix issue with click to move and wsad conflict

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		if (Input.GetKeyDown(KeyCode.G)) // G for gamepad.  TODO allow player to map later
		{
			isInDirectMode = !isInDirectMode; // toggle mode
			print ("isInDirectMode = " + isInDirectMode);
			currentClickTarget = transform.position;
		}

		if (isInDirectMode)
		{
			ProcessDirectMovement();	
		} else
		{
			ProcessMouseMovement ();
		}

    }

	private void ProcessDirectMovement()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		// calculate camera relative direction to move
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 movement = v*cameraForward + h*Camera.main.transform.right;

		thirdPersonCharacter.Move (movement, false, false);
	}

	private void ProcessMouseMovement ()
	{
		//TODO Need to clear CurrentClickTarget when changing from keyboard back to mouse
		if (Input.GetMouseButton (0))
		{
			// Below is just for debug
			//print ("Cursor raycast hit layer: " + cameraRaycaster.currentLayerHit);
			// print ("Current Click Target: " + currentClickTarget);
			clickPoint = cameraRaycaster.hit.point;
			switch (cameraRaycaster.currentLayerHit)
			{
				case Layer.Walkable:
					//currentClickTarget = clickPoint;
					currentClickTarget = ShortDestination (clickPoint, walkMoveStopRadius);
					// Move can be here if you want to hold down button to walk
					break;
				case Layer.Enemies:
					currentClickTarget = ShortDestination (clickPoint, attackMoveStopRadius);
					break;
				default:
					print ("Unexpected Layer Found!");
					return;
			}
		}
		WalkToDestination ();
	}

	void WalkToDestination ()
	{
		var playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius)
		{
			// Move is called here so we don't have to hold down button
			// Below is just for debug
			// print ("playerToClickPoint " + playerToClickPoint);
			// print ("Magnitude: " + playerToClickPoint.magnitude);
			thirdPersonCharacter.Move (playerToClickPoint, false, false);
		}
		else
		{
			thirdPersonCharacter.Move (Vector3.zero, false, false);
		}
	}

	Vector3 ShortDestination(Vector3 destination, float shortening)
	{
		Vector3 reductionVector = (destination - transform.position).normalized * shortening;
		return destination - reductionVector;
	}

	void OnDrawGizmos()
	{
		// Draw movement gizmos

		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, currentClickTarget);
		Gizmos.DrawSphere (currentClickTarget, 0.1f);
		Gizmos.DrawSphere (clickPoint, 0.15f);
	
		// Draw attack sphere
		// Gizmos.color = new Color(255f, 0f, 0f, .5f);
		// Gizmos.DrawSphere (transform.position, attackMoveStopRadius);
	}
}

