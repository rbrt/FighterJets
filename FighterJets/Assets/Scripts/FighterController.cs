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
									 maxYaw,
									 missileDelayTime;

	[SerializeField] protected Transform leftMissileSpawn,
										 leftMissileTarget,
										 rightMissileSpawn,
										 rightMissileTarget;

	[SerializeField] protected GameObject missilePrefab;


	bool accelerating,
		 decelerating,
		 yawingLeft,
		 yawingRight,
		 rollingRight,
		 rollingLeft,
		 pitchingUp,
		 pitchingDown,
		 canShootMissiles;

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
		canShootMissiles= true;
	}

    void YawRight(){
        if (!yawingRight)
        {
            if (yawCoroutine.IsRunning)
            {
                yawCoroutine.Stop();
            }
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(true, maxYaw));
        }

        yawingRight = true;
    }

    void YawLeft(){
        if (!yawingLeft)
        {
            if (yawCoroutine.IsRunning)
            {
                yawCoroutine.Stop();
            }
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(false, -maxYaw));
        }

        yawingLeft = true;
    }

    void YawLeftReturn(){
        yawingLeft = false;
        if (yawCoroutine.IsRunning)
        {
            yawCoroutine.Stop();
        }

        if (Input.GetKey(KeyCode.D)) {
            yawingRight = false;
            YawRight();
        }
        else { 
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(true, 0));
        }
    }

    void YawRightReturn(){
        yawingRight = false;

        if (yawCoroutine.IsRunning) {
            yawCoroutine.Stop();
        }
        if (Input.GetKey(KeyCode.A)){
            yawingLeft = false;
            YawLeft();
        }
        else{
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(false, 0));
        }
    }

    void RollRight(){
        if (!rollingRight) {
            if (rollCoroutine.IsRunning) {
                rollCoroutine.Stop();
            }
            rollCoroutine = this.StartSafeCoroutine(RollCoroutine(false, -maxRoll));
        }

        rollingRight = true;
    }
    
    void RollLeft(){
        if (!rollingLeft) {
            if (rollCoroutine.IsRunning) {
                rollCoroutine.Stop();
            }
            rollCoroutine = this.StartSafeCoroutine(RollCoroutine(true, maxRoll));
        }

        rollingLeft = true;
    }

    void RollRightReturn(){
        rollingRight = false;

        if (rollCoroutine.IsRunning) {
            rollCoroutine.Stop();
        }

        if (Input.GetKey(KeyCode.LeftArrow)){ 
            rollingLeft = false;
            RollLeft();
        }
        else{
            rollCoroutine = this.StartSafeCoroutine(RollCoroutine(true, 0));
        }
    }

    void RollLeftReturn(){
        rollingLeft = false;

        if (rollCoroutine.IsRunning) {
            rollCoroutine.Stop();
        }

        if (Input.GetKey(KeyCode.RightArrow)){
            rollingRight = false;
            RollRight();
        }
        else {
            rollCoroutine = this.StartSafeCoroutine(RollCoroutine(false, 0));
        }
    }

    void PitchUp(){
        if (!pitchingUp) {
            if (pitchCoroutine.IsRunning) {
                pitchCoroutine.Stop();
            }
            pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(false, -maxPitch));
        }

        pitchingUp = true;
    }

    void PitchDown(){
        if (!pitchingDown) {
            if (pitchCoroutine.IsRunning) {
                pitchCoroutine.Stop();
            }
            pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(true, maxPitch));
        }

        pitchingDown = true;
    }

    void PitchUpReturn(){
        pitchingUp = false;
        if (pitchCoroutine.IsRunning) {
            pitchCoroutine.Stop();
        }

        if (Input.GetKey(KeyCode.UpArrow)) { 
            pitchingDown = false;
            PitchDown();
        }
        else {
            pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(true, 0));
        }
        
    }

    void PitchDownReturn(){
        pitchingDown = false;

        if (pitchCoroutine.IsRunning) {
            pitchCoroutine.Stop();
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            pitchingUp = false;
            PitchUp();
        }
        else { 
            pitchCoroutine = this.StartSafeCoroutine(PitchCoroutine(false, 0));
        }
    }

	void Update (){
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
            YawLeft();
		}
		// Yaw Right
		if (Input.GetKeyDown(KeyCode.D)){
            YawRight();
		}
		// Roll Left
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			RollLeft();
		}
		// Roll Right
		if (Input.GetKeyDown(KeyCode.RightArrow)){
            RollRight();
		}
		// Pitch Down
		if (Input.GetKeyDown(KeyCode.UpArrow)){
			PitchDown();
		}
		// Pitch Up
		if (Input.GetKeyDown(KeyCode.DownArrow)){
			PitchUp();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift)){
			if (canShootMissiles){
				canShootMissiles = false;
				this.StartSafeCoroutine(MissileTimer());
				ShootMissiles();
			}
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
			YawRightReturn();
		}
		// Yaw Left
		if (Input.GetKeyUp(KeyCode.A)){
            YawLeftReturn();
		}
		// Roll Left
		if (Input.GetKeyUp(KeyCode.LeftArrow)){
			RollLeftReturn();

		}
		// Roll Right
		if (Input.GetKeyUp(KeyCode.RightArrow)){
			RollRightReturn();

		}
		// Pitch Down
		if (Input.GetKeyUp(KeyCode.UpArrow)){
			PitchDownReturn();
		}
		// Pitch Up
		if (Input.GetKeyUp(KeyCode.DownArrow)){
			PitchUpReturn();
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

	void ShootMissiles(){
		var leftMissile = GameObject.Instantiate(missilePrefab,
												 leftMissileSpawn.transform.position,
												 leftMissileSpawn.transform.rotation) as GameObject;

		var rightMissile = GameObject.Instantiate(missilePrefab,
												  rightMissileSpawn.transform.position,
                                                  rightMissileSpawn.transform.rotation) as GameObject;
        
        leftMissile.transform.parent = transform;
        rightMissile.transform.parent = transform;

        leftMissile.GetComponent<MissileLogic>().ShootMissile(GameObject.Find("MissileTarget").transform,
                                                              leftMissileSpawn.transform,
                                                              leftMissileTarget.transform);
        rightMissile.GetComponent<MissileLogic>().ShootMissile(GameObject.Find("MissileTarget").transform,
                                                               rightMissileSpawn.transform,
                                                               rightMissileTarget.transform);
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

        currentRoll = target;
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

        currentPitch = target;
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

        currentYaw = target;
	}

	IEnumerator MissileTimer(){
		float startTime = Time.time;
		while (Time.time - startTime > missileDelayTime){
			yield return null;
		}

		canShootMissiles = true;
	}
}
