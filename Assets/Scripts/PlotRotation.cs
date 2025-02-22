﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotRotation : MonoBehaviour
{
    public GameObject menu;

    public float menuDistance = 2.5f;

    OVRInput.RawAxis1D menuButton = OVRInput.RawAxis1D.LIndexTrigger;

    // Variable to prevent button spamming
    private bool cooldown = false;

    Camera mainCamera;

    // Get data object to keep data points at constant size
    private Transform data;

    // Get axes to scale when changing point size
    private Transform[] axes;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        data = this.transform.Find("Data");
        
        // Get all axes individually
        Transform axesBase = this.transform.Find("Axes");
        axes = new Transform[3];
        axes[0] = axesBase.Find("x1Pivot");
        axes[1] = axesBase.Find("x2Pivot");
        axes[2] = axesBase.Find("x3Pivot");
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

        // Zoom in and out with grip:
        
        // Zoom out
        float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);

        if(leftGrip!=0) {
            Vector3 scaling = new Vector3(leftGrip, leftGrip, leftGrip) * Time.deltaTime * this.transform.localScale.x;
            this.transform.localScale -= scaling;
        }

        // Zoom in
        float rightGrip = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);

        if(rightGrip!=0) {
            Vector3 scaling = new Vector3(rightGrip, rightGrip, rightGrip) * Time.deltaTime * this.transform.localScale.x;
            this.transform.localScale += scaling;
        }

        // Increase and decrease point size: 

        // Decrease point size with left controller X
        if(OVRInput.Get(OVRInput.Button.Three)) {
            // Get all points
            Transform[] tarray = data.GetComponentsInChildren<Transform>(false);
            // Decrease point size
            foreach(Transform point in tarray) {
                point.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            }

            // Decrease axes size
            axes[0].localScale -= new Vector3(0, 0.1f, 0.1f) * Time.deltaTime;
            axes[1].localScale -= new Vector3(0.1f, 0, 0.1f) * Time.deltaTime;
            axes[2].localScale -= new Vector3(0.1f, 0.1f, 0) * Time.deltaTime;
        }

        // Increase point size with left controller Y
        if(OVRInput.Get(OVRInput.Button.Four)) {
            // Get all points
            Transform[] tarray = data.GetComponentsInChildren<Transform>(false);
            // Increase point size
            foreach(Transform point in tarray) {
                point.localScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            }

            // Increase axes size
            axes[0].localScale += new Vector3(0, 1f, 1f) * Time.deltaTime;
            axes[1].localScale += new Vector3(1f, 0, 1f) * Time.deltaTime;
            axes[2].localScale += new Vector3(1f, 1f, 0) * Time.deltaTime;
        }

        // Reset graph rotation
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            this.transform.rotation = Quaternion.identity;
        }

        // Reset graph position
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick)) {
            this.transform.position = new Vector3(-1.2f, 1f, 0);
        }

        // Open and close menu, menu appears in view of user
        if (OVRInput.Get(menuButton)>=0.5 && cooldown == false) {
            if (menu.activeSelf) {
                menu.SetActive(false);  
            } else {
                // Get forward vector of main camera (the direction of the users view)
                Vector3 viewDirection = mainCamera.transform.forward * menuDistance;
                // Move menu in users view, freeze y axis to display menu at the same height wrt the users eyelevel
                menu.transform.position = mainCamera.transform.position + new Vector3(viewDirection.x, -0.5f, viewDirection.z);
                Quaternion cr = mainCamera.transform.rotation;
                // Freezes x and z rotation (i.e. head tilt)
                menu.transform.rotation = new Quaternion(0, cr.y, 0, cr.w); // Quaternion.LookRotation(viewDirection/menuDistance, Vector3.up);
                menu.SetActive(true);
            }
            cooldown = true;
       }

       
       // block menu button if still pressed, unlock button when trigger released
       if (OVRInput.Get(menuButton)<0.5) {
           cooldown = false;
       }
    }

    // Prevent button spamming
    void ResetCooldown(){
     cooldown = false;
    }
}
