using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVP;

public class AutoThrottleInput : MonoBehaviour {

    public VehicleController target;

    void OnEnable()
    {
        if (target == null)
            target = GetComponent<VehicleController>();
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        target.steerInput = 0;
        target.throttleInput = 1;
        target.brakeInput = 0;
        target.handbrakeInput = 0;
    }
}
