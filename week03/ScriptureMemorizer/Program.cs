/* 
EXCEEDING REQUIREMENTS:
- Added a ScriptureLibrary class to work with multiple scriptures
- Implemented file loading functionality to load scriptures from a text file
- Added a progress tracker to show how many words remain to be memorized
- Only hide words that aren't already hidden
- Added adaptive difficulty, hides approximately 10% of remaining visible words each time
- Added multiple constructors for Reference class, including one that parses from string
- Added ability for user to enter their own scripture or use the default library
- Added congratulatory message when scripture is fully memorized
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public bool IsHidden
    {
        get { return _isHidden; }
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public string GetDisplayText()
    {
        if (_isHidden)
        {
            return new string('_', _text.Length);
        }
        else
        {
            return _text;
        }
    }

    public void Show()
    {
        _isHidden = false;
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public Reference(string referenceText)
    {
        string[] parts = referenceText.Split(' ');

        int lastPartIndex = parts.Length - 1;
        string[] chapterVersePart = parts[lastPartIndex].Split(':');
        
        if (chapterVersePart.Length != 2)
        {
            throw new ArgumentException("Invalid reference format. Expected format: 'Book Chapter:Verse' or 'Book Chapter:StartVerse-EndVerse'");
        }
        
        _book = string.Join(" ", parts.Take(lastPartIndex));
        _chapter = int.Parse(chapterVersePart[0]);
        
        string versePart = chapterVersePart[1];
        if (versePart.Contains("-"))
        {
            string[] verses = versePart.Split('-');
            _startVerse = int.Parse(verses[0]);
            _endVerse = int.Parse(verses[1]);
        }
        else
        {
            _startVerse = int.Parse(versePart);
            _endVerse = _startVerse;
        }
    }

    public string GetDisplayText()
    {
        if (_startVerse == _endVerse)
        {
            return $"{_book} {_chapter}:{_startVerse}";
        }
        else
        {
            return $"{_book} {_chapter}:{_startVerse}-{_endVerse}";
        }
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();

        string[] wordArray = text.Split(' ');
        foreach (string wordText in wordArray)
        {
            _words.Add(new Word(wordText));
        }
    }

    public void HideRandomWords(int count)
    {
        Random random = new Random();
        List<Word> visibleWords = _words.Where(w => !w.IsHidden).ToList();

        int wordsToHide = Math.Min(count, visibleWords.Count);
        
        for (int i = 0; i < wordsToHide; i++)
        {
            if (visibleWords.Count == 0)
                break;
                
            int index = random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public string GetDisplayText()
    {
        string reference = _reference.GetDisplayText();

        string wordDisplay = string.Join(" ", _words.Select(w => w.GetDisplayText()));
        
        return $"{reference}\n\n{wordDisplay}";
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(w => w.IsHidden);
    }

    public int VisibleWordCount()
    {
        return _words.Count(w => !w.IsHidden);
    }
}

class ScriptureLibrary
{
    private List<Scripture> _scriptures;
    
    public ScriptureLibrary()
    {
        _scriptures = new List<Scripture>();
    }
    
    public void AddScripture(Scripture scripture)
    {
        _scriptures.Add(scripture);
    }
    
    public void LoadFromFile(string filename)
    {
        try
        {
            string[] lines = File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i += 2)
            {
                if (i + 1 < lines.Length)
                {
                    string referenceText = lines[i];
                    string scriptureText = lines[i + 1];
                    
                    Reference reference = new Reference(referenceText);
                    Scripture scripture = new Scripture(reference, scriptureText);
                    AddScripture(scripture);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading scriptures: {ex.Message}");
        }
    }
    
    public Scripture GetRandomScripture()
    {
        if (_scriptures.Count == 0)
            return null;
            
        Random random = new Random();
        return _scriptures[random.Next(_scriptures.Count)];
    }
    
    public int Count
    {
        get { return _scriptures.Count; }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Scripture Memorizer Program");
        Console.WriteLine("==========================");
        
        ScriptureLibrary library = new ScriptureLibrary();

        library.AddScripture(new Scripture(
            new Reference("John", 3, 16),
            "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."
        ));
        
        library.AddScripture(new Scripture(
            new Reference("Proverbs", 3, 5, 6),
            "Trust in the LORD with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight."
        ));

        string scriptureFile = "scriptures.txt";
        if (File.Exists(scriptureFile))
        {
            library.LoadFromFile(scriptureFile);
            Console.WriteLine($"Loaded scriptures from {scriptureFile}");
        }

        Scripture scripture = null;
        
        if (library.Count > 1)
        {
            Console.WriteLine("\nDo you want to:");
            Console.WriteLine("1. Use a random scripture from the library");
            Console.WriteLine("2. Enter your own scripture");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                scripture = library.GetRandomScripture();
            }
        }

        if (scripture == null)
        {
            Console.WriteLine("\nEnter a scripture reference (e.g., 'John 3:16' or 'Proverbs 3:5-6'):");
            Console.WriteLine("Or press Enter to use John 3:16");
            string referenceText = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(referenceText))
            {
                referenceText = "John 3:16";
            }
            
            Reference reference = new Reference(referenceText);
            
            Console.WriteLine("\nEnter the scripture text:");
            Console.WriteLine("Or press Enter to use default text for John 3:16");
            string scriptureText = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(scriptureText) && referenceText == "John 3:16")
            {
                scriptureText = "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life.";
            }
            
            scripture = new Scripture(reference, scriptureText);
        }

        bool quit = false;
        
        while (!quit && !scripture.IsCompletelyHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());

            int visibleWords = scripture.VisibleWordCount();
            Console.WriteLine($"\nProgress: {visibleWords} words remaining to memorize");

            Console.WriteLine("\nPress Enter to continue or type 'quit' to exit:");
            string input = Console.ReadLine();
            
            if (input.ToLower() == "quit")
            {
                quit = true;
            }
            else
            {
                int totalWordCount = visibleWords;
                int wordsToHide = Math.Max(1, totalWordCount / 10); 
                
                scripture.HideRandomWords(wordsToHide);
            }
        }

        if (!quit)
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nCongratulations! You've memorized the scripture!");
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
