using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public DimensionsType gamestates;
    private DimensionsType previousGamestate;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Root"))
        {
            Debug.Log("Root Detected");

            if (previousGamestate == 0)
            {
                // first time entering, store the current gamestate
                previousGamestate = gamestates;
            }
            else
            {
                // change back to previous gamestate
                gamestates = previousGamestate;
                previousGamestate = 0;
            }

            DimensionLayersManager.Instance.ChangeDimension(gamestates);
        }
    }
}
