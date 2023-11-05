using System;
using System.Collections.Generic;
using UnityEngine;

public class FinishAction : MonoBehaviour
{
    public static Action<FinishType> Finish;
    public static bool IsGameOver = false;
    [SerializeField] private List<GameObject> _finishableObjects;
    [SerializeField] private ParticleSystem[] _confetti;
   // [SerializeField] private CinemachineVirtualCamera _skinChangerCamera;

    private void Start()
    {
        IsGameOver = false;
        Finish += Activate;
    }
    
    private void OnDestroy()
    {
        Finish -= Activate;
    }

    public void Activate(FinishType finishType = FinishType.None)
    {
        IsGameOver = true;
        if (_finishableObjects.Count > 0)
        {
            switch (finishType)
            {
                case FinishType.Win:
                    
                    foreach (var obj in _finishableObjects)
                    {
                        if (obj.TryGetComponent(out IFinishable finishable))
                            finishable.StartActionOnWin();
                    }

                    for (int i = 0; i < _confetti.Length; i++)
                    {
                        _confetti[i].Play();
                    }
                    break;

                case FinishType.Lose:
                    foreach (var obj in _finishableObjects)
                    {
                        if (obj.TryGetComponent(out IFinishable finishable))
                            finishable.StartActionOnLose();
                    }
                    break;
                case FinishType.Draw:
                    foreach (var obj in _finishableObjects)
                    {
                        if (obj.TryGetComponent(out IFinishable finishable))
                            finishable.StartActionOnDrawn();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    

    public enum FinishType
    {
        None,
        Win,
        Lose,
        Draw
    }
}