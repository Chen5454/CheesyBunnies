using System;
using System.Collections.Generic;
using UnityEngine;

public class DimensionLayersManager : MonoBehaviour
{
	private static DimensionLayersManager _instance;
	public static DimensionLayersManager Instance => _instance;

	[SerializeField] private List<DimensionLayer> _dimensionLayers = new List<DimensionLayer>();
	private DimensionLayer _previousDimension;

	[SerializeField] private int _backgroundLayerOrder = -100;
	[SerializeField] private int _upperLayerOrder = -99;
	//public PortalManager CurrentPortal;
	public DimensionLayer currentDimenstion;

    private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);

		_previousDimension = _dimensionLayers[0];
	}	

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
           ChangeDimension(DimensionsType.DIMENSION_2);
		}
    }

	public void ChangeDimension(DimensionsType type)
	{
		if (GetDimension(type).GetDimensionType == _previousDimension.GetDimensionType)//attempting to change to the same dimension
		{
			Debug.LogError("Dimension Layer Manager: Attempting to change to the same dimension, canceling function");
			return;
		}
		//vfx

		//changing dimensions
		GetDimension(type).SetLayerOrder(_upperLayerOrder);
		GetDimension(type).SetDimensionActive(true);//get the wanted dimension and enable it

		_previousDimension.SetDimensionActive(false);//disable the first dimension layer
		_previousDimension.SetLayerOrder(_backgroundLayerOrder);
		_previousDimension = GetDimension(type);//update the previous dimension to the new dimension
		currentDimenstion = _previousDimension;

    }
	DimensionLayer GetDimension(DimensionsType type)
	{

		return _dimensionLayers.Find(x => x.GetDimensionType == type);
	}

}
[Serializable]
public class DimensionLayer
{
	[SerializeField] private GameObject _layerGO;
	[SerializeField] private DimensionsType _dimensionType;
	public DimensionsType GetDimensionType => _dimensionType;

	public void SetDimensionActive(bool isActive)
	{
		_layerGO.SetActive(isActive);
	}
	public void SetLayerOrder(int order)
	{
		_layerGO.GetComponent<SpriteRenderer>().sortingOrder = order;
	}
}

public enum DimensionsType
{
	DIMENSION_1,
	DIMENSION_2,
	DIMENSION_3,
	DIMENSION_4,
	DIMENSION_5,
	DIMENSION_6,
	DIMENSION_7,
}