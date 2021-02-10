using UnityEngine;
using UnityEngine.EventSystems;

// || This is attached to the Viewport of ScrollView
public class DroppableContent : MonoBehaviour, IDropHandler
{
    private float gapBetweenItems = 20;

    // || Cached References

    private RectTransform content;

    private void Awake()
    {
        if (transform.GetChild(0))
        {
            content = transform.GetChild(0).GetComponent<RectTransform>();
        }
    }

    private void Start()
    {
        float scale = BandController.Instance.Canvas.scaleFactor;
        gapBetweenItems *= (scale * 5);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (content && eventData.pointerDrag)
        {
            GameObject draggable = eventData.pointerDrag;
            RectTransform targetTransform = draggable.GetComponent<RectTransform>();
            ItemPrefab itemPrefab = draggable.GetComponent<ItemPrefab>();

            if (targetTransform && itemPrefab)
            {
                draggable.transform.SetParent(content);
                draggable.transform.SetAsLastSibling();

                if (!itemPrefab.InitialParent.Equals(content))
                {
                    if (itemPrefab.CurrentListName.Equals("BandItemPrefabs"))
                    {
                        BandController.Instance.BandItemPrefabs.RemoveAt(itemPrefab.CurrentIndex);
                        BandController.Instance.Bands.RemoveAt(itemPrefab.CurrentIndex);
                        BandController.Instance.FavoriteItemPrefabs.Add(itemPrefab);
                        BandController.Instance.Favorites.Add(itemPrefab.Band);
                    }
                    else if (itemPrefab.CurrentListName.Equals("FavoriteItemPrefabs"))
                    {
                        BandController.Instance.FavoriteItemPrefabs.RemoveAt(itemPrefab.CurrentIndex);
                        BandController.Instance.Favorites.RemoveAt(itemPrefab.CurrentIndex);
                        BandController.Instance.BandItemPrefabs.Add(itemPrefab);
                        BandController.Instance.Bands.Add(itemPrefab.Band);
                    }
                }
                else
                {
                    Vector2 dropPosition = eventData.position;
                    int differenceY = Mathf.FloorToInt((dropPosition.y - itemPrefab.InitialPosition.y));
                    int numberToJump = Mathf.Abs(differenceY / (int) gapBetweenItems);

                    if (numberToJump >= 1)
                    {
                        if (differenceY <= -gapBetweenItems)
                        {
                            itemPrefab.MoveBottom(numberToJump);
                        }
                        else if (differenceY >= -gapBetweenItems)
                        {
                            itemPrefab.MoveUp(numberToJump);
                        }

                        return;
                    }
                }

                BandController.Instance.ListItems();
            }
        }
    }
}