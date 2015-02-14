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
									 yawRotation,
									 maxPitch,
									 maxRoll,
									 maxYaw;

	bool accelerating,
		 decelerating,
		 yawingLeft,
		 yawingRight,
		 rollingRight,
		 rollingLeft,
		 pitchingUp,
		 pitchingDown;

	[SerializeField] protected float currentSpeed,
									 currentPitch,
									 currentYaw,
									 currentRoll;

	SafeCoroutine yawCoroutine,
				  pitchCoroutine,
				  rollCoroutine;

	IEnumerator Primer(){
		yield break;
	}

	void Awake(){
		yawCoroutine = this.StartSafeCoroutine(Primer());
		pitchCoroutine = this.StartSafeCoroutine(Primer());
		rollCoroutine = this.StartSafeCoroutine(Primer());
	}

	void Update () {
		// Accelerate
		if (Input.GetKeyDown(KeyCode.W)){
			accelerating = true;
		}
		// Decelerate
		if (Input.GetKeyDown(KeyCode.S)){
			decelerating = true;
		}
		// Yaw Left
		if (Input.GetKeyDown(KeyCode.A)){
			if (!yawingLeft){
				if (yawCoroutine.IsRunning){
					yawCoroutine.Stop();
				}
				yawCoroutine = this.StartSafeCoroutine(YawCoroutine(false, -maxYaw));
			}

			yawingLeft = true;
		}
		// Yaw Right
		if (Input.GetKeyDown(KeyCode.D)){
			if (!yawingRight){
				if (yawCoroutine.IsRunning){
					yawCoroutine.Stop();
				}
				yawCoroutine = this.StartSafeCoroutine(YawCoroutine(true, maxYaw));
			}

			yawingRight = true;
		}
		// Roll Left
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			if (!rollingLeft){
				if (rollCoroutine.IsRunning){
					rollCoroutine.Stop();
				}
				rollCoroutine = this.StartSafeCoroutine(RollCoroutine(true, maxRoll));
			}

			rollingLeft = true;
		}
		// Roll Right
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			if (!rollingRight){
				if (rollCoroutine.IsRunning){
					rollCoroutine.Stop();
				}
				rollCoroutine = this.StartSafeCoroutine(RollCoroutine(false, -maxRoll));
			}

			rollingRight = true;
		}
		// Pitch Down
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			if (!pitchingDown){
				if (pitchCoroutine.IsRunning){
					pitchCoroutine.Stop();
				}
				pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(true, maxPitch));
			}

			pitchingDown = true;
		}
		// Pitch Up
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			if (!pitchingUp){
				if (pitchCoroutine.IsRunning){
					pitchCoroutine.Stop();
				}
				pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(false, -maxPitch));
			}

			pitchingUp = true;
		}

		// Accelerate
		if (Input.GetKeyUp(KeyCode.W)){
			accelerating = false;
		}
		// Decelerate
		if (Input.GetKeyUp(KeyCode.S)){
			decelerating = false;
		}
		// Yaw Right
		if (Input.GetKeyUp(KeyCode.D)){
			yawingRight = false;

			if (yawCoroutine.IsRunning){
				yawCoroutine.Stop();
			}
			yawCoroutine = this.StartSafeCoroutine(YawCoroutine(false, 0));
		}
		// Yaw Left
		if (Input.GetKeyUp(KeyCode.A)){
			yawingLeft = false;
			if (yawCoroutine.IsRunning){
				yawCoroutine.Stop();
			}
			yawCoroutine = this.StartSafeCoroutine(YawCoroutine(true, 0));
		}
		// Roll Left
		if (Input.GetKeyUp(KeyCode.LeftArrow)){
			rollingLeft = false;

			if (rollCoroutine.IsRunning){
				rollCoroutine.Stop();
			}
			rollCoroutine = this.StartSafeCoroutine(RollCoroutine(false, 0));

		}
		// Roll Right
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			rollingRight = false;

			if (rollCoroutine.IsRunning){
				rollCoroutine.Stop();
			}
			rollCoroutine = this.StartSafeCoroutine(RollCoroutine(true, 0));

		}
		// Pitch Down
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			pitchingDown = false;

			if (pitchCoroutine.IsRunning){
				pitchCoroutine.Stop();
			}
			pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(false, 0));
		}
		// Pitch Up
		if (Input.GetKeyUp(KeyCode.DownArrow)){
			pitchingUp = false;
			if (pitchCoroutine.IsRunning){
				pitchCoroutine.Stop();
			}
			pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(true, 0));
		}

		if (accelerating){
			currentSpeed = Mathf.Min(maxSpeed, currentSpeed + acceleration * Time.deltaTime);
		}
		if (decelerating){
			currentSpeed = Mathf.Max(minSpeed, currentSpeed - deceleration * Time.deltaTime);
		}

		transform.Rotate(currentPitch, currentYaw, currentRoll);

		var pos = transform.position;
		pos += transform.forward * currentSpeed;
		transform.position = pos;
	}

	IEnumerator RollCoroutine(bool increase, float target){
		if (increase){
			while (currentRoll < target){
				currentRoll += rollRotation;
				yield return null;
			}
		}
		else{
			while (currentRoll > target){
				currentRoll -= rollRotation;
				yield return null;
			}
		}
	}

	IEnumerator PitchCoroutine(bool increase, float target){
		if (increase){
			while (currentPitch < target){
				currentPitch += pitchRotation;
				yield return null;
			}
		}
		else{
			while (currentPitch > target){
				currentPitch -= pitchRotation;
				yield return null;
			}
		}
	}

	IEnumerator YawCoroutine(bool increase, float target){
		if (increase){
			while (currentYaw < target){
				currentYaw += yawRotation;
				yield return null;
			}
		}
		else{
			while (currentYaw > target){
				currentYaw -= yawRotation;
				yield return null;
			}
		}
	}
}
