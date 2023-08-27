using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera _captureCastleVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera _blueCastleDeathVirtualCamera;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    public bool IsCameraBlendCompleted => _cinemachineBrain.IsBlending && (_cinemachineBrain.ActiveBlend.TimeInBlend + 0.05f >= _cinemachineBrain.ActiveBlend.Duration || !_cinemachineBrain.ActiveBlend.IsValid);

    public void SetPlayerVirtualCamera()
    {
        _playerVirtualCamera.Priority = 1;
        _captureCastleVirtualCamera.Priority = 0;
        _blueCastleDeathVirtualCamera.Priority = 0;
    }

    public IEnumerator SetCaptureCastleVirtualCamera()
    {
        _playerVirtualCamera.Priority = 0;
        _captureCastleVirtualCamera.Priority = 1;
        _blueCastleDeathVirtualCamera.Priority = 0;

        yield return new WaitForSeconds(3f);

        SetPlayerVirtualCamera();
    }

    public IEnumerator SetBlueCastleDeathVirtualCamera()
    {
        yield return new WaitForSeconds(2f);

        _playerVirtualCamera.Priority = 0;
        _captureCastleVirtualCamera.Priority = 0;
        _blueCastleDeathVirtualCamera.Priority = 1;
    }
}