using UnityEngine;

public class Objectesting : MonoBehaviour
{
    GameObject _root;
    RootMovement _rootMovement;
    public CameraFollow CameraFollow;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Root"))
        {
            _root = other.gameObject;
            _rootMovement = _root.GetComponent<RootMovement>();
            _rootMovement.enabled = false;
            CameraFollow.FollowLineReverse();
        }
    }



}