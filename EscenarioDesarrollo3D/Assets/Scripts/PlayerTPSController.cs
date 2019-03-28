﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPSController : MonoBehaviour {

    public Camera cam;
    private InputData input;
    private CharacterAnimBasedMovement characterMovement;


	void Start ()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
	}
	    
	void Update ()
    {
        input.GetInput();

        characterMovement.MoveCharacter(input.hMovement, input.vMovement, cam, input.jump, input.dash, input.press);

    }
}
