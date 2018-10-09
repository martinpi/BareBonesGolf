using UnityEngine;
using System.Collections;
using Prime31.ZestKit;

[RequireComponent(typeof(Rigidbody))]

public class DragKick : MonoBehaviour {

	public Transform _cylinder;

	public Vector3 _dragPos;

	public float force = 5f;

	private Rigidbody _body;

	void Start() {
		_body = GetComponent<Rigidbody>();
		_cylinder.gameObject.SetActive(false);

		// transform.localScale = Vector3.one / 4f;
		// transform.ZKlocalScaleTo(Vector3.one, 1f).setLoops(LoopType.PingPong).setEaseType(EaseType.ElasticIn).start();
	}

	// void OnDrawGizmos() {
	// 	if (Input.GetMouseButton(0)) {
	// 		Gizmos.color = Color.red;
	// 		Gizmos.DrawLine(transform.position, _dragPos);
	// 	}
	// }

	public IEnumerator hideCylinder(float delay) {
		yield return new WaitForSeconds(delay);
		_cylinder.gameObject.SetActive(false);
	}

	public IEnumerator shootDelayed(Vector3 force, float delay) {
		yield return new WaitForSeconds(delay);

		_body.AddForce(force, ForceMode.Impulse);
		_body.constraints = RigidbodyConstraints.None;

		StartCoroutine(hideCylinder(0.1f));
	}

    void FixedUpdate() {
		if (Input.GetMouseButtonDown(0) && !_cylinder.gameObject.activeSelf) {
			_cylinder.gameObject.SetActive(true);
		}

		if (Input.GetMouseButton(0) && _cylinder.gameObject.activeSelf) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100F)) {
				_dragPos = hit.point;
				_dragPos += Vector3.up * 0.25f;
			}

				_cylinder.position = _dragPos + (transform.position - _dragPos)/4f;
				_cylinder.LookAt(transform.position);
				Vector3 scale = _cylinder.localScale;
				scale.z = (transform.position - _dragPos).magnitude/4f;
				_cylinder.localScale = scale;

			Time.timeScale = 0.3f;
		} else {
			Time.timeScale = 1f;
		}

		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		if (Input.GetMouseButtonUp(0) && _cylinder.gameObject.activeSelf) {
			float duration = 0.3f;
			
			Vector3 dist = transform.position - _dragPos;
			dist.y = 0f;
			StartCoroutine(shootDelayed(dist.normalized * force, duration));

			_body.constraints = RigidbodyConstraints.FreezeAll;

			_cylinder.ZKpositionTo(transform.position - (transform.position - _dragPos)/4f, duration).setEaseType(EaseType.ExpoIn).start();
		}
    }

}
