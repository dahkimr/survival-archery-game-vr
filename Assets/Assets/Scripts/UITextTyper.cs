using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class UITextTyper : MonoBehaviour {

	/* Code from youtube tutorial https://www.youtube.com/watch?v=Mzt1rEEdeOI */

	private Text textComp;

	public float startDelay = 1.1f;
	public float delay = .5f;
	public float delay2 = 0.05f;
	public float typeDelay = 0.01f;
	public string[] txtStrArr;
	public Text pressBtnTxt;
	public Button fireBtn;
	public Button sleepBtn;

    public Canvas deathCanvas;

	private string txtStr;
	private int idx = 0;
	private bool textFinished = false;
	private bool firstDialog = true;
	private Canvas canvas;
	private bool dialogWasEnded = false;
	private bool gameNotStarted = true;

    public static UITextTyper inst;

    void Awake()
    {
        textComp = GetComponent<Text>();
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

    // Use this for initialization
    void Start () {
		canvas = this.GetComponentInParent<Canvas>();
		txtStrArr = new string[] {"I went out to go hunting on my own,\nand mindlessly wandered.",
		"Long after, I realized I didn't know\nwhere I was, and couldn't find my way out.",
        "I set up a small shelter and fire,\nbut I need to stay alive.",
		"Check my hands for my energy and\nbody heat levels.\nIf they drop, I'll die.",
		"I sent a message out before my phone\ndied, I'm sure help will come soon..."};
		pressBtnTxt.enabled = false;
		fireBtn.gameObject.SetActive(false);
		sleepBtn.gameObject.SetActive(false);
		deathCanvas.gameObject.SetActive(false);
		textFinished = false;
		SetText();
		StartCoroutine("TypeIn");
	}

	void Update() {
		if (gameNotStarted && textFinished && SteamVR_Actions.default_GuiPress.GetState (SteamVR_Input_Sources.Any) && idx < txtStrArr.Length) {
			// get rid of old text
			// set new text with coroutine
			if (!dialogWasEnded) {
				textFinished = false;
				txtStr = "";
				SetText();
				StartCoroutine("TypeIn");
			}
		}
		else if (gameNotStarted && textFinished && SteamVR_Actions.default_GuiPress.GetState (SteamVR_Input_Sources.Any) && idx >= txtStrArr.Length 
					|| SteamVR_Actions.default_Skip.GetState (SteamVR_Input_Sources.Any)) {
			// close the canvas
			if (!dialogWasEnded) {
				idx = 50;
				gameNotStarted = false;
				StartCoroutine("CloseCanvas");
			}
		}
	}
	
	public IEnumerator TypeIn() {
		if (firstDialog) {
			yield return new WaitForSeconds(startDelay);
			firstDialog = false;
		}
		else {
			yield return new WaitForSeconds(delay);
		}
		pressBtnTxt.enabled = false;

		for (int i = 0; i <= txtStr.Length; i++) {
			textComp.text = txtStr.Substring(0, i);
			yield return new WaitForSeconds(typeDelay);
		}
		yield return new WaitForSeconds(delay2);
		pressBtnTxt.enabled = true;
		textFinished = true;
	}

	public IEnumerator CloseCanvas() {
		canvas.enabled = false;
		yield return new WaitForSeconds(0.3f);
		Game.inst.GameStart();
		fireBtn.gameObject.SetActive(true);
		sleepBtn.gameObject.SetActive(true);
		dialogWasEnded = true;
	}

	private void SetText() {
		txtStr = txtStrArr[idx];
		idx++;
	}

    public void DisplayDeath()
    {
        Transform targChild = deathCanvas.transform.Find("TargetCount");
        Transform timeChild = deathCanvas.transform.Find("TimeCount");
        Text targTxt = targChild.GetComponent<Text>();
        Text timeTxt = timeChild.GetComponent<Text>();

        targTxt.text = "Targets killed: " + Game.inst.targetsKilled;
        timeTxt.text = "Time survived: " + Game.inst.timeSurvived.ToString("F2") + " hours";

        deathCanvas.gameObject.SetActive(true);
    }

	public void Restart() {
		deathCanvas.gameObject.SetActive(false);
	}
}
