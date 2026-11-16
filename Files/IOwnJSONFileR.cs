using System;
using System.IO;
using System.Text;
using System.Text.Json;
using static System.Console;

namespace OwnProject;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

public class JsonFileRepository : IFileRepository
{
  public void Save<T>(string path, T data)
  {
    string json = JsonSerializer.Serialize(data);
    byte[] bytes = Encoding.UTF8.GetBytes(json);
    string hex = Convert.ToHexString(bytes);
    File.WriteAllText(path, hex);
  }

  public T Load<T>(string path)
  {
    if (!File.Exists(path)) return default;
    string hex = File.ReadAllText(path);
    byte[] bytes = Convert.FromHexString(hex);
    string json = Encoding.UTF8.GetString(bytes);
    return JsonSerializer.Deserialize<T>(json);
  }

  public bool Exists(string path)
  {
    return File.Exists(path);
  }

  public void Add<T>(string path, T data)
  {
    var list = Load<List<T>>(path) ?? new List<T>();
    list.Add(data);
    Save(path, list);
  }

  public void Update<T>(string path, int id, T data)
  {
    var list = Load<List<T>>(path);
    if (list == null || id < 0 || id >= list.Count) return;
    list[id] = data;
    Save(path, list);
  }

  public void Delete(string path, int id)
  {
    var list = Load<List<object>>(path);
    if (list == null || id < 0 || id >= list.Count) return;
    list.RemoveAt(id);
    Save(path, list);
  }

  public T GetById<T>(string path, int id)
  {
    var list = Load<List<T>>(path);
    if (list == null || id < 0 || id >= list.Count) return default;
    return list[id];
  }

  public T FindBy<T>(string path, Func<T, bool> predicate)
  {
    var list = Load<List<T>>(path);
    return list == null ? default : list.FirstOrDefault(predicate);
  }

  public List<T> GetAll<T>(string path)
  {
    return Load<List<T>>(path) ?? new List<T>();
  }

  public int Count<T>(string path)
  {
    var list = Load<List<T>>(path);
    return list?.Count ?? 0;
  }

  public bool UserExists(string path, int id)
  {
    var list = Load<List<object>>(path);
    if (list == null) return false;

    foreach (var item in list)
    {
      if (item == null) continue;

      if (item is JsonElement element && element.ValueKind == JsonValueKind.Object)
      {
        if (element.TryGetProperty("Id", out var idElement) && idElement.ValueKind == JsonValueKind.Number && idElement.TryGetInt32(out int existingId) && existingId == id)
          return true;
      }
      else
      {
        var property = item.GetType().GetProperty("Id");
        if (property != null && property.PropertyType == typeof(int))
        {
          var value = property.GetValue(item);
          if (value is int existingId && existingId == id)
            return true;
        }
      }
    }

    return false;
  }
}