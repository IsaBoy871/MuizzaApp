using System;
using SQLite;

public class Affirmation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }
} 