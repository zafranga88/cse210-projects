using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string CommentText { get; set; }

    public Comment(string commenterName, string commentText)
    {
        CommenterName = commenterName;
        CommentText = commentText;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; }

    private List<Comment> _comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        _comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public List<Comment> GetComments()
    {
        return _comments;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Video> videos = new List<Video>();

        Video video1 = new Video("Programming Basics", "CodeMaster", 3600);
        video1.AddComment(new Comment("JavaFan", "Great tutorial!"));
        video1.AddComment(new Comment("CodeNinja", "Very helpful explanation."));
        video1.AddComment(new Comment("TechLearner", "Awesome content."));
        videos.Add(video1);

        Video video2 = new Video("C# Advanced Techniques", "CSharpPro", 2700);
        video2.AddComment(new Comment("DotNetDev", "Excellent deep dive."));
        video2.AddComment(new Comment("CodeWizard", "Really sophisticated techniques."));
        video2.AddComment(new Comment("ProgrammerPal", "Learned so much!"));
        videos.Add(video2);

        Video video3 = new Video("Object-Oriented Programming", "SoftwareGuru", 4800);
        video3.AddComment(new Comment("ClassDesigner", "Perfect explanation of abstraction."));
        video3.AddComment(new Comment("CodeArchitect", "Comprehensive overview."));
        video3.AddComment(new Comment("TechEnthusiast", "Clarified so many concepts!"));
        videos.Add(video3);

        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");

            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.CommenterName}: {comment.CommentText}");
            }

            Console.WriteLine();
        }
    }
}