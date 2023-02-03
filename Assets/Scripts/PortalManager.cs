using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
   
        if (other.CompareTag("Root"))
        {
            Debug.Log("Root Detected");

            DimensionsType[] dimensions = (DimensionsType[])System.Enum.GetValues(typeof(DimensionsType));
            int randomIndex = UnityEngine.Random.Range(0, dimensions.Length);
            DimensionsType randomDimension = dimensions[randomIndex];
            DimensionLayersManager.Instance.ChangeDimension(randomDimension);
        }
    }
}
