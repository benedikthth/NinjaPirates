using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFix : MonoBehaviour {

    public Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
    }
    private void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);
    }
   
}
