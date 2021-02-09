using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPrefab : MonoBehaviour
{
    // || Inspector References

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveBottomButton;
    [SerializeField] private Button deleteButton;

    // State

    [SerializeField] private int currentIndex;

    // Cached References

    [SerializeField] private Band band;

    // Properties

    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }
    public Band Band { get => band; set => band = value; }

    private void Awake()
    {
        if (moveBottomButton && moveUpButton && deleteButton)
        {
            moveBottomButton.onClick.AddListener(MoveBottom);
            moveUpButton.onClick.AddListener(MoveUp);
            deleteButton.onClick.AddListener(Delete);
        }
    }

    public void Set(int index, Band band, int bandsCount)
    {
        currentIndex = index;
        Band = band;

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
        BandController.Instance.ItemPrefabs.RemoveAt(currentIndex);
        BandController.Instance.ItemPrefabs.Insert(previousIndex, this);
        BandController.Instance.Bands.RemoveAt(currentIndex);
        BandController.Instance.Bands.Insert(previousIndex, Band);
        currentIndex = previousIndex;
        BandController.Instance.ListItems();
    }

    private void MoveBottom()
    {
        int nextIndex = (currentIndex + 1);
        BandController.Instance.ItemPrefabs.RemoveAt(currentIndex);
        BandController.Instance.ItemPrefabs.Insert(nextIndex, this);
        BandController.Instance.Bands.RemoveAt(currentIndex);
        BandController.Instance.Bands.Insert(nextIndex, Band);
        currentIndex = nextIndex;
        BandController.Instance.ListItems();
    }

    private void Delete()
    {
        BandController.Instance.ItemPrefabs.RemoveAt(currentIndex);
        BandController.Instance.Bands.RemoveAt(currentIndex);
        Destroy(gameObject);
        BandController.Instance.ListItems();
    }
}
