using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class DragKick : MonoBehaviour {

	public Vector3 _dragPos;

	public float force = 5f;

	void OnDrawGizmos() {
		if (Input.GetMouseButton(0)) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, _dragPos);
		}
	}

    void Update() {
		if (Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100F)) {
				_dragPos = hit.point;
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			Vector3 dist = transform.position - _dragPos;
			dist.y = 0f;
			GetComponent<Rigidbody>().AddForce(dist.normalized * force, ForceMode.Impulse);
		}
    }

}
