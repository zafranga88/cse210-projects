using System;
using System.Collections.Generic;
using System.IO;

public class Journal
{
    private List<Entry> _entries;
    
    public Journal()
    {
        _entries = new List<Entry>();
    }
    
    public void AddEntry(Entry entry)
    {
        _entries.Add(entry);
    }
    
    public void DisplayAll()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("Journal is empty. No entries to display.");
            return;
        }
        
        Console.WriteLine("===== Journal Entries =====");
        foreach (Entry entry in _entries)
        {
            entry.Display();
        }
    }
    
    public void SaveToFile(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (Entry entry in _entries)
                {
                    writer.WriteLine(entry.ToFileString());
                }
            }
            Console.WriteLine($"Journal saved successfully to {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving journal: {ex.Message}");
        }
    }
    
    public void LoadFromFile(string filename)
    {
        try
        {
            _entries.Clear();
            
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Entry entry = Entry.FromFileString(line);
                        _entries.Add(entry);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Warning: Skipped an invalid entry in the file.");
                    }
                }
            }
            
            Console.WriteLine($"Journal loaded successfully from {filename}");
            Console.WriteLine($"Loaded {_entries.Count} entries.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading journal: {ex.Message}");
        }
    }
}