using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
	Vector3 currentClickTarget;

	bool isInDirectMode = false; // TODO consider making static
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
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
		Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 m_Move = v*m_CamForward + h*Camera.main.transform.right;

		m_Character.Move (m_Move, false, false);
	}

	private void ProcessMouseMovement ()
	{
		//TODO Need to clear CurrentClickTarget when changing from keyboard back to mouse
		if (Input.GetMouseButton (0))
		{
			print ("Cursor raycast hit layer: " + cameraRaycaster.layerHit);
			// print ("Current Click Target: " + currentClickTarget);
			switch (cameraRaycaster.layerHit)
			{
				case Layer.Walkable:
					currentClickTarget = cameraRaycaster.hit.point;
					// Move can be here if you want to hold down button to walk
					break;
				case Layer.Enemies:
					print ("Can't move to enemy location");
					break;
				default:
					print ("Unexpected Layer Found!");
					return;
			}
		}
		var playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude >= walkMoveStopRadius)
		{
			// Move is called here so we don't have to hold down button
			print ("playerToClickPoint " + playerToClickPoint);
			print ("Magnitude: " + playerToClickPoint.magnitude);
			m_Character.Move (playerToClickPoint, false, false);
		}
		else
		{
			m_Character.Move (Vector3.zero, false, false);
		}
	}
}

