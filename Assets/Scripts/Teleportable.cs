﻿using UnityEngine;
using System.Collections;

public class Teleportable : MonoBehaviour
{

    public PlayerControl Player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool InTeleporter     { get; set; }

    public void TeleportTo(Vector3 pos)
    {
        Player.SetPosition(pos);

    }
}
