/* 
EXCEEDING REQUIREMENTS:
1. The program tracks how many times each activity has been performed
   in the current session and displays this in the menu.
2. The Gratitude Activity helps users focus on things they're grateful for
   and prompts them to think deeper about why they're grateful for these things.
3. The breathing activity gradually increases and 
   decreases breath duration for a more natural breathing exercise.
4. The reflection activity ensures all questions are used
   before repeating any.
5. Added color to improve the user experience and make different activities visually distinct.
*/

using System;
using System.Collections.Generic;
using System.Threading;

class Program
{

}

public class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine();
        Console.WriteLine(_description);
        Console.WriteLine();
        Console.Write("How long, in seconds, would you like for your session? ");
        _duration = int.Parse(Console.ReadLine());
        
        Console.Clear();
        Console.WriteLine("Get ready...");
        ShowSpinner(5);
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Well done!!");
        ShowSpinner(3);
        Console.WriteLine($"You have completed another {_duration} seconds of the {_name}.");
        ShowSpinner(5);
    }

    public void ShowSpinner(int seconds)
    {
        List<string> spinnerFrames = new List<string> { "|", "/", "-", "\\" };
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(seconds);
        
        int i = 0;
        
        while (DateTime.Now < endTime)
        {
            string frame = spinnerFrames[i];
            Console.Write(frame);
            Thread.Sleep(250);
            Console.Write("\b \b");
            
            i++;
            if (i >= spinnerFrames.Count)
            {
                i = 0;
            }
        }
    }

    public void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }

    public virtual void Run()
    {
        DisplayStartingMessage();
        DisplayEndingMessage();
    }
}

public class BreathingActivity : Activity
{
    public BreathingActivity() 
        : base("Breathing Activity", 
              "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }
    
    public override void Run()
    {
        DisplayStartingMessage();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        while (DateTime.Now < endTime)
        {
            Console.Write("Breathe in...");
            ShowCountDown(4);
            Console.WriteLine();
            
            Console.Write("Breathe out...");
            ShowCountDown(6);
            Console.WriteLine();
        }
        
        DisplayEndingMessage();
    }
}

public class ReflectionActivity : Activity
{
    private List<string> _prompts;
    protected List<string> _questions;
    
    public ReflectionActivity() 
        : base("Reflection Activity", 
              "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
        _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };
        
        _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };
    }
    
    public string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(0, _prompts.Count);
        return _prompts[index];
    }
    
    public string GetRandomQuestion()
    {
        Random random = new Random();
        int index = random.Next(0, _questions.Count);
        return _questions[index];
    }
    
    public override void Run()
    {
        DisplayStartingMessage();
        
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine();
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.WriteLine();
        Console.WriteLine("When you have something in mind, press enter to continue.");
        Console.ReadLine();
        
        Console.WriteLine("Now ponder on each of the following questions as they related to this experience.");
        Console.Write("You may begin in: ");
        ShowCountDown(5);
        Console.Clear();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        while (DateTime.Now < endTime)
        {
            Console.Write($"> {GetRandomQuestion()} ");
            ShowSpinner(10);
            Console.WriteLine();
        }
        
        DisplayEndingMessage();
    }
}

public class ListingActivity : Activity
{
    private List<string> _prompts;
    
    public ListingActivity() 
        : base("Listing Activity", 
              "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
        _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };
    }
    
    public string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(0, _prompts.Count);
        return _prompts[index];
    }
    
    public override void Run()
    {
        DisplayStartingMessage();
        
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.Write("You may begin in: ");
        ShowCountDown(5);
        Console.WriteLine();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        int count = 0;
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            Console.ReadLine();
            count++;
        }
        
        Console.WriteLine($"You listed {count} items!");
        
        DisplayEndingMessage();
    }
}



public class MindfulnessProgram
{
    private static Dictionary<string, int> _activityCounts = new Dictionary<string, int>
    {
        { "Breathing Activity", 0 },
        { "Reflection Activity", 0 },
        { "Listing Activity", 0 },
        { "Gratitude Activity", 0 }
    };
        public static void IncrementActivityCount(string activityName)
    {
        if (_activityCounts.ContainsKey(activityName))
        {
            _activityCounts[activityName]++;
        }
    }
        public static int GetActivityCount(string activityName)
    {
        if (_activityCounts.ContainsKey(activityName))
        {
            return _activityCounts[activityName];
        }
        return 0;
    }
}

public class GratitudeActivity : Activity
{
    private List<string> _prompts;
    private List<string> _deeperQuestions;
    
    public GratitudeActivity() 
        : base("Gratitude Activity", 
              "This activity will help you develop an attitude of gratitude by focusing on specific things you're thankful for and exploring why they matter to you.")
    {
        _prompts = new List<string>
        {
            "Think of something in nature that you're grateful for.",
            "Think of a person in your life that you're grateful for.",
            "Think of a skill or ability you have that you're grateful for.",
            "Think of a challenge you've overcome that you're now grateful for.",
            "Think of a small everyday comfort that you're grateful for."
        };
        
        _deeperQuestions = new List<string>
        {
            "How has this enriched your life?",
            "What would be different if this wasn't in your life?",
            "When did you first realize you were grateful for this?",
            "How can you express your gratitude for this?",
            "How does focusing on this gratitude make you feel right now?"
        };
    }
    
    public string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(0, _prompts.Count);
        return _prompts[index];
    }
    
    public string GetRandomQuestion()
    {
        Random random = new Random();
        int index = random.Next(0, _deeperQuestions.Count);
        return _deeperQuestions[index];
    }
    
    public override void Run()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        
        DisplayStartingMessage();
        
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine();
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.WriteLine();
        Console.WriteLine("When you have something in mind, press enter to continue.");
        Console.ReadLine();
        
        Console.WriteLine("Write down what you're grateful for:");
        Console.Write("> ");
        string gratitudeItem = Console.ReadLine();
        
        Console.WriteLine("\nNow reflect deeper on your gratitude:");
        
        foreach (string question in _deeperQuestions)
        {
            Console.WriteLine($"> {question}");
            ShowSpinner(3);
            Console.Write("Your thoughts: ");
            Console.ReadLine();
            Console.WriteLine();
        }
        
        MindfulnessProgram.IncrementActivityCount(_name);
        
        DisplayEndingMessage();
        
        Console.ResetColor();
    }
}

public class EnhancedBreathingActivity : BreathingActivity
{
    public override void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        
        DisplayStartingMessage();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        int inhaleDuration = 2;  
        int exhaleDuration = 3;
        int maxInhaleDuration = 6;  
        int maxExhaleDuration = 8;
        bool increasing = true;
        
        while (DateTime.Now < endTime)
        {
            Console.Write("Breathe in...");
            ShowCountDown(inhaleDuration);
            Console.WriteLine();
            
            Console.Write("Breathe out...");
            ShowCountDown(exhaleDuration);
            Console.WriteLine();
            
            if (increasing)
            {
                inhaleDuration = Math.Min(inhaleDuration + 1, maxInhaleDuration);
                exhaleDuration = Math.Min(exhaleDuration + 1, maxExhaleDuration);
                
                if (inhaleDuration >= maxInhaleDuration)
                {
                    increasing = false;
                }
            }
            else
            {
                inhaleDuration = Math.Max(inhaleDuration - 1, 2);
                exhaleDuration = Math.Max(exhaleDuration - 1, 3);
                
                if (inhaleDuration <= 2)
                {
                    increasing = true;
                }
            }
        }
        
        MindfulnessProgram.IncrementActivityCount(_name);
        
        DisplayEndingMessage();
        
        Console.ResetColor();
    }
}

public class EnhancedReflectionActivity : ReflectionActivity
{
    private List<int> _usedQuestionIndices;
    
    public EnhancedReflectionActivity() : base()
    {
        _usedQuestionIndices = new List<int>();
    }
    
    public string GetNonRepeatingQuestion()
    {
        Random random = new Random();
        int index;
        
        if (_usedQuestionIndices.Count >= _questions.Count)
        {
            _usedQuestionIndices.Clear();
        }
        
        do
        {
            index = random.Next(0, _questions.Count);
        } while (_usedQuestionIndices.Contains(index));
        
        _usedQuestionIndices.Add(index);
        return _questions[index];
    }
    
    public override void Run()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        
        DisplayStartingMessage();
        
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine();
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.WriteLine();
        Console.WriteLine("When you have something in mind, press enter to continue.");
        Console.ReadLine();
        
        Console.WriteLine("Now ponder on each of the following questions as they related to this experience.");
        Console.Write("You may begin in: ");
        ShowCountDown(5);
        Console.Clear();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        while (DateTime.Now < endTime)
        {
            Console.Write($"> {GetNonRepeatingQuestion()} ");
            ShowSpinner(10);
            Console.WriteLine();
        }
        
        MindfulnessProgram.IncrementActivityCount(_name);
        
        DisplayEndingMessage();
        
        Console.ResetColor();
    }
}

public class EnhancedListingActivity : ListingActivity
{
    public override void Run()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        
        DisplayStartingMessage();
        
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine($"--- {GetRandomPrompt()} ---");
        Console.Write("You may begin in: ");
        ShowCountDown(5);
        Console.WriteLine();
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddSeconds(_duration);
        
        List<string> items = new List<string>();
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string item = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(item))
            {
                items.Add(item);
            }
        }
        
        Console.WriteLine($"You listed {items.Count} items!");
        
        MindfulnessProgram.IncrementActivityCount(_name);
        
        DisplayEndingMessage();
        
        Console.ResetColor();
    }
}

class EnhancedProgram
{
    static void Main(string[] args)
    {
        bool running = true;
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("==================");
            Console.WriteLine($"1. Start Breathing Activity (Done {MindfulnessProgram.GetActivityCount("Breathing Activity")} times)");
            Console.WriteLine($"2. Start Reflection Activity (Done {MindfulnessProgram.GetActivityCount("Reflection Activity")} times)");
            Console.WriteLine($"3. Start Listing Activity (Done {MindfulnessProgram.GetActivityCount("Listing Activity")} times)");
            Console.WriteLine($"4. Start Gratitude Activity (Done {MindfulnessProgram.GetActivityCount("Gratitude Activity")} times)");
            Console.WriteLine("5. Quit");
            Console.Write("Select a choice from the menu: ");
            
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    EnhancedBreathingActivity breathing = new EnhancedBreathingActivity();
                    breathing.Run();
                    break;
                case "2":
                    EnhancedReflectionActivity reflection = new EnhancedReflectionActivity();
                    reflection.Run();
                    break;
                case "3":
                    EnhancedListingActivity listing = new EnhancedListingActivity();
                    listing.Run();
                    break;
                case "4":
                    GratitudeActivity gratitude = new GratitudeActivity();
                    gratitude.Run();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(2000);
                    break;
            }
        }
    }
}