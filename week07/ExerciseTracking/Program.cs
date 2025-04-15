/* 
EXCEEDING REQUIREMENTS:

1. Enhanced Gamification Features:Users gain levels as they earn points, users unlock achievements for reaching milestones, visual feedback when leveling up or unlocking achievements.

2. Additional Goal Type:Progressive goal that allows tracking progress toward larger goals with incremental rewards, and gives points for each progress step plus a completion bonus.

3. User Experience Improvements:Clear UI with organized menus and feedback, validation for user inputs, detailed goal status displays, achievement tracking and display.
   
4. File Format Improvements:Structured file format that saves all program state, saves achievements and level information, robust error handling for file operations.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EternalQuest
{
    public abstract class Goal
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Points { get; protected set; }
        public bool IsComplete { get; protected set; }

        public Goal(string name, string description, int points)
        {
            Name = name;
            Description = description;
            Points = points;
            IsComplete = false;
        }

        public abstract int RecordEvent();
        public abstract string GetStringRepresentation();
        public abstract string GetDetailsString();

        public string GetStatusMark()
        {
            return IsComplete ? "[X]" : "[ ]";
        }
    }

    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, string description, int points) 
            : base(name, description, points)
        {
        }

        public SimpleGoal(string name, string description, int points, bool isComplete) 
            : base(name, description, points)
        {
            IsComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (!IsComplete)
            {
                IsComplete = true;
                return Points;
            }
            return 0;
        }

        public override string GetStringRepresentation()
        {
            return $"SimpleGoal:{Name},{Description},{Points},{IsComplete}";
        }

        public override string GetDetailsString()
        {
            return $"{GetStatusMark()} {Name} ({Description})";
        }
    }

    public class EternalGoal : Goal
    {
        public int TimesCompleted { get; private set; }

        public EternalGoal(string name, string description, int points) 
            : base(name, description, points)
        {
            TimesCompleted = 0;
        }

        public EternalGoal(string name, string description, int points, int timesCompleted) 
            : base(name, description, points)
        {
            TimesCompleted = timesCompleted;
        }

        public override int RecordEvent()
        {
            TimesCompleted++;
            return Points;
        }

        public override string GetStringRepresentation()
        {
            return $"EternalGoal:{Name},{Description},{Points},{TimesCompleted}";
        }

        public override string GetDetailsString()
        {
            return $"{GetStatusMark()} {Name} ({Description}) - Completed {TimesCompleted} times";
        }
    }

    public class ChecklistGoal : Goal
    {
        public int RequiredCount { get; private set; }
        public int CompletedCount { get; private set; }
        public int BonusPoints { get; private set; }

        public ChecklistGoal(string name, string description, int points, int requiredCount, int bonusPoints) 
            : base(name, description, points)
        {
            RequiredCount = requiredCount;
            CompletedCount = 0;
            BonusPoints = bonusPoints;
        }

        public ChecklistGoal(string name, string description, int points, int requiredCount, int completedCount, int bonusPoints) 
            : base(name, description, points)
        {
            RequiredCount = requiredCount;
            CompletedCount = completedCount;
            BonusPoints = bonusPoints;
            IsComplete = CompletedCount >= RequiredCount;
        }

        public override int RecordEvent()
        {
            if (CompletedCount < RequiredCount)
            {
                CompletedCount++;
                
                if (CompletedCount == RequiredCount)
                {
                    IsComplete = true;
                    return Points + BonusPoints;
                }
                
                return Points;
            }
            return 0;
        }

        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal:{Name},{Description},{Points},{RequiredCount},{CompletedCount},{BonusPoints}";
        }

        public override string GetDetailsString()
        {
            return $"{GetStatusMark()} {Name} ({Description}) - Completed {CompletedCount}/{RequiredCount} times";
        }
    }

    public class ProgressiveGoal : Goal
    {
        public int TargetValue { get; private set; }
        public int CurrentProgress { get; private set; }
        public int ProgressPoints { get; private set; }

        public ProgressiveGoal(string name, string description, int completionPoints, int targetValue, int progressPoints)
            : base(name, description, completionPoints)
        {
            TargetValue = targetValue;
            CurrentProgress = 0;
            ProgressPoints = progressPoints;
        }

        public ProgressiveGoal(string name, string description, int completionPoints, int targetValue, int currentProgress, int progressPoints)
            : base(name, description, completionPoints)
        {
            TargetValue = targetValue;
            CurrentProgress = currentProgress;
            ProgressPoints = progressPoints;
            IsComplete = CurrentProgress >= TargetValue;
        }

        public override int RecordEvent()
        {
            if (IsComplete)
                return 0;

            CurrentProgress++;
            
            if (CurrentProgress >= TargetValue)
            {
                IsComplete = true;
                return Points + ProgressPoints;
            }
            
            return ProgressPoints;
        }

        public override string GetStringRepresentation()
        {
            return $"ProgressiveGoal:{Name},{Description},{Points},{TargetValue},{CurrentProgress},{ProgressPoints}";
        }

        public override string GetDetailsString()
        {
            return $"{GetStatusMark()} {Name} ({Description}) - Progress: {CurrentProgress}/{TargetValue}";
        }
    }

    public class QuestManager
    {
        private List<Goal> _goals;
        private int _score;
        private int _level;
        private List<string> _achievements;

        public QuestManager()
        {
            _goals = new List<Goal>();
            _score = 0;
            _level = 1;
            _achievements = new List<string>();
        }

        public void AddGoal(Goal goal)
        {
            _goals.Add(goal);
        }

        public void DisplayGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("You don't have any goals yet. Create some goals first!");
                return;
            }

            Console.WriteLine("\n=== Your Goals ===");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }

        public void RecordEvent()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("You don't have any goals yet. Create some goals first!");
                return;
            }

            DisplayGoals();
            Console.Write("\nWhich goal did you accomplish? Enter the number: ");
            if (int.TryParse(Console.ReadLine(), out int goalIndex) && goalIndex > 0 && goalIndex <= _goals.Count)
            {
                Goal goal = _goals[goalIndex - 1];
                int pointsEarned = goal.RecordEvent();
                
                if (pointsEarned > 0)
                {
                    _score += pointsEarned;
                    Console.WriteLine($"Congratulations! You earned {pointsEarned} points!");
                    
                    int newLevel = 1 + (_score / 1000);
                    if (newLevel > _level)
                    {
                        int levelsGained = newLevel - _level;
                        _level = newLevel;
                        Console.WriteLine($"**** LEVEL UP! You are now level {_level}! ****");
                        
                        if (_level % 5 == 0)
                        {
                            string achievement = $"Milestone: Reached Level {_level}";
                            _achievements.Add(achievement);
                            Console.WriteLine($"**** ACHIEVEMENT UNLOCKED: {achievement} ****");
                        }
                    }
                    
                    CheckForAchievements();
                }
                else
                {
                    Console.WriteLine("No points earned. This goal may already be complete.");
                }
            }
            else
            {
                Console.WriteLine("Invalid goal number.");
            }
        }

        private void CheckForAchievements()
        {
            if (_goals.Count == 5 && !_achievements.Contains("Goal Setter: Created 5 goals"))
            {
                _achievements.Add("Goal Setter: Created 5 goals");
                Console.WriteLine("**** ACHIEVEMENT UNLOCKED: Goal Setter: Created 5 goals ****");
            }
            
            int[] milestones = { 1000, 5000, 10000 };
            foreach (int milestone in milestones)
            {
                string achievement = $"Point Master: Earned {milestone} points";
                if (_score >= milestone && !_achievements.Contains(achievement))
                {
                    _achievements.Add(achievement);
                    Console.WriteLine($"**** ACHIEVEMENT UNLOCKED: {achievement} ****");
                }
            }
            
            int completedGoals = 0;
            foreach (Goal goal in _goals)
            {
                if (goal.IsComplete)
                    completedGoals++;
            }
            
            if (completedGoals == 3 && !_achievements.Contains("Goal Achiever: Completed 3 goals"))
            {
                _achievements.Add("Goal Achiever: Completed 3 goals");
                Console.WriteLine("**** ACHIEVEMENT UNLOCKED: Goal Achiever: Completed 3 goals ****");
            }
        }

        public void DisplayScore()
        {
            Console.WriteLine($"\n=== Your Status ===");
            Console.WriteLine($"Score: {_score} points");
            Console.WriteLine($"Level: {_level} ({_score % 1000}/1000 points to next level)");
            
            if (_achievements.Count > 0)
            {
                Console.WriteLine("\n=== Your Achievements ===");
                foreach (string achievement in _achievements)
                {
                    Console.WriteLine($"- {achievement}");
                }
            }
        }

        public void SaveGoals(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine($"{_score},{_level}");
                    
                    writer.WriteLine(_achievements.Count);
                    foreach (string achievement in _achievements)
                    {
                        writer.WriteLine(achievement);
                    }
                    
                    writer.WriteLine(_goals.Count);
                    foreach (Goal goal in _goals)
                    {
                        writer.WriteLine(goal.GetStringRepresentation());
                    }
                }
                
                Console.WriteLine($"Goals saved successfully to {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving goals: {ex.Message}");
            }
        }

        public void LoadGoals(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine($"File {filename} not found.");
                    return;
                }
                
                string[] lines = File.ReadAllLines(filename);
                int currentLine = 0;
                
                string[] scoreLevel = lines[currentLine++].Split(',');
                _score = int.Parse(scoreLevel[0]);
                _level = int.Parse(scoreLevel[1]);
                
                int achievementCount = int.Parse(lines[currentLine++]);
                _achievements.Clear();
                for (int i = 0; i < achievementCount; i++)
                {
                    _achievements.Add(lines[currentLine++]);
                }
                
                int goalCount = int.Parse(lines[currentLine++]);
                _goals.Clear();
                
                for (int i = 0; i < goalCount; i++)
                {
                    string line = lines[currentLine++];
                    string[] parts = line.Split(':');
                    string goalType = parts[0];
                    string[] goalData = parts[1].Split(',');
                    
                    Goal goal = null;
                    switch (goalType)
                    {
                        case "SimpleGoal":
                            goal = new SimpleGoal(goalData[0], goalData[1], int.Parse(goalData[2]), bool.Parse(goalData[3]));
                            break;
                        case "EternalGoal":
                            goal = new EternalGoal(goalData[0], goalData[1], int.Parse(goalData[2]), int.Parse(goalData[3]));
                            break;
                        case "ChecklistGoal":
                            goal = new ChecklistGoal(goalData[0], goalData[1], int.Parse(goalData[2]), 
                                                    int.Parse(goalData[3]), int.Parse(goalData[4]), int.Parse(goalData[5]));
                            break;
                        case "ProgressiveGoal":
                            goal = new ProgressiveGoal(goalData[0], goalData[1], int.Parse(goalData[2]),
                                                      int.Parse(goalData[3]), int.Parse(goalData[4]), int.Parse(goalData[5]));
                            break;
                    }
                    
                    if (goal != null)
                        _goals.Add(goal);
                }
                
                Console.WriteLine($"Goals loaded successfully from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            QuestManager questManager = new QuestManager();
            bool quit = false;

            Console.WriteLine("Welcome to the Eternal Quest Program!");
            Console.WriteLine("This program helps you track your goals and progress on your eternal journey.");

            while (!quit)
            {
                Console.WriteLine("\n=== Menu Options ===");
                Console.WriteLine("1. Create New Goal");
                Console.WriteLine("2. List Goals");
                Console.WriteLine("3. Save Goals");
                Console.WriteLine("4. Load Goals");
                Console.WriteLine("5. Record Event");
                Console.WriteLine("6. Show Score");
                Console.WriteLine("7. Quit");
                Console.Write("Select a choice from the menu: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateGoal(questManager);
                        break;
                    case "2":
                        questManager.DisplayGoals();
                        break;
                    case "3":
                        Console.Write("Enter filename to save: ");
                        string saveFilename = Console.ReadLine();
                        questManager.SaveGoals(saveFilename);
                        break;
                    case "4":
                        Console.Write("Enter filename to load: ");
                        string loadFilename = Console.ReadLine();
                        questManager.LoadGoals(loadFilename);
                        break;
                    case "5":
                        questManager.RecordEvent();
                        break;
                    case "6":
                        questManager.DisplayScore();
                        break;
                    case "7":
                        quit = true;
                        Console.WriteLine("Thank you for using the Eternal Quest Program. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void CreateGoal(QuestManager questManager)
        {
            Console.WriteLine("\n=== Goal Types ===");
            Console.WriteLine("1. Simple Goal - Complete once for points");
            Console.WriteLine("2. Eternal Goal - Repeatable goals that never complete");
            Console.WriteLine("3. Checklist Goal - Complete multiple times with bonus");
            Console.WriteLine("4. Progressive Goal - Track progress towards a larger goal");
            Console.Write("Which type of goal would you like to create? ");

            string goalType = Console.ReadLine();
            
            Console.Write("What is the name of your goal? ");
            string name = Console.ReadLine();
            
            Console.Write("What is a short description of it? ");
            string description = Console.ReadLine();
            
            Console.Write("How many points is this goal worth? ");
            int points = int.Parse(Console.ReadLine());

            Goal goal = null;

            switch (goalType)
            {
                case "1": 
                    goal = new SimpleGoal(name, description, points);
                    break;
                
                case "2": 
                    goal = new EternalGoal(name, description, points);
                    break;
                
                case "3": 
                    Console.Write("How many times does this goal need to be accomplished? ");
                    int targetCount = int.Parse(Console.ReadLine());
                    
                    Console.Write("What is the bonus for accomplishing it that many times? ");
                    int bonus = int.Parse(Console.ReadLine());
                    
                    goal = new ChecklistGoal(name, description, points, targetCount, bonus);
                    break;
                
                case "4":
                    Console.Write("What is the target value to reach? ");
                    int targetValue = int.Parse(Console.ReadLine());
                    
                    Console.Write("How many points for each progress step? ");
                    int progressPoints = int.Parse(Console.ReadLine());
                    
                    goal = new ProgressiveGoal(name, description, points, targetValue, progressPoints);
                    break;
                
                default:
                    Console.WriteLine("Invalid goal type.");
                    return;
            }

            if (goal != null)
            {
                questManager.AddGoal(goal);
                Console.WriteLine("Goal created successfully!");
            }
        }
    }
}

