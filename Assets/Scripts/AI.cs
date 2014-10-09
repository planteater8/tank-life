﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Player))]
public abstract class AI : MonoBehaviour {
	
	public float stepLength = 0.01f;
	private float currStep = 0.0f;
	
	public float visibleRadius = 5.0f;
	public float dangerRadius = 1.0f;
	
	protected Player player;

	
	private float currSpeed = 0.0f;
	private float moveTime = 0.0f;
	private float currAngle = 0.0f;
	private float turnTime = 0.0f;
	private float currCannonAngle = 0.0f;
	private float cannonTurnTime = 0.0f;


	public abstract void StepLogic();

	// Use this for initialization
	void Start () {
		player = GetComponent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currStep <= 0.0f) {
			currStep = stepLength;
			StepLogic();
		}
		else {
			currStep -= Time.deltaTime;
		}

		
		if (currSpeed != 0.0f && player.fuel > 0.0f && moveTime > 0.0f) {
			rigidbody.AddForce (transform.up * currSpeed);
			moveTime -= Time.deltaTime;
			player.BurnFuel();
		}
		else {
			moveTime = 0.0f;
			currSpeed = 0.0f;
		}

		
		if (player.cannon && (currCannonAngle > 0.5f || currCannonAngle < -0.5f)) {
			float dir = Mathf.Sign (currCannonAngle);
			float amt = dir * player.turnSpeed * Time.deltaTime;
			player.cannon.Rotate (new Vector3 (0.0f, 0.0f, amt));
			currCannonAngle -= amt;
		}
		
		if (currAngle > 0.5f || currAngle < -0.5f) {
			float dir = Mathf.Sign(currAngle);
			float amt = dir * player.turnSpeed * Time.deltaTime;
			transform.Rotate (new Vector3(0.0f, 0.0f, amt));
			currAngle -= amt;
		}
	}
	
	public Player GetNearestVisiblePlayer() {
		Transform obj = GetNearestVisibleThing ();
		if (obj) {
			return obj.GetComponent<Player>();
		}
		return null;
	}
	
	public Transform GetNearestVisibleThing() {
		RaycastHit hit;
		bool cast = Physics.SphereCast (transform.position, visibleRadius, transform.forward, out hit, visibleRadius);
		if (cast && hit.transform != this.transform) {
			return hit.transform;
		}
		return null;
	}
	
	public bool IsBlocked(Vector3 inDir, float clearance = 0.5f) {
		Vector3 startBase = transform.position + (inDir * clearance);
		Vector3 start1 = startBase + (Vector3.Cross (inDir, transform.forward) * clearance);
		Vector3 start2 = startBase - (Vector3.Cross (inDir, transform.forward) * clearance);
		Debug.DrawLine(startBase, startBase+(inDir*dangerRadius), Color.green);
		return Physics.Raycast (start1, inDir, dangerRadius) || Physics.Raycast (start2, inDir, dangerRadius);
	}

	
	public void MoveForward(float time) {
		currSpeed = player.moveSpeed;
		moveTime = time;
	}
	
	public void MoveBackwards(float time) {
		currSpeed = -player.moveSpeed;
		moveTime = time;
	}
	
	public void StopMoving(float time = 0.0f) {
		moveTime = time;
		currSpeed = 0.0f;
	}
	
	public void TurnTo(float deg) {
		Turn(deg - transform.eulerAngles.z);
	}
	public void Turn(float deg) {
		currAngle = -deg;
	}
	
	public void TurnCannonTo(Vector3 to) {
		player.cannon.LookAt (new Vector3(to.x,to.y,player.cannon.position.z), -Vector3.forward);

	//	player.cannon.LookAt (to);
	}
	public void TurnCannonTo(float deg) {
		TurnCannon(deg - player.cannon.eulerAngles.z);
	}
	public void TurnCannon(float deg) {
		currCannonAngle = deg;
	}
	
	public bool IsTurned() {
		return Mathf.Abs (currAngle) < 2.0f;
	}
	
	public bool IsMoved() {
		return moveTime <= 0.0f;
	}
}
