using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {

	[SerializeField] protected Transform target;

	void Awake(){
		ShootMissile(GameObject.Find("MissileTarget").transform);
	}

	public void ShootMissile(Transform target){
		this.target = target;
		this.StartSafeCoroutine(SmarterShoot());
	}

	IEnumerator SmarterShoot(){
		float targetDistance = 5;
		float force = 1000;
		float maxStep = 500f;

		var rigidbody = GetComponent<Rigidbody>();
		while (Vector3.Distance(transform.position, target.position) > targetDistance){
			rigidbody.AddForce(transform.forward * force);

			transform.rotation = Quaternion.RotateTowards(transform.rotation,
														  Quaternion.LookRotation(target.position - transform.position),
														  maxStep * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator Shoot(){
		//DrawCurve();
		float step = 0;

		Vector3 pointA = transform.position,
				pointC = target.position;

		var angle = Vector3.Angle(pointC - pointA, transform.right);
		if (pointC.y < pointA.y){
			angle *= -1;
		}

		Vector3 pointB = Vector3.Cross(pointA, pointC).normalized * angle;

		while (step <= 1){
			transform.position = GetPointOnBezierCurve(pointA, pointB, pointC, step);
			step += .02f;
			yield return null;
		}
	}

	void DrawCurve(){
		Vector3 pointA = transform.position,
				pointC = target.position;

		Vector3 midpoint = GetMidPoint(pointA, pointC);

		var angle = Vector3.Angle(pointC - pointA, transform.right);
		if (pointC.y < pointA.y){
			angle *= -1;
		}

		Vector3 pointB = Vector3.Cross(pointA, pointC).normalized * angle;

		Debug.DrawLine(pointA, pointB, Color.blue, 10f);

		for (float i = 0; i < 1; i+= .01f){
			Debug.DrawLine(midpoint, GetPointOnBezierCurve(pointA, pointB, pointC, i), Color.red, 10f);
		}

	}

	Vector3 GetMidPoint(Vector3 a, Vector3 b){
		return new Vector3((a.x + b.x) / 2,
						   (a.y + b.y) / 2,
						   (a.z + b.z) / 2);
	}

	Vector3 GetPointOnBezierCurve(Vector3 point1, Vector3 point2, Vector3 point3, float step){
		Vector3 point = new Vector3(Bezier(point1.x, point2.x, point3.x, step),
									Bezier(point1.y, point2.y, point3.y, step),
									Bezier(point1.z, point2.z, point3.z, step));
		return point;
	}

	float Bezier(float point0, float point1, float point2, float step){
		// (1-t)^2 * P0 +
		//  2(1-t) * t * P1 +
		//  t^2 * P2

		return Mathf.Pow((1 - step), 2) * point0 + 2 * (1 - step) * step * point1 + Mathf.Pow(step, 2) * point2;
	}
}
