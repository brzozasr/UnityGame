using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class door : MonoBehaviour {
	
	public Material GreenLight;
	public Material RedLight;

	private Animation _doorAnimation;
	private Transform _platform;
	private bool _openPrevState;

	private void Start()
	{
		_platform = transform.Find("Platform");
		//_doorAnimation = thedoor.GetComponent<Animation>();
		Player.OnPlatformEnter += OpenDoor;
	}

	private void Update()
	{
		
	}

	private void OpenDoor(object sender, EventArgs args)
	{
		var sphere = _platform.GetChild(0);
		var spotLight = _platform.GetChild(1);

		sphere.GetComponent<Renderer>().material = GreenLight;
		
		spotLight.GetComponent<Light>().color = Color.green;
		
		_doorAnimation.Play("open");
	}
}