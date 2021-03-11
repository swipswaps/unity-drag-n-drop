using System;
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
    [SerializeField] private List<Button> clearButtons;
    [SerializeField] private List<Button> shuffleButtons;
    [SerializeField] private ItemPrefab itemPrefab;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;

    // || Cached References

    private System.Random random = new System.Random();

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

        ResetBands(false);

        BandItemPrefabs = new List<ItemPrefab>();
        FavoriteItemPrefabs = new List<ItemPrefab>();

        if (quitButton && resetButton && rightButton && leftButton)
        {
            quitButton.onClick.AddListener(() => Application.Quit());
            resetButton.onClick.AddListener(() => ResetBands(true));
            rightButton.onClick.AddListener(() => TransferBands(Bands, Favorites));
            leftButton.onClick.AddListener(() => TransferBands(Favorites, Bands));

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
            panelsHeaders != null && panelsHeaders.Count == 2 &&
            clearButtons != null && clearButtons.Count == 2 &&
            shuffleButtons != null && shuffleButtons.Count == 2)
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

            int index = 0;
            foreach (PropertiesHolder holder in holders)
            {
                ListItems(holder);

                clearButtons[index].onClick.RemoveAllListeners();
                clearButtons[index].onClick.AddListener(() =>
                {
                    holder.Bands.Clear();
                    ListItems();
                });
                shuffleButtons[index].onClick.RemoveAllListeners();
                shuffleButtons[index].onClick.AddListener(() => ShuffleList(holder.Bands));

                clearButtons[index].interactable = shuffleButtons[index].interactable = (holder.Bands.Count > 0);

                index++;
            }

            UpdateButtons();
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

            UpdateTitle(holder);
        }
    }

    private void UpdateTitle(PropertiesHolder holder)
    {
        string currentTitle = holder.PanelTitle.text;
        int indexOf = currentTitle.IndexOf("-");
        currentTitle = (indexOf != -1 ? currentTitle.Substring(0, indexOf).Trim() : currentTitle);
        string counterText = (holder.ItemPrefabs.Count != 0 ? string.Format("{0} Item(s)", holder.ItemPrefabs.Count) : "Empty");
        currentTitle = string.Format("{0} - {1}", currentTitle, counterText);
        holder.PanelTitle.text = currentTitle;
    }

    private void UpdateButtons()
    {
        rightButton.interactable = (Bands.Count > 0);
        leftButton.interactable = (Favorites.Count > 0);
    }

    private void ResetBands(bool render)
    {
        Bands = new List<Band>()
        {
            new Band(1, "Led Zeppelin", 1968),
            new Band(2, "The Who", 1964),
            new Band(3, "Kansas", 1973),
            new Band(4, "Journey", 1973),
            new Band(5, "Styx", 1972),
            new Band(6, "Foo Fighters", 1994),
        };

        Favorites = new List<Band>()
        {
            new Band(7, "Queen", 1970),
            new Band(8, "Black Sabbath", 1968),
            new Band(9, "Metallica", 1981),
            new Band(10, "Megadeth", 1983),
        };

        if (render)
        {
            ListItems();
        }
    }

    private void TransferBands(List<Band> source, List<Band> target)
    {
        target.AddRange(source);
        source.Clear();
        ListItems();
    }

    private void ShuffleList(List<Band> bands)
    {
        List<Band> temp = new List<Band>();
        temp.AddRange(bands);
        bands.Clear();
        while (bands.Count != temp.Count)
        {
            int randomIndex = random.Next(0, temp.Count);
            Band band = temp[randomIndex];
            if (bands.FindIndex(b => b.Name.ToLower().Equals(band.Name.ToLower())) == -1)
            {
                bands.Add(band);
            }
        }

        ListItems();
    }
}