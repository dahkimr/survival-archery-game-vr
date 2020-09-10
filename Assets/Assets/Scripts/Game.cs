using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	public Material daySkybox;
	public Material middaySkybox;
	public Material nightSkybox;
	public Material deathSkybox;
	public Light lighting;
	public Light dayLighting;
	public Light middayLighting;
	public Light nightLighting;
	public Light deathLighting;

	public Image bodyHeatImg;
    public Image energyImg;

    public int timeCount = 6;

    public bool isNearFire = false;

	private float time = 17.0f;
	private int timeOfDayIdx = 3;
	public bool gameStarted = false;

    public int targetsKilled = 0;
    public float timeSurvived = 0.0f;

	public static Game inst;

	private Vector3 startPlayerPos;

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
		startPlayerPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeOfDayIdx != 1 && time >=0 && time <= 6) {
			RenderSettings.skybox = daySkybox;
			lighting.intensity = dayLighting.intensity;
			lighting.color = dayLighting.color;
			timeOfDayIdx = 1;
		}
		else if (timeOfDayIdx != 2 && time > 6 && time <= 12) {
			RenderSettings.skybox = middaySkybox;
			lighting.intensity = middayLighting.intensity;
			lighting.color = middayLighting.color;
			timeOfDayIdx = 2;
		}
		else if (timeOfDayIdx != 3 && time > 12 && time <= 18) {
			RenderSettings.skybox = nightSkybox;
			lighting.intensity = nightLighting.intensity;
			lighting.color = nightLighting.color;
			timeOfDayIdx = 3;
		}

		if (gameStarted) {

			if (time > 6 && !Fire.inst.GetFire() || time > 6 && Fire.inst.GetFire() && !isNearFire) {
				// make decrease different amounts for time of day
				float dcr;
				if (time <=12) {
					dcr = 0.02f;
				}
				else {
					dcr = 0.1f;
				}
				if (bodyHeatImg.fillAmount > 0) {
					bodyHeatImg.fillAmount = bodyHeatImg.fillAmount - (dcr * Time.deltaTime);
				}
			}
			else if (time <= 6 || Fire.inst.GetFire() && isNearFire) {
				if (bodyHeatImg.fillAmount < 1) {
					bodyHeatImg.fillAmount = bodyHeatImg.fillAmount + (0.05f * Time.deltaTime);
				}
			}
            energyImg.fillAmount = energyImg.fillAmount - (0.01f * Time.deltaTime);

            if (energyImg.fillAmount <= 0.0f || bodyHeatImg.fillAmount <= 0.0f)
            {
                PlayerDeath();
            }

            AddTime(Time.deltaTime * 0.1f);
			Debug.Log(time);
			Debug.Log(timeOfDayIdx);
		}
	}

	public void DecrementTimeCount() {
		if (timeCount > 0)
			timeCount--;
	}

    // basically sleep
	public void PassTimeSleep() {
        AddTime(6.0f);
        bodyHeatImg.fillAmount = bodyHeatImg.fillAmount - 0.3f;
        energyImg.fillAmount = energyImg.fillAmount + 0.2f;
        Fire.inst.KillFire();
        DeleteTargets();
		TargetManager.inst.SpawnPrefabs();
    }

	public void GameStart() {
		gameStarted = true;
		bodyHeatImg.fillAmount = 1f;
        energyImg.fillAmount = 1f;
		time = 0.0f;
	}

    private void AddTime(float incr)
    {
        if (time <= 18)
        {
            time += incr;
            timeSurvived += incr;
        }
        else
        {
            time -= 18;
			time += incr;
        }
    }

    private void DeleteTargets()
    {
        GameObject[] targets;
        targets = GameObject.FindGameObjectsWithTag("target");
        foreach (GameObject target in targets)
        {
            Destroy(target);
        }

    }

    public void IncreaseEnergy()
    {
        energyImg.fillAmount = energyImg.fillAmount + 0.1f;
    }

    private void PlayerDeath()
    {
        gameStarted = false;
        // display "You Died"
		lighting.intensity = nightLighting.intensity;
		lighting.color = nightLighting.color;
        UITextTyper.inst.DisplayDeath();
        targetsKilled = 0;
        timeSurvived = 0.0f;
		
    }

	public void ResetPosition() {
		this.transform.position = startPlayerPos;
	}

    public void IncreaseTargetsKilled()
    {
        targetsKilled++;
    }
}
