using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour 
{
	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D targetCursor = null;
	[SerializeField] Texture2D unknownCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

	CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () 
	{
		cameraRaycaster = GetComponent<CameraRaycaster> ();	
		cameraRaycaster.onLayerChange += OnLayerChanged;
	}
	

	void OnLayerChanged (Layer newLayer) // Only call when layer changes
	{
		print ("CursorAffordance delegate reporting for duty!");
		switch (newLayer)
		{
			case Layer.Walkable:
				Cursor.SetCursor (walkCursor, cursorHotspot, CursorMode.Auto);
				break;
			case Layer.Enemies:
				Cursor.SetCursor (targetCursor, cursorHotspot, CursorMode.Auto);
				break;
			case Layer.RaycastEndStop:
				print ("Made it to RaycastEndStop in case statement!");
				Cursor.SetCursor (unknownCursor, cursorHotspot, CursorMode.Auto);
				break;
			default:
				Debug.Log ("Don't know what cursor to show!");
				break;
		}

	}

	// TODO consider de-registering on leaving all game scenes
}
