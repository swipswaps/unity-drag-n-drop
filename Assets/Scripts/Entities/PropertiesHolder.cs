using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertiesHolder
{
    public RectTransform PanelContent { get; set; }
    public List<ItemPrefab> ItemPrefabs { get; set; }
    public List<Band> Bands { get; set; }
    public BandType Type { get; set; }
    public TextMeshProUGUI PanelTitle { get; set; }
}