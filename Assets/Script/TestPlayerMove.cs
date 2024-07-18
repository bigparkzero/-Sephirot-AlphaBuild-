using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    public float moveSpeed, sprintSpeed;
    public bool InverseY;
    public float TopClamp, BottomClamp;

    public GameObject cameraRoot;

    Vector2 moveinput;
    Vector2 lookinput;
    float targetRotation;
    float cameraX, cameraY;

    Camera mainCamera;
    CharacterController controller;

    void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        InputManagement();
        Move();
    }
    void Move()
    {
        float targetSpeed = (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed);
        if (moveinput == Vector2.zero) targetSpeed = 0.0f;
        Vector3 inputDirection = new Vector3(moveinput.x, 0.0f, moveinput.y).normalized;
        if (moveinput != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              mainCamera.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) +
                         new Vector3(0.0f, 0.0f, 0.0f) * Time.deltaTime);

    }
    void InputManagement()
    {
        lookinput = new Vector2(Input.GetAxis("Mouse X"), (InverseY ? - 1: 1 )*Input.GetAxis("Mouse Y"));
        moveinput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    void CameraRotation()
    {

        cameraRoot.transform.position = transform.position;

        if (lookinput.sqrMagnitude >= 0.01f)
        {
            cameraX += lookinput.x;
            cameraY += lookinput.y;
        }
        cameraX = ClampAngle(cameraX, float.MinValue, float.MaxValue);
        cameraY = ClampAngle(cameraY, BottomClamp, TopClamp);
        cameraRoot.transform.eulerAngles = new Vector3(cameraY,
            cameraX, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void OnValidate()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
