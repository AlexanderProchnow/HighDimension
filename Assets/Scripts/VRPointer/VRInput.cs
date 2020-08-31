using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRInput : BaseInput
{
    public Camera eventCamera = null;
    OVRInput.RawButton clickButton = OVRInput.RawButton.RIndexTrigger;
    //OVRInput.Controller controller = OVRInput.Controller.All;

    protected override void Awake() {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button) {
        return OVRInput.Get(clickButton);
    }

    public override bool GetMouseButtonDown(int button) {
        return OVRInput.Get(clickButton);
    }

    public override bool GetMouseButtonUp(int button) {
        return !OVRInput.Get(clickButton);
    }

    public override Vector2 mousePosition {
        get {
            return new Vector2(eventCamera.pixelWidth/2, eventCamera.pixelHeight/2);
        }
    }
}
