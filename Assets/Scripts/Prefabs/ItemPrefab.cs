using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // || Inspector References

    [Header("Children UI Elements")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button deleteButton;

    // Cached References

    private RectTransform rectTransform;
    [SerializeField] private List<Band> currentBandList;
    [SerializeField] private List<ItemPrefab> currentPrefabList;

    // Properties

    public int CurrentIndex { get; private set; }
    public Band Band { get; private set; }
    public Transform InitialParent { get; set; }
    public Vector2 InitialPosition { get; set; }
    public CanvasGroup CanvasGroup { get; private set; }
    public BandType Type { get; set; }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();

        if (deleteButton)
        {
            deleteButton.onClick.AddListener(() => Delete());
        }
    }

    public void Set(int index, Band band, int bandsCount, BandType type)
    {
        CurrentIndex = index;
        Band = band;
        Type = type;

        if (descriptionText)
        {
            string description = "{0} - {1} - {2}";
            description = string.Format(description, (index + 1).ToString("00"), band.Name, band.Year);
            descriptionText.text = description;
        }
    }

    private void GetListReferences()
    {
        currentBandList = (Type == BandType.Current ? BandController.Instance.Bands : BandController.Instance.Favorites);
        currentPrefabList = (Type == BandType.Current ? BandController.Instance.BandItemPrefabs : BandController.Instance.FavoriteItemPrefabs);
    }

    public void MoveUp(int numberToDecrementIndex)
    {
        GetListReferences();

        int listCount = currentBandList.Count;
        int previousIndex = (CurrentIndex - numberToDecrementIndex);
        if (previousIndex >= 0 && previousIndex <= listCount - 1)
        {
            currentPrefabList.RemoveAt(CurrentIndex);
            currentPrefabList.Insert(previousIndex, this);
            currentBandList.RemoveAt(CurrentIndex);
            currentBandList.Insert(previousIndex, Band);
            CurrentIndex = previousIndex;
        }

        BandController.Instance.ListItems();
    }

    public void MoveBottom(int numberToIncrementIndex)
    {
        GetListReferences();
        
        int listCount = currentBandList.Count;
        int nextIndex = (CurrentIndex + numberToIncrementIndex);
        if (nextIndex >= 0 && nextIndex <= listCount - 1)
        {
            currentPrefabList.RemoveAt(CurrentIndex);
            currentPrefabList.Insert(nextIndex, this);
            currentBandList.RemoveAt(CurrentIndex);
            currentBandList.Insert(nextIndex, Band);
            CurrentIndex = nextIndex;
        }

        BandController.Instance.ListItems();
    }

    private void Delete()
    {
        GetListReferences();
        currentPrefabList.RemoveAt(CurrentIndex);
        currentBandList.RemoveAt(CurrentIndex);
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

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += (eventData.delta / BandController.Instance.Canvas.scaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup.alpha = 1f;
        CanvasGroup.blocksRaycasts = true;

        if (eventData.hovered.Count >= 1)
        {
            transform.SetParent(InitialParent);
            transform.SetSiblingIndex(CurrentIndex);
        }
    }
}