﻿using UnityEngine;
using System.Collections;
/*
PURPOSE: Simple script to make the zombies health bars
	always facing the main characters camera.
*/
public class CameraFacingBillBoard : MonoBehaviour
{
	public Camera m_Camera;
	void Start()
	{
		m_Camera = Camera.main;
	}
	void Update()
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back,
		                 m_Camera.transform.rotation * Vector3.up);
	}
}
