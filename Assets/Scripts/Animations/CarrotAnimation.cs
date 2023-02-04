using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotAnimation : MonoBehaviour
{
	public int CarrotBodyIndex;
	public int CarrotStateIndex;//0 = baby, 1 = young 2 = adult
	public Animator anim;

	[SerializeField] private float _visualUpdateDelay;


	public IEnumerator UpdateAnimator(int bodyIndex, int stateIndex)
	{
		yield return new WaitForSeconds(_visualUpdateDelay);
		SetCarrotVisual(bodyIndex, stateIndex);
	}

	public void SetCarrotVisual(int bodyIndex,int stateIndex)
	{
		anim.SetInteger("BodyIndex", bodyIndex);
		anim.SetInteger("StateIndex", stateIndex);
	}

}
