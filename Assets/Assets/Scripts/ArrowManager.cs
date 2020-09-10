using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ArrowManager : MonoBehaviour {

	// code following tutorial https://www.youtube.com/watch?v=Dh7Wwqs-s2c

	public GameObject arrPrefab;
	public GameObject strAttPnt;
	public GameObject arrStrtPnt;
	public GameObject strStrtPnt;

	public static ArrowManager inst;

	private GameObject currArr;
	private bool isAttached = false;
	private float dist;

	void Awake () {
		if (!inst)
			inst = this;
	}

	void OnDestroy () {
		if (inst) {
			inst = null;
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		AttachArrow ();
		PullString ();
	}

	private void PullString () {
		if (isAttached) {
			// Bow.inst.PlayBowPull();
			dist = (strStrtPnt.transform.position - this.gameObject.transform.position).magnitude;
			strAttPnt.transform.localPosition = strStrtPnt.transform.localPosition + new Vector3 (0.0f, -dist * 0.1f, 0.0f); 

			if (SteamVR_Actions.default_PullArrow.GetStateUp (SteamVR_Input_Sources.LeftHand)) {
				Bow.inst.StopPlayBowPull();
				Fire ();
				strAttPnt.transform.localPosition = strStrtPnt.transform.localPosition;
			}	
		}
	}

	private void Fire() {
		Rigidbody currArrRB = currArr.GetComponent<Rigidbody> ();
		currArr.transform.parent = null;
		Debug.Log("dist: " + dist);
		currArrRB.AddForce(currArr.transform.forward * (dist * 35f), ForceMode.Impulse);
		currArrRB.useGravity = true;
		
		// ArrowTip.inst.PlayAirRelease();

		currArr = null;
		isAttached = false;
	}

	private void AttachArrow () {
		if (!currArr) {
			currArr = Instantiate (arrPrefab);
			currArr.transform.parent = this.gameObject.transform;
			currArr.transform.localPosition = new Vector3 (0.0f, -0.27f, 0.36f);
			currArr.transform.localRotation = Quaternion.Euler (30f, 0f, 0f);
		}
	}

	public void AttachBowToArrow () {
		currArr.transform.parent = strAttPnt.transform;
		currArr.transform.position = arrStrtPnt.transform.position;
		currArr.transform.rotation = arrStrtPnt.transform.rotation;
		isAttached = true;
		Bow.inst.PlayBowPull();
	}
}