using System;

public class Entry
{
    public string Date { get; set; }
    public string Prompt { get; set; }
    public string Response { get; set; }
    
    public void Display()
    {
        Console.WriteLine($"Date: {Date}");
        Console.WriteLine($"Prompt: {Prompt}");
        Console.WriteLine($"Response: {Response}");
        Console.WriteLine();
    }
    
    public string ToFileString()
    {
        return $"{Date}~|~{Prompt}~|~{Response}";
    }
    
    public static Entry FromFileString(string fileString)
    {
        string[] parts = fileString.Split("~|~");
        
        if (parts.Length != 3)
        {
            throw new FormatException("Invalid entry format in file.");
        }
        
        return new Entry
        {
            Date = parts[0],
            Prompt = parts[1],
            Response = parts[2]
        };
    }
}