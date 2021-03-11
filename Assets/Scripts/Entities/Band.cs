
public class Band
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Year { get; private set; }

    public Band(int id, string name, int year)
    {
        Id = id;
        Name = name;
        Year = year;
    }
}
