using UnityEngine;
using UnityEngine.EventSystems;

// || This is attached to the Viewport of ScrollView
public class DroppableContent : MonoBehaviour, IDropHandler
{
    // || Cached References

    private RectTransform content;

    private void Awake()
    {
        if (transform.GetChild(0))
        {
            content = transform.GetChild(0).GetComponent<RectTransform>();
        }
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

                BandController.Instance.ListItems();
            }
        }
    }
}