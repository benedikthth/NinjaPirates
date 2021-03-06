﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour {

    //camera position offset
    public Vector3 offset;
    public Vector3 velocity;
    public float smoothTime;

    private Camera cam;

    public float minSize;
    public float maxSize;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if(Game.Instance.Player.Count != 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, GetCenterPoint() + offset, ref velocity, smoothTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, GetOptimalCameraSize(), Time.deltaTime * 2);
        }
    }

    private Vector3 GetCenterPoint()
    {
        Bounds bounds = new Bounds(Game.Instance.Player[1].transform.position, Vector3.zero);
        for(int i = 0; i < 1; i++)
        {
            if (Game.Instance.Player[i].Alive)
            {
                bounds.Encapsulate(Game.Instance.Player[i].transform.position);
            }
        }

        return bounds.center;
    }

    private float GetOptimalCameraSize()
    {
        Bounds bounds = new Bounds(Game.Instance.ship.transform.position, Vector3.zero);
        for (int i = 0; i < Game.Instance.Player.Count; i++)
        {
            if (Game.Instance.Player[i].Alive)
            {
                bounds.Encapsulate(Game.Instance.Player[i].transform.position);
            }
        }

        return Mathf.Clamp(Mathf.Max(bounds.size.x, bounds.size.y * 1.78f ) * .75f , minSize, maxSize);
    }
}
