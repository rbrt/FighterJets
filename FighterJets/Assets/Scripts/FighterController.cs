using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FighterController : MonoBehaviour {

	[SerializeField] protected float acceleration,
									 deceleration,
									 maxSpeed,
									 minSpeed,
									 pitchRotation,
									 rollRotation,
									 yawRotation;

	bool accelerating,
		 decelerating,
		 yawingLeft,
		 yawingRight,
		 rollingRight,
		 rollingLeft,
		 pitchingUp,
		 pitchingDown;

	[SerializeField] protected float currentSpeed;

	void Update () {
		// Accelerate
		if (Input.GetKeyDown(KeyCode.W)){
			accelerating = true;
		}
		// Yaw Left
		if (Input.GetKeyDown(KeyCode.A)){
			yawingLeft = true;
		}
		// Decelerate
		if (Input.GetKeyDown(KeyCode.S)){
			decelerating = true;
		}
		// Yaw Right
		if (Input.GetKeyDown(KeyCode.D)){
			yawingRight = true;
		}
		// Roll Left
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			rollingLeft = true;
		}
		// Roll Right
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			rollingRight = true;
		}
		// Pitch Down
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			pitchingDown = true;
		}
		// Pitch Up
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			pitchingUp = true;
		}

		// Accelerate
		if (Input.GetKeyUp(KeyCode.W)){
			accelerating = false;
		}
		// Yaw Left
		if (Input.GetKeyUp(KeyCode.A)){
			yawingLeft = false;
		}
		// Decelerate
		if (Input.GetKeyUp(KeyCode.S)){
			decelerating = false;
		}
		// Yaw Right
		if (Input.GetKeyUp(KeyCode.D)){
			yawingRight = false;
		}
		// Roll Left
		if (Input.GetKeyUp(KeyCode.LeftArrow)){
			rollingLeft = false;
		}
		// Roll Right
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			rollingRight = false;
		}
		// Pitch Down
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			pitchingDown = false;
		}
		// Pitch Up
		if (Input.GetKeyUp(KeyCode.DownArrow)){
			pitchingUp = false;
		}

		if (accelerating){
			currentSpeed = Mathf.Min(maxSpeed, currentSpeed + acceleration * Time.deltaTime);
		}
		if (decelerating){
			currentSpeed = Mathf.Max(minSpeed, currentSpeed - deceleration * Time.deltaTime);
		}
		if (yawingLeft){
			transform.Rotate(0, -yawRotation, 0);
		}
		if (yawingRight){
			transform.Rotate(0, yawRotation, 0);
		}
		if (rollingLeft){
			transform.Rotate(0, 0, rollRotation);
		}
		if (rollingRight){
			transform.Rotate(0, 0, -rollRotation);
		}
		if (pitchingDown){
			transform.Rotate(pitchRotation, 0, 0);
		}
		if (pitchingUp){
			transform.Rotate(-pitchRotation, 0, 0);
		}

		var pos = transform.position;
		pos += transform.forward * currentSpeed;
		transform.position = pos;
	}
}
