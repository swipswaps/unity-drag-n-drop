using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Band
{
    private int id;
    private string name;
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
