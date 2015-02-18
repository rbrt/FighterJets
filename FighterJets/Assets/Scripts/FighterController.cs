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
									 missileDelayTime,
									 laserDelayTime;

	[SerializeField] protected Transform leftMissileSpawn,
										 leftMissileTarget,
										 rightMissileSpawn,
										 rightMissileTarget,
										 leftLaserSpawn,
										 rightLaserSpawn;

	[SerializeField] protected GameObject missilePrefab,
										  laserPrefab,
										  leftChargeParticle,
										  rightChargeParticle;


	bool accelerating,
		 decelerating,
		 yawingLeft,
		 yawingRight,
		 rollingRight,
		 rollingLeft,
		 pitchingUp,
		 pitchingDown,
		 canShootMissiles,
		 canShootLasers,
		 chargingLaser,
		 boosting;

	[SerializeField] protected float currentSpeed,
									 currentPitch,
									 currentYaw,
									 currentRoll;

	[SerializeField] protected Material chargeParticle,
										chargeLaserMaterial;
	[SerializeField] protected Color baseParticleColor,
									 chargedParticleColor;

	[SerializeField] protected Boost leftBoost,
									 rightBoost;

	[SerializeField] protected PlayerShield playerShield;

	float delayBeforeChargingLaser = .75f,
		  laserMaxChargeDuration = 3f,
		  lastLaserTime,
		  boostAmount,
		  maximumBoost = 5;

	SafeCoroutine yawCoroutine,
				  pitchCoroutine,
				  rollCoroutine,
				  laserChargeCoroutine;

	static FighterController instance;

	public static FighterController Instance{
		get { return instance; }
	}

	public float BoostAmount{
		get { return boostAmount; }
		set { boostAmount = value; }
	}

	IEnumerator Primer(){
		yield break;
	}

	void Awake(){
		yawCoroutine = this.StartSafeCoroutine(Primer());
		pitchCoroutine = this.StartSafeCoroutine(Primer());
		rollCoroutine = this.StartSafeCoroutine(Primer());
		laserChargeCoroutine = this.StartSafeCoroutine(Primer());
		canShootMissiles = true;
		canShootLasers = true;

		instance = this;
	}

    void YawRight(){
        if (!yawingRight)
        {
            if (yawCoroutine.IsRunning){
                yawCoroutine.Stop();
            }
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(true, maxYaw));
        }

        yawingRight = true;
    }

    void YawLeft(){
        if (!yawingLeft)
        {
            if (yawCoroutine.IsRunning){
                yawCoroutine.Stop();
            }
            yawCoroutine = this.StartSafeCoroutine(YawCoroutine(false, -maxYaw));
        }

        yawingLeft = true;
    }

    void YawLeftReturn(){
        yawingLeft = false;
        if (yawCoroutine.IsRunning){
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

		if (Input.GetKeyDown(KeyCode.RightShift)){
			if (!boosting){
				boosting = true;
				leftBoost.StartBoost();
				rightBoost.StartBoost();
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)){
			if (canShootLasers){
				if (laserChargeCoroutine.IsRunning){
					laserChargeCoroutine.Stop();
				}

				lastLaserTime = Time.time;
				canShootLasers = false;
				this.StartSafeCoroutine(LaserTimer());
				ShootLasers();
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

		if (Input.GetKeyUp(KeyCode.Space)){
			if (Time.time - lastLaserTime > delayBeforeChargingLaser){
				ShootLasers(chargePercent: Mathf.Min(1.0f, (Time.time - lastLaserTime) / laserMaxChargeDuration));
				this.StartSafeCoroutine(LaserTimer());
			}
			laserChargeCoroutine = this.StartSafeCoroutine(FadeOutLaserCharge());
			chargingLaser = false;
		}

		if (Input.GetKeyUp(KeyCode.RightShift)){

			boosting = false;
			leftBoost.StopBoost();
			rightBoost.StopBoost();
		}

		if (accelerating){
			currentSpeed = Mathf.Min(maxSpeed + boostAmount * maximumBoost,
								     currentSpeed + (acceleration + (boostAmount * maximumBoost)) * Time.deltaTime);
		}
		if (decelerating){
			currentSpeed = Mathf.Max(minSpeed, currentSpeed - deceleration * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.Space) && !chargingLaser && Time.time - lastLaserTime > delayBeforeChargingLaser){
			chargingLaser = true;
			leftChargeParticle.SetActive(true);
			rightChargeParticle.SetActive(true);
		}

		if (chargingLaser){
			chargeParticle.SetColor("_TintColor", Color.Lerp(baseParticleColor,
															chargedParticleColor,
															(Time.time - lastLaserTime) / laserMaxChargeDuration)
								    );
		}

		transform.Rotate(currentPitch, currentYaw, currentRoll);

		var pos = transform.position;
		pos += transform.forward * currentSpeed;
		transform.position = pos;
	}

	void ShootLasers(float chargePercent = 0){
		var leftLaser = (GameObject.Instantiate(laserPrefab,
							   					leftLaserSpawn.transform.position,
							   					leftLaserSpawn.transform.rotation) as GameObject).GetComponent<Laser>();

		var rightLaser = (GameObject.Instantiate(laserPrefab,
							   					 rightLaserSpawn.transform.position,
							   					 rightLaserSpawn.transform.rotation) as GameObject).GetComponent<Laser>();

		if (chargePercent != 0){
			chargeLaserMaterial.SetColor("_TintColor", chargeParticle.GetColor(("_TintColor")));
			leftLaser.GetComponent<Renderer>().sharedMaterial = chargeLaserMaterial;
			rightLaser.GetComponent<Renderer>().sharedMaterial = chargeLaserMaterial;
		}

		leftLaser.FireLaser(chargePercent);
		rightLaser.FireLaser(chargePercent);
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
		float rotationAmount = target == 0 ? rollRotation * 1.5f : rollRotation;

		if (increase){
			while (currentRoll < target){
				currentRoll += currentRoll < 0 ? rotationAmount * 2 : rotationAmount;
				yield return null;
			}
		}
		else{
			while (currentRoll > target){
				currentRoll -= currentRoll > 0 ? rotationAmount * 2 : rotationAmount;
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

	IEnumerator FadeOutLaserCharge(){
		Color color = chargeParticle.GetColor("_TintColor");
		float duration = .2f;
		for (float i = 1; i > 0; i -= Time.deltaTime / duration){
			color.a = i;
			chargeParticle.SetColor("_TintColor", color);
			yield return null;
		}

		leftChargeParticle.SetActive(false);
		rightChargeParticle.SetActive(false);
	}

	IEnumerator MissileTimer(){
		float startTime = Time.time;
		while (Time.time - startTime < missileDelayTime){
			yield return null;
		}

		canShootMissiles = true;
	}

	IEnumerator LaserTimer(){
		float startTime = Time.time;
		while (Time.time - startTime < laserDelayTime){
			yield return null;
		}

		canShootLasers = true;
	}
}
