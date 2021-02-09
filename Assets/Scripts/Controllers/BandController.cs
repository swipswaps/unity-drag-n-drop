using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BandController : MonoBehaviour
{
    // || Inspector References

    [SerializeField] private RectTransform scrollViewBandsContent;
    [SerializeField] private ItemPrefab itemPrefab;

    // || State

    [SerializeField] private List<Band> bands;
    [SerializeField] private List<ItemPrefab> itemPrefabs;

    // || Cached References

    public static BandController Instance { get; private set; }
    public List<ItemPrefab> ItemPrefabs { get => itemPrefabs; private set => itemPrefabs = value; }
    public List<Band> Bands { get => bands; private set => bands = value; }

    private void Awake()
    {
        Instance = this;

        bands = new List<Band>()
        {
            new Band(1, "Queen", 1970),
            new Band(2, "Led Zeppelin", 1968),
            new Band(3, "Black Sabbath", 1968),
            new Band(4, "The Who", 1964),
            new Band(5, "Metallica", 1981),
            new Band(6, "Megadeth", 1983),
            new Band(7, "Kansas", 1973),
            new Band(8, "Journey", 1973),
            new Band(9, "Styx", 1972),
            new Band(10, "Foo Fighters", 1994),
        };

        ItemPrefabs = new List<ItemPrefab>();
    }

    private void Start()
    {
        ListItems();
    }

    public void ListItems()
    {
        if (scrollViewBandsContent && itemPrefab)
        {
            foreach (Transform item in scrollViewBandsContent)
            {
                Destroy(item.gameObject);
            }

            ItemPrefabs.Clear();

            for (int index = 0; index < bands.Count; index++)
            {
                ItemPrefab item = Instantiate(itemPrefab, scrollViewBandsContent.position, Quaternion.identity);
                item.transform.SetParent(scrollViewBandsContent);
                item.transform.localScale = Vector3.one;
                item.Set(index, bands[index], bands.Count);
                ItemPrefabs.Add(item);
                
            }
        }
    }
}
