using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotRotation : MonoBehaviour
{
    public GameObject menu;

    public float menuDistance = 2.5f;

    OVRInput.Axis1D menuButton = OVRInput.Axis1D.PrimaryIndexTrigger;

    // Variable to prevent button spamming
    private bool cooldown = false;

     Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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

        // Open and close menu
        if (OVRInput.Get(menuButton)>=0.5 && cooldown == false) {
            if (menu.activeSelf) {
                menu.SetActive(false);  
            } else {
                // Move menu in users view
                menu.transform.position = mainCamera.transform.position + new Vector3(0,0, menuDistance);
                Quaternion cr = mainCamera.transform.rotation;
                // Freezes z rotation (e.g. head tilt)
                menu.transform.rotation = new Quaternion(cr.x, cr.y, 0, cr.w);
                menu.SetActive(true);
            }
            cooldown = true;
       }

       if (OVRInput.Get(menuButton)<0.5) {
           cooldown = false;
       }
    }

    // Prevent button spamming
    void ResetCooldown(){
     cooldown = false;
    }
}
