using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedProp : Holojam.Tools.Synchronizable {

    public int Index;

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
        data = new Holojam.Network.Flake(2, 0, 0, 1);
    }

    protected override void Sync() {
        if (Host) {
            data.ints[0] = 1;
            data.vector3s[0] = transform.position;
            data.vector3s[1] = transform.rotation.eulerAngles; 
        }
        else {
            if (data.ints[0] == 1) {
                transform.position = data.vector3s[0];
                transform.rotation = Quaternion.Euler(data.vector3s[1]);
            }
        }
    }
}
