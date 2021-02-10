using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // || Inspector References

    [Header("Children UI Elements")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveBottomButton;
    [SerializeField] private Button deleteButton;

    // State

    [SerializeField] private int currentIndex;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private Transform initialParent;
    [SerializeField] private string currentListName;

    // Cached References

    private Band band;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // Properties

    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public Band Band { get => band; set => band = value; }
    public string CurrentListName { get => currentListName; set => currentListName = value; }
    public Transform InitialParent { get => initialParent; set => initialParent = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (moveBottomButton && moveUpButton && deleteButton)
        {
            moveBottomButton.onClick.AddListener(MoveBottom);
            moveUpButton.onClick.AddListener(MoveUp);
            deleteButton.onClick.AddListener(Delete);
        }
    }

    public void Set(int index, Band band, int bandsCount, string currentListName)
    {
        currentIndex = index;
        Band = band;
        CurrentListName = currentListName;

        moveUpButton.gameObject.SetActive(index > 0);
        moveBottomButton.gameObject.SetActive(index < (bandsCount - 1));

        if (descriptionText)
        {
            string description = "{0} - {1}";
            description = string.Format(description, (index + 1).ToString("00"), band.Name);
            descriptionText.text = description;
        }
    }

    private void MoveUp()
    {
        int previousIndex = (currentIndex - 1);

        if (currentListName.Equals("BandItemPrefabs"))
        {
            BandController.Instance.BandItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.BandItemPrefabs.Insert(previousIndex, this);
            BandController.Instance.Bands.RemoveAt(currentIndex);
            BandController.Instance.Bands.Insert(previousIndex, Band);
        }
        else if (currentListName.Equals("FavoriteItemPrefabs"))
        {
            BandController.Instance.FavoriteItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.FavoriteItemPrefabs.Insert(previousIndex, this);
            BandController.Instance.Favorites.RemoveAt(currentIndex);
            BandController.Instance.Favorites.Insert(previousIndex, Band);
        }

        currentIndex = previousIndex;
        BandController.Instance.ListItems();
    }

    private void MoveBottom()
    {
        int nextIndex = (currentIndex + 1);

        if (currentListName.Equals("BandItemPrefabs"))
        {
            BandController.Instance.BandItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.BandItemPrefabs.Insert(nextIndex, this);
            BandController.Instance.Bands.RemoveAt(currentIndex);
            BandController.Instance.Bands.Insert(nextIndex, Band);
        }
        else if (currentListName.Equals("FavoriteItemPrefabs"))
        {
            BandController.Instance.FavoriteItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.FavoriteItemPrefabs.Insert(nextIndex, this);
            BandController.Instance.Favorites.RemoveAt(currentIndex);
            BandController.Instance.Favorites.Insert(nextIndex, Band);
        }
        
        currentIndex = nextIndex;
        BandController.Instance.ListItems();
    }

    private void Delete()
    {
        if (currentListName.Equals("BandItemPrefabs"))
        {
            BandController.Instance.BandItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.Bands.RemoveAt(currentIndex);
        }
        else if (currentListName.Equals("FavoriteItemPrefabs"))
        {
            BandController.Instance.FavoriteItemPrefabs.RemoveAt(currentIndex);
            BandController.Instance.Favorites.RemoveAt(currentIndex);
        }
        
        Destroy(gameObject);
        BandController.Instance.ListItems();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BandController.Instance.MainPanel)
        {
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
            InitialParent = transform.parent;
            transform.SetParent(BandController.Instance.MainPanel);
            transform.SetAsLastSibling();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.hovered.Count >= 1)
        {
            transform.SetParent(InitialParent);
            transform.SetSiblingIndex(currentIndex);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += (eventData.delta / BandController.Instance.Canvas.scaleFactor);
    }
}
