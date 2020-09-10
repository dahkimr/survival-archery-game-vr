using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Arrow : MonoBehaviour {

	// code following tutorial https://www.youtube.com/watch?v=Dh7Wwqs-s2c
	private bool wasAttached = false;

	private void Update() {
		if (wasAttached)
			DestroyArrow();
	}

	private void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "Golden Bow") {
			AttachArrow ();
		}
	}

	private void OnTriggerStay (Collider other) {
		if (other.gameObject.name == "Golden Bow") {
			AttachArrow ();
		}
	}

	private void AttachArrow () {
		if (SteamVR_Actions.default_PullArrow.GetStateDown (SteamVR_Input_Sources.LeftHand)) {
			ArrowManager.inst.AttachBowToArrow ();
			wasAttached = true;
		}
	}

	private void DestroyArrow() {
		Destroy(gameObject, 10);
	}
}