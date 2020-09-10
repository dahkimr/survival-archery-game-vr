using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

    public Transform campfirePos;

    void Update() {
        if (this.transform.position.x < campfirePos.position.x - 5 || this.transform.position.x > campfirePos.position.x + 5 || this.transform.position.z < campfirePos.position.z - 5  || this.transform.position.z > campfirePos.position.z + 5 ) {
            Game.inst.isNearFire = false;
        }
        else {
            Game.inst.isNearFire = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fire")
        {
            Game.inst.isNearFire = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "fire")
        {
            Game.inst.isNearFire = true;
        }
    }

    private void OnTriggerLeave(Collider other)
    {
        if (other.tag == "fire")
        {
            Game.inst.isNearFire = false;
        }
    }
}
