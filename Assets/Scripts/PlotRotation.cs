using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

        // Rotate graph
        Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (rightJoystick!=Vector2.zero)
        {
            this.transform.Rotate(new Vector3(0, -rightJoystick.x, 0));
        }

        // Move graph
        Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (leftJoystick!=Vector2.zero)
        {
            this.transform.position += new Vector3(leftJoystick.x, 0, leftJoystick.y) * Time.deltaTime;
        }

        // zoom in and out with grip
        float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);

        if(leftGrip!=0) {
            this.transform.localScale -= new Vector3(leftGrip, leftGrip, leftGrip) * Time.deltaTime;
        }

        float rightGrip = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);

        if(rightGrip!=0) {
            this.transform.localScale += new Vector3(rightGrip, rightGrip, rightGrip) * Time.deltaTime;
        }


        // Reset graph rotation
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            this.transform.rotation = Quaternion.identity;
        }
        // -1 0 left
        // 0  1 up

        //bool button3 = OVRInput.Get(OVRInput.Button.Three);
    }
}
