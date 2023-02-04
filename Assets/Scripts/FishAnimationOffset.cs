using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FishAnimationOffset : MonoBehaviour
{
    private void Start()
    {
        var animator = GetComponent<Animator>();
        animator.SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }
}
