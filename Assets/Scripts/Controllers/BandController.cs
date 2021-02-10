using System.Collections.Generic;
using UnityEngine;

public class BandController : MonoBehaviour
{
    // || Inspector References

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform mainPanel;
    [SerializeField] private RectTransform scrollViewBandsPanelContent;
    [SerializeField] private RectTransform scrollViewFavoritePanelContent;
    [SerializeField] private ItemPrefab itemPrefab;

    // || State

    [SerializeField] private List<Band> bands;
    [SerializeField] private List<Band> favorites;
    [SerializeField] private List<ItemPrefab> bandItemPrefabs;
    [SerializeField] private List<ItemPrefab> favoritesItemPrefabs;

    // || Properties

    public static BandController Instance { get; private set; }
    public List<ItemPrefab> BandItemPrefabs { get => bandItemPrefabs; private set => bandItemPrefabs = value; }
    public List<ItemPrefab> FavoriteItemPrefabs { get => favoritesItemPrefabs; private set => favoritesItemPrefabs = value; }
    public List<Band> Bands { get => bands; private set => bands = value; }
    public List<Band> Favorites { get => favorites; private set => favorites = value; }
    public Canvas Canvas => canvas;
    public RectTransform MainPanel => mainPanel;
    public RectTransform ScrollViewBandsPanelContent => scrollViewBandsPanelContent;
    public RectTransform ScrollViewFavoritePanelContent => scrollViewFavoritePanelContent;

    private void Awake()
    {
        Instance = this;

        Bands = new List<Band>()
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

        Favorites = new List<Band>();

        BandItemPrefabs = new List<ItemPrefab>();
        FavoriteItemPrefabs = new List<ItemPrefab>();
    }

    private void Start()
    {
        ListItems();
    }

    public void ListItems()
    {
        ListItems(scrollViewBandsPanelContent, BandItemPrefabs, Bands, "BandItemPrefabs");
        ListItems(scrollViewFavoritePanelContent, FavoriteItemPrefabs, Favorites, "FavoriteItemPrefabs");
    }

    private void ListItems(RectTransform content, List<ItemPrefab> listItemPrefabs, List<Band> listBands, string currentListName)
    {
        if (itemPrefab)
        {
            foreach (Transform item in content)
            {
                Destroy(item.gameObject);
            }

            listItemPrefabs.Clear();

            for (int index = 0; index < listBands.Count; index++)
            {
                ItemPrefab item = Instantiate(itemPrefab, content.position, Quaternion.identity);
                item.transform.SetParent(content);
                item.transform.localScale = Vector3.one;
                item.Set(index, listBands[index], listBands.Count, currentListName);
                listItemPrefabs.Add(item);
            }
        }
    }
}