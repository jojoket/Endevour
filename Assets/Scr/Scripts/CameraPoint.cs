using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPoint : MonoBehaviour
{

    public CinemachineVirtualCamera camera;

    private Vector3 mouseDelta;
    private Vector3 lastMousePos;

    public float maxZoom = 2.4f;
    public float minZoom = 5f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            this.lastMousePos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse1)) {
            if (this.lastMousePos != Input.mousePosition) {
                this.mouseDelta = Input.mousePosition - this.lastMousePos;
            } else {
                this.mouseDelta = Vector3.zero;
            }
            this.transform.position -= new Vector3(this.mouseDelta.x / Screen.width * 9, this.mouseDelta.y / Screen.height * 5, 0) * this.camera.m_Lens.OrthographicSize / 2.53f;

            this.lastMousePos = Input.mousePosition;
        }

        this.camera.m_Lens.OrthographicSize -= Input.mouseScrollDelta.y / 5;
        this.camera.m_Lens.OrthographicSize = Mathf.Clamp(this.camera.m_Lens.OrthographicSize, this.maxZoom, this.minZoom);
    }
}
