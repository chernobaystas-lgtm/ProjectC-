using System;

namespace OwnProject;


public class Drawing
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? ModifiedAt { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public Drawing() { }

    public Drawing(string name, string filePath, int width, int height)
    {
        Name = name;
        FilePath = filePath;
        Width = width;
        Height = height;
    }

    public void Update(string name, string filePath, int width, int height)
    {
        Name = name;
        FilePath = filePath;
        Width = width;
        Height = height;
        ModifiedAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Name} ({Width}x{Height})";
    }
}
