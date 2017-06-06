using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : Holojam.Tools.Synchronizable, IActivateReceiver {

    public ParticleSystem PollenParticles;
    public int Index;

    private bool _Active;

    public override bool AutoHost { get { return false; } }

    public override bool Host {
        get {
            return Index == Holojam.Tools.BuildManager.BUILD_INDEX;
        }
    }

    public override string Label {
        get {
            return this.name + "-syncedProp";
        }
    }

    public override void ResetData() {
        data = new Holojam.Network.Flake(0, 0, 0, 2);
    }

    protected override void Sync() {
        if (Host) {
            data.ints[0] = 1;
            if (_Active) {
                data.ints[1] = 1;
            }
            else {
                data.ints[1] = 0;
            }
            _Active = false;
        }
        if (data.ints[0] == 1 && data.ints[1] == 1) {
            PollenParticles.Play();
        }
        else {
            PollenParticles.Stop();
        }
    }

    public void Held() {
        _Active = true;
    }
}
