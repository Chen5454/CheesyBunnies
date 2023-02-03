using System;
using System.Collections.Generic;
using UnityEngine;

public class DimensionLayersManager : MonoBehaviour
{
	[SerializeField] private List<DimensionLayer> _dimensionLayers = new List<DimensionLayer>();
	private DimensionLayer _previousDimension;

	public void ChangeDimension(DimensionsType type)
	{
		//vfx

		//changing dimensions
		GetDimension(type).SetDimensionActive(true);//get the wanted dimension and enable it

		_previousDimension.SetDimensionActive(false);//disable the first dimension layer
		_previousDimension = GetDimension(type);//update the previous dimension to the new dimension


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