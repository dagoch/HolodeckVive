using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour, IActivateReceiver {

    public ParticleSystem PollenParticles;

    private bool _Active;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate() {
        if (_Active) {
            return;
        }

        _Active = true;
        PollenParticles.Play();
    }

    public void Deactivate() {
        if (!_Active) {
            return;
        }

        _Active = false;
        PollenParticles.Stop();
    }
}
