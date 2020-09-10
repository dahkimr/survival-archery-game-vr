using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

    public AudioSource pullAudSrc;

    public static Bow inst;
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

    public void PlayBowPull()
    {
        pullAudSrc.Play();
    }

    public void StopPlayBowPull()
    {
        pullAudSrc.Stop();
    }
}
