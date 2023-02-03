using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public DimensionsType dimensionsType;
    public DimensionsType previousGameType;
    public DimensionLayer previousGamesLayer;

    private void Update()
    {
        previousGamesLayer = DimensionLayersManager.Instance.currentDimenstion;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Root"))
        {
            Debug.Log("Root Detected");


            DimensionLayersManager.Instance.ChangeDimension(dimensionsType);

            previousGameType = previousGamesLayer.GetDimensionType;

            DimensionsType temp = dimensionsType;
            dimensionsType = previousGameType;
            previousGameType = temp;
        }
    }
}
