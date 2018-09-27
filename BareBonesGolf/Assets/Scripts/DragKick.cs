using UnityEngine;
using System.Collections;
// using Prime31.ZestKit;

[RequireComponent(typeof(Rigidbody))]

public class DragKick : MonoBehaviour {

	public Transform _cylinder;

	public Vector3 _dragPos;

	public float force = 5f;

	void Start() {
		// transform.localScale = Vector3.one / 4f;
		// transform.ZKlocalScaleTo(Vector3.one, 1f).setLoops(LoopType.PingPong).setEaseType(EaseType.ElasticIn).start();
	}

	void OnDrawGizmos() {
		if (Input.GetMouseButton(0)) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, _dragPos);
		}
	}

    void FixedUpdate() {
		if (Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100F)) {
				_dragPos = hit.point;
				_dragPos += Vector3.up * 0.25f;

				_cylinder.position = _dragPos + (transform.position - _dragPos)/4f;
				_cylinder.LookAt(transform.position);
				Vector3 scale = _cylinder.localScale;
				scale.z = (transform.position - _dragPos).magnitude/4f;
				_cylinder.localScale = scale;
			}

			Time.timeScale = 0.5f;
		} else {
			Time.timeScale = 1f;
		}

		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		if (Input.GetMouseButtonUp(0)) {
			Vector3 dist = transform.position - _dragPos;
			dist.y = 0f;
			GetComponent<Rigidbody>().AddForce(dist.normalized * force, ForceMode.Impulse);
		}
    }

}
