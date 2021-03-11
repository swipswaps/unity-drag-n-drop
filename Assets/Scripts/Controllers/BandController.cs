using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BandController : MonoBehaviour
{
    // || Inspector References

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform mainPanel;
    [SerializeField] private List<RectTransform> panelsContent;
    [SerializeField] private List<TextMeshProUGUI> panelsHeaders;
    [SerializeField] private ItemPrefab itemPrefab;
    [SerializeField] private Button quitButton;


    // || Properties

    public static BandController Instance { get; private set; }
    public List<ItemPrefab> BandItemPrefabs { get; private set; }
    public List<ItemPrefab> FavoriteItemPrefabs { get; private set; }
    public List<Band> Bands { get; private set; }
    public List<Band> Favorites { get; private set; }
    public RectTransform MainPanel => mainPanel;

    public Canvas Canvas { get => canvas; private set => canvas = value; }

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

        if (quitButton)
        {
            quitButton.onClick.AddListener(() => Application.Quit());

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                quitButton.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        ListItems();
    }

    public void ListItems()
    {
        if (panelsContent != null && panelsContent.Count == 2 &&
            panelsHeaders != null && panelsHeaders.Count == 2)
        {
            PropertiesHolder[] holders =
            {
                new PropertiesHolder()
                {
                    Bands = Bands, ItemPrefabs = BandItemPrefabs, PanelContent = panelsContent[0],
                    PanelTitle = panelsHeaders[0], Type = BandType.Current
                },
                new PropertiesHolder()
                {
                    Bands = Favorites, ItemPrefabs = FavoriteItemPrefabs, PanelContent = panelsContent[1],
                    PanelTitle = panelsHeaders[1], Type = BandType.Favorite
                },
            };

            foreach(PropertiesHolder holder in holders)
            {
                ListItems(holder);
            }
        }
    }

    private void ListItems(PropertiesHolder holder)
    {
        if (itemPrefab)
        {
            foreach (Transform item in holder.PanelContent)
            {
                Destroy(item.gameObject);
            }

            holder.ItemPrefabs.Clear();

            for (int index = 0; index < holder.Bands.Count; index++)
            {
                ItemPrefab item = Instantiate(itemPrefab, holder.PanelContent.position, Quaternion.identity);
                item.transform.SetParent(holder.PanelContent);
                item.transform.localScale = Vector3.one;
                item.Set(index, holder.Bands[index], holder.Bands.Count, holder.Type);
                holder.ItemPrefabs.Add(item);
            }

            UpdateTitleAndButtons(holder);
        }
    }

    private void UpdateTitleAndButtons(PropertiesHolder holder)
    {
        string currentTitle = holder.PanelTitle.text;
        int indexOf = currentTitle.IndexOf("-");
        currentTitle = (indexOf != -1 ? currentTitle.Substring(0, indexOf).Trim() : currentTitle);
        string counterText = (holder.ItemPrefabs.Count != 0 ? string.Format("{0} Item(s)", holder.ItemPrefabs.Count) : "Empty");
        currentTitle = string.Format("{0} - {1}", currentTitle, counterText);
        holder.PanelTitle.text = currentTitle;
    }
}