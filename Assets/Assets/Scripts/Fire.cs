using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour {

	public bool fireOn = false;
	public ParticleSystem firePS;
	public ParticleSystem sparkPS;
	public Light fireLight;
	public static Fire inst;

	private bool fireIsOn = false;
    private AudioSource audSrc;

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
        audSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void LightFire () {
		if (!fireIsOn) {
			firePS.Play ();
			sparkPS.Play ();
            audSrc.Play();
			fireLight.intensity = 1.5f;
			fireIsOn = true;
		} 
	}

    public void KillFire()
    {
        if (fireIsOn)
        {
            firePS.Stop();
            sparkPS.Stop();
            audSrc.Stop();
            fireLight.intensity = 0.0f;
            fireIsOn = false;
        }
    }

	public bool GetFire() {
		return fireIsOn;
	}
}
