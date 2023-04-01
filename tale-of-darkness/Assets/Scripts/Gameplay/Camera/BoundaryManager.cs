using UnityEngine;
using Cinemachine;

public class BoundaryManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam1;
    [SerializeField] private CinemachineVirtualCamera vcam2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                vcam1.Priority = 0;
                vcam2.Priority = 1;
        }
    }
}
