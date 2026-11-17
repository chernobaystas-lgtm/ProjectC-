
namespace OwnProject;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

  private void MergeUser(string path, User existing, User user, bool replaceLists)
  {

    /*
   * Обновляет данные пользователя и сохраняет результат в файл.
   *
   * Параметры:
   * - path: путь к файлу, куда сохранить результат
   * - existing: старый пользователь из файла (кого обновляем)
   * - user: новые данные (что обновляем)
   * - replaceLists: заменять списки (true) или добавлять к ним (false)
   *
   * Что делает по шагам:
   * 1. Email: если в user есть email — ставим в existing
   * 2. PasswordHash: если в user есть пароль — ставим в existing
   * 3. Списки (Drawings, LoginHistory):
   *    - replaceLists = true  -> existing.Drawings = user.Drawings (заменить)
   *    - replaceLists = false -> existing.Drawings.AddRange(user.Drawings) (добавить)
   * 4. Save: сохраняет existing в файл по path
   */


    if (!string.IsNullOrEmpty(user.Email))
      existing.Email = user.Email;

    if (!string.IsNullOrEmpty(user.PasswordHash))
      existing.PasswordHash = user.PasswordHash;

    if (replaceLists)
    {
      if (user.Drawings.Count > 0)
        existing.Drawings = user.Drawings;
      if (user.LoginHistory.Count > 0)
        existing.LoginHistory = user.LoginHistory;
    }
    else
    {
      if (user.Drawings.Count > 0)
        existing.Drawings.AddRange(user.Drawings);
      if (user.LoginHistory.Count > 0)
        existing.LoginHistory.AddRange(user.LoginHistory);
    }

    Save(path, existing);
  }

  public void Add<T>(string path, T data)
  {
    if (typeof(T) == typeof(User))
    {
      var user = data as User;
      var existing = Load<User>(path);

      if (existing != null && user != null && existing.Id == user.Id)
      {
        MergeUser(path, existing, user, replaceLists: false);
        return;
      }
    }

    var list = Load<List<T>>(path) ?? new List<T>();
    list.Add(data);
    Save(path, list);
  }

  public void Update<T>(string path, int id, T data)
  {
    if (typeof(T) == typeof(User))
    {
      var user = data as User;
      var existing = Load<User>(path);

      if (existing != null && user != null && existing.Id == id)
      {
        MergeUser(path, existing, user, replaceLists: true);
        return;
      }
    }

    var list = Load<List<T>>(path);
    if (list == null || id < 0 || id >= list.Count) return;
    list[id] = data;
    Save(path, list);
  }




  public void DeleteUserInfo(string path, int id)
  {
    var list = Load<List<User>>(path);

    if (list == null)
      return;

    var user = list.FirstOrDefault(u => u.Id == id);

    if (user == null)
      return;

    list.Remove(user);
    Save(path, list);
  }


  public T? GetById<T>(string path, int id)
  {
    var list = Load<List<T>>(path);

    if (list == null)
      return default;

    var property = typeof(T).GetProperty("Id");

    if (property == null || property.PropertyType != typeof(int))
      return default;

    return list.FirstOrDefault(item =>
        item != null &&
        (int)property.GetValue(item)! == id);
  }

  public T? FindBy<T>(string path, Func<T, bool> predicate)
  {
    if (predicate == null)
      throw new ArgumentNullException(nameof(predicate));

    var list = Load<List<T>>(path);

    if (list == null)
      return default;

    return list.FirstOrDefault(predicate);
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

  public void Delete<T>(string path, int id)
  {
    var list = Load<List<T>>(path);

    if (list == null)
      return;

    var item = list.FirstOrDefault(x =>
    {
      var property = typeof(T).GetProperty("Id");

      if (property == null || property.PropertyType != typeof(int))
        return false;

      return (int)property.GetValue(x)! == id;
    });

    if (item == null)
      return;

    list.Remove(item);
    Save(path, list);
  }
}