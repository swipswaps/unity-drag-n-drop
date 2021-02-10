using System;
using UnityEngine;

[Serializable]
public class Band
{
    private int id;
    [SerializeField] private string name;
    private int year;

    public int Id { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public int Year { get => year; set => year = value; }

    public Band(int id, string name, int year)
    {
        Id = id;
        Name = name;
        Year = year;
    }
}
