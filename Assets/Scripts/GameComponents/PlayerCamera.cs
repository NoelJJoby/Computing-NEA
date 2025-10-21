using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public enum CameraMode
    {
        FirstPerson,
        ThirdPerson 
    }

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform thirdPersonRotateCenter;


    [SerializeField] CameraMode cameraMode = CameraMode.FirstPerson;
    [SerializeField] private float mouseSensitivity = 100f;


    private Vector3 initialTPCameraPos = new(0, 2, -2);
    private Vector3 initialFPCameraPos = new(0, 0.5f, 0);


    private float xRotation;
    private float yRotation;
    private bool paused = false;

    public void Start()
    {
        cameraMode = CameraMode.FirstPerson;
        GameInputProcessor.Instance.OnCameraChangePOV += PlayerCamera_OnCameraChangePOV;
        GameInputProcessor.Instance.OnGamePause += GameInputProcessor_OnGamePause;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

    }

    private void GameManager_OnGameOver(object sender, EventArgs e)
    {

        paused = true;
    }
    private void GameInputProcessor_OnGamePause(object sender, EventArgs e)
    {
        paused = !paused;
    }

    private void PlayerCamera_OnCameraChangePOV(object sender, EventArgs e)
    {
        xRotation = 0f;
        switch (cameraMode)
        {
            case CameraMode.FirstPerson:
                cameraMode = CameraMode.ThirdPerson;
                transform.localPosition = initialTPCameraPos;
                transform.localRotation.SetFromToRotation(initialTPCameraPos, playerBody.localPosition);

                break;
            case CameraMode.ThirdPerson:
                cameraMode = CameraMode.FirstPerson;
                transform.localPosition = initialFPCameraPos;
                transform.localRotation = Quaternion.identity;
                break;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        if (paused)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

            switch (cameraMode)
            {
                case CameraMode.FirstPerson:
                    MoveCameraFirstPerson();
                    break;
                case CameraMode.ThirdPerson:
                    MoveCameraThirdPerson();
                    break;

            }

    }


    private void MoveCameraFirstPerson()
    {
        Vector2 mouseInputVector = Time.deltaTime * mouseSensitivity * GameInputProcessor.Instance.GetInputCameraVector();

        xRotation = Mathf.Clamp(xRotation - mouseInputVector.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseInputVector.x);
    }


    // Unfinished
    private void MoveCameraThirdPerson()
    {
        Vector2 mouseInputVector = Time.deltaTime * mouseSensitivity * GameInputProcessor.Instance.GetInputCameraVector();
        playerBody.Rotate(Vector3.up * mouseInputVector.x);
        xRotation = Mathf.Clamp(xRotation - mouseInputVector.y, -90f, 90f);
        yRotation = Mathf.Clamp(mouseInputVector.x, -180f, 180f);

        transform.RotateAround(playerBody.localPosition, Vector3.up, yRotation);
        transform.LookAt(playerBody);

        float y = transform.localRotation.eulerAngles.y;
        float z = transform.localRotation.eulerAngles.z;

        transform.localRotation = Quaternion.Euler(xRotation, y, z);
        playerBody.Rotate(Vector3.up * yRotation);

    }
    private void OnDestroy()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
}
