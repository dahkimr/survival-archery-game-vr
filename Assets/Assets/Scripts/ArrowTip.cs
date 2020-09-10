using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ArrowTip : MonoBehaviour {

	private bool firstSleep = true;
	private GameObject target = null;
	private Vector3 hitPos;

	public AudioSource hitAudSrc;
    public SoundPlayOneshot airReleaseSound;

    public static ArrowTip inst;
    void Awake()
    {
        if (!inst)
            inst = this;
    }

    void OnDestroy()
    {
        if (inst)
        {
            inst = null;
        }
    }

    public void PlayAirRelease()
    {
        airReleaseSound.Play();
    }

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "lightfire") {
			Fire.inst.LightFire ();
		}
		else if (other.tag == "sleep") {
			// display are you sure you want to sleep now?
			// display yes and no buttons
			// if yes
			// fade camera, open next scene
			FadeToBlack();
			Invoke("Sleep", .5f);
			Invoke("FadeFromBlack", 1.5f);
			
		}
        else if (other.tag == "restart")
        {
            FadeToBlack();
            Invoke("RestartGame", .5f);
            Invoke("FadeFromBlack", 1.5f);
        }
		if (other.tag == "target") {
			hitAudSrc.Play();
			Debug.Log("ontrigger with: " + other.name);
			Destroy(other.gameObject);
            Game.inst.IncreaseEnergy();
            Game.inst.IncreaseTargetsKilled();
			target = null;
		}
		// if you hit the alarmcollider
		if (other.tag == "alarmzone") {
			Debug.Log("ontrigger with: " + other.name);
			target = other.transform.parent.gameObject;
			Debug.Log(target.name);
			// get where hit alarmzone
			hitPos = this.transform.position;
			Invoke("CheckIfTargetKilled", .2f);
		}
		if (other.tag == "ground") {
			// stick to object so basically pause the position
			GameObject arr = this.transform.parent.gameObject;
			Rigidbody arrRB = arr.GetComponent<Rigidbody> ();

			arrRB.useGravity = false;
			arrRB.velocity = Vector3.zero;
		}
	}

	private void OnTriggerStay (Collider other) {
		if (other.tag == "ground") {
				// stick to object so basically pause the position
				GameObject arr = this.transform.parent.gameObject;
				Rigidbody arrRB = arr.GetComponent<Rigidbody> ();

				arrRB.useGravity = false;
				arrRB.velocity = Vector3.zero;
			}
	}

	private void FadeToBlack() {
		Debug.Log("fadetoblack was called");
		SteamVR_Fade.Start(Color.clear, 0f);
		SteamVR_Fade.Start(Color.black, .5f);
		Game.inst.gameStarted = false;
	}
	private void FadeFromBlack() {
		Debug.Log("fadefromblack was called");
		SteamVR_Fade.Start(Color.black, 0f);
		SteamVR_Fade.Start(Color.clear, 1f);
		Game.inst.gameStarted = true;
	}
	private void GameStart() {
        Game.inst.GameStart();
    }
	private void Sleep() {
		Game.inst.PassTimeSleep();
	}
	private void CheckIfTargetKilled() {
		if (target) {
			// make target run
			TargetManager.inst.MakeTargetRun(target, hitPos);
			target = null;
		}
	}
    private void RestartGame()
    {
		Fire.inst.KillFire ();
		UITextTyper.inst.Restart();
		Game.inst.ResetPosition();
        Game.inst.GameStart();
    }
}
