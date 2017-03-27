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
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
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
			m_Character.Move(playerToClickPoint - transform.position, false, false);	
		}
		else
		{
			m_Character.Move(Vector3.zero,false, false);
		}

    }
}

