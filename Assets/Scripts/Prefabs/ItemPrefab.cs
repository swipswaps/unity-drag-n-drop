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
    public Vector2 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public CanvasGroup CanvasGroup { get => canvasGroup; private set => canvasGroup = value; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();

        if (moveBottomButton && moveUpButton && deleteButton)
        {
            moveBottomButton.onClick.AddListener(() => MoveBottom(1));
            moveUpButton.onClick.AddListener(() => MoveUp(1));
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

    public void MoveUp(int numberToDecrementIndex)
    {
        bool isFavorite = currentListName.Equals("FavoriteItemPrefabs");
        int listCount = (isFavorite ? BandController.Instance.Favorites.Count : BandController.Instance.Bands.Count);
        int previousIndex = (currentIndex - numberToDecrementIndex);

        if (previousIndex >= 0 && previousIndex <= listCount - 1)
        {
            if (!isFavorite)
            {
                BandController.Instance.BandItemPrefabs.RemoveAt(currentIndex);
                BandController.Instance.BandItemPrefabs.Insert(previousIndex, this);
                BandController.Instance.Bands.RemoveAt(currentIndex);
                BandController.Instance.Bands.Insert(previousIndex, Band);
            }
            else
            {
                BandController.Instance.FavoriteItemPrefabs.RemoveAt(currentIndex);
                BandController.Instance.FavoriteItemPrefabs.Insert(previousIndex, this);
                BandController.Instance.Favorites.RemoveAt(currentIndex);
                BandController.Instance.Favorites.Insert(previousIndex, Band);
            }

            currentIndex = previousIndex;
        }

        BandController.Instance.ListItems();
    }

    public void MoveBottom(int numberToIncrementIndex)
    {
        bool isFavorite = currentListName.Equals("FavoriteItemPrefabs");
        int listCount = (isFavorite ? BandController.Instance.Favorites.Count : BandController.Instance.Bands.Count);
        int nextIndex = (currentIndex + numberToIncrementIndex);

        if (nextIndex >= 0 && nextIndex <= listCount - 1)
        {
            if (!isFavorite)
            {
                BandController.Instance.BandItemPrefabs.RemoveAt(currentIndex);
                BandController.Instance.BandItemPrefabs.Insert(nextIndex, this);
                BandController.Instance.Bands.RemoveAt(currentIndex);
                BandController.Instance.Bands.Insert(nextIndex, Band);
            }
            else
            {
                BandController.Instance.FavoriteItemPrefabs.RemoveAt(currentIndex);
                BandController.Instance.FavoriteItemPrefabs.Insert(nextIndex, this);
                BandController.Instance.Favorites.RemoveAt(currentIndex);
                BandController.Instance.Favorites.Insert(nextIndex, Band);
            }

            currentIndex = nextIndex;
        }
        
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
            CanvasGroup.alpha = 0.5f;
            CanvasGroup.blocksRaycasts = false;
            InitialPosition = transform.position;
            InitialParent = transform.parent;
            transform.SetParent(BandController.Instance.MainPanel);
            transform.SetAsLastSibling();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;

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