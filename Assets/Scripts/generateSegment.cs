﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateSegment : MonoBehaviour {
	public GameObject[] pathPrefabs;
	public float spawnDistance = 15;
	private GameObject[] pathSegments;
	public float[] pathLengths;

	private float endLength = -20;
	private int nextPlatform = 1;

	public Transform player;

	// Use this for initialization
	void Start () {
		pathSegments = new GameObject[pathPrefabs.Length];
		pathLengths = new float[pathPrefabs.Length];

		for (int i = 0; i < pathPrefabs.Length; i++) {
			Vector3 pos = transform.position + new Vector3(0, 0, endLength);
			GameObject pathSegment = (GameObject)Instantiate (pathPrefabs[i], pos, transform.rotation);
			pathSegment.transform.SetParent (transform);
			pathLengths [i] = CalculateZSize (pathSegment);
			pathSegments[i] = pathSegment;
		}
	}

	private float CalculateZSize(GameObject obj)
	{
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

		foreach(Renderer renderer in obj.GetComponentsInChildren<Renderer>())
		{
			bounds.Encapsulate(renderer.bounds);
		}

		Vector3 localCenter = bounds.center - this.transform.position;
		bounds.center = localCenter;
		return bounds.size.z;
	}
		
	// Update is called once per frame
	void Update () {
		Vector3 distance = player.position - transform.position;
		double zDist = Vector3.Dot(distance, transform.forward.normalized);

		if (zDist + spawnDistance > endLength) {
			placeNextPlatform ();
		}
	}

	void placeNextPlatform () {
		pathSegments [nextPlatform].transform.position = new Vector3 (0, 0, endLength);
		endLength += pathLengths[nextPlatform];
		nextPlatform = (nextPlatform + 1) % pathSegments.Length;
	}
}