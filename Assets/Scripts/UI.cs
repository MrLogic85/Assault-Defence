﻿using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

    public Transform baseContent;

	// Use this for initialization
	void Start () {
        Base[] bases = TurretComponentLibrary.instance.GetBases();
        for (int i = 0; i < bases.Length; i++)
        {
            GUITexture baseTexture = bases[i].GetComponent<GUITexture>();
            if (baseTexture != null)
            {
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
