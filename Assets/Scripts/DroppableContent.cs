using System.Collections.Generic;
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
                    List<ItemPrefab> currentPrefabList = (itemPrefab.Type == BandType.Current ? BandController.Instance.BandItemPrefabs : BandController.Instance.FavoriteItemPrefabs);
                    List<Band> currentBandList = (itemPrefab.Type == BandType.Current ? BandController.Instance.Bands : BandController.Instance.Favorites);
                    List<ItemPrefab> otherPrefabList = (itemPrefab.Type == BandType.Current ? BandController.Instance.FavoriteItemPrefabs : BandController.Instance.BandItemPrefabs);
                    List<Band> otherBandList = (itemPrefab.Type == BandType.Current ? BandController.Instance.Favorites : BandController.Instance.Bands);

                    currentPrefabList.RemoveAt(itemPrefab.CurrentIndex);
                    currentBandList.RemoveAt(itemPrefab.CurrentIndex);
                    otherPrefabList.Add(itemPrefab);
                    otherBandList.Add(itemPrefab.Band);
                    BandController.Instance.ListItems();
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
                    }
                }
            }
        }
    }
}