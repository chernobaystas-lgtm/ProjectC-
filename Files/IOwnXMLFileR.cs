using System;

namespace OwnProject;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public class XmlFileRepository : IFileRepository
{
  // Сохранение объекта в XML
  public void Save<T>(string path, T data)
  {
    if (string.IsNullOrWhiteSpace(path))
      throw new ArgumentException("Путь к файлу не может быть пустым.", nameof(path));

    string fullPath = Path.GetFullPath(path);
    string? directory = Path.GetDirectoryName(fullPath);

    if (!string.IsNullOrEmpty(directory))
      Directory.CreateDirectory(directory);

    var serializer = new XmlSerializer(typeof(T));

    var settings = new XmlWriterSettings
    {
      Encoding = new UTF8Encoding(false),
      Indent = true
    };

    using var writer = XmlWriter.Create(fullPath, settings);
    serializer.Serialize(writer, data);
  }

  // Загрузка объекта из XML
  public T Load<T>(string path)
  {
    if (!File.Exists(path))
      return default!;

    if (new FileInfo(path).Length == 0)
      return default!;

    var serializer = new XmlSerializer(typeof(T));

    using var stream = File.OpenRead(path);

    return (T)serializer.Deserialize(stream)!;
  }

  // Проверка существования файла
  public bool Exists(string path)
  {
    return File.Exists(path);
  }

  // Объединение информации пользователя
  private void MergeUser(
      User existing,
      User user,
      bool replaceLists)
  {
    if (!string.IsNullOrWhiteSpace(user.Email))
      existing.Email = user.Email;

    if (!string.IsNullOrWhiteSpace(user.PasswordHash))
      existing.PasswordHash = user.PasswordHash;

    if (replaceLists)
    {
      // Заменяем списки целиком, в том числе пустыми
      existing.Drawings = user.Drawings?.ToList()
          ?? new List<Drawing>();

      existing.LoginHistory = user.LoginHistory?.ToList()
          ?? new List<LoginLog>();
    }
    else
    {
      // Добавляем новые элементы к существующим
      existing.Drawings ??= new List<Drawing>();
      existing.LoginHistory ??= new List<LoginLog>();

      if (user.Drawings != null)
        existing.Drawings.AddRange(user.Drawings);

      if (user.LoginHistory != null)
        existing.LoginHistory.AddRange(user.LoginHistory);
    }
  }

  // Добавление объекта
  public void Add<T>(string path, T data)
  {
    // Отдельная обработка пользователей
    if (data is User user)
    {
      var users = Load<List<User>>(path)
                  ?? new List<User>();

      var existing = users.FirstOrDefault(
          u => u != null && u.Id == user.Id);

      if (existing != null)
      {
        // Если пользователь уже существует,
        // обновляем его данные и добавляем новые записи
        MergeUser(existing, user, replaceLists: false);
      }
      else
      {
        users.Add(user);
      }

      Save(path, users);
      return;
    }

    // Обычное добавление в список
    var list = Load<List<T>>(path)
               ?? new List<T>();

    list.Add(data);

    Save(path, list);
  }

  // Обновление объекта
  public void Update<T>(string path, int id, T data)
  {
    // Отдельная обработка пользователей
    if (typeof(T) == typeof(User))
    {
      if (data is not User user)
        return;

      var users = Load<List<User>>(path);

      if (users == null)
        return;

      int userIndex = users.FindIndex(
          u => u != null && u.Id == id);

      if (userIndex == -1 || user.Id != id)
        return;

      MergeUser(
          users[userIndex],
          user,
          replaceLists: true);

      Save(path, users);
      return;
    }

    // Обновление остальных объектов по Id
    var list = Load<List<T>>(path);

    if (list == null)
      return;

    int index = list.FindIndex(
        item => TryGetId(item, out int itemId) && itemId == id);

    if (index == -1)
      return;

    list[index] = data;

    Save(path, list);
  }

  // Проверка наличия свойства Id у объекта
  private static bool TryGetId<T>(T item, out int id)
  {
    id = 0;

    if (item == null)
      return false;

    var property = item.GetType().GetProperty(
        "Id",
        BindingFlags.Public | BindingFlags.Instance);

    if (property == null || property.PropertyType != typeof(int))
      return false;

    var value = property.GetValue(item);

    if (value is not int foundId)
      return false;

    id = foundId;
    return true;
  }

  // Удаление пользователя по Id
  public void DeleteUserInfo(string path, int id)
  {
    Delete<User>(path, id);
  }

  // Получение объекта по Id
  public T GetById<T>(string path, int id)
  {
    var list = Load<List<T>>(path);

    if (list == null)
      return default!;

    return list.FirstOrDefault(
        item => TryGetId(item, out int itemId) && itemId == id)!;
  }

  // Поиск объекта по условию
  public T FindBy<T>(string path, Func<T, bool> predicate)
  {
    if (predicate == null)
      throw new ArgumentNullException(nameof(predicate));

    var list = Load<List<T>>(path);

    if (list == null)
      return default!;

    return list.FirstOrDefault(predicate)!;
  }

  // Получение всех объектов
  public List<T> GetAll<T>(string path)
  {
    return Load<List<T>>(path) ?? new List<T>();
  }

  // Получение количества объектов
  public int Count<T>(string path)
  {
    var list = Load<List<T>>(path);

    return list?.Count ?? 0;
  }

  // Проверка существования пользователя по Id
  public bool UserExists(string path, int id)
  {
    var users = Load<List<User>>(path);

    if (users == null)
      return false;

    return users.Any(user =>
        user != null && user.Id == id);
  }

  // Удаление объекта по настоящему Id
  public void Delete<T>(string path, int id)
  {
    var list = Load<List<T>>(path);

    if (list == null)
      return;

    int index = list.FindIndex(
        item => TryGetId(item, out int itemId) && itemId == id);

    if (index == -1)
      return;

    list.RemoveAt(index);

    Save(path, list);
  }

  // Совместимость с интерфейсом,
  // если в нём остался старый Delete без <T>
  public void Delete(string path, int id)
  {
    Delete<User>(path, id);
  }
}
