using System;

namespace OwnProject;

public class XmlFileRepository : IFileRepository
{
  public void Save<T>(string path, T data) { }
  public T Load<T>(string path) { return default; }
  public bool Exists(string path) { return false; }
  public void Add<T>(string path, T data) { }
  public void Update<T>(string path, int id, T data) { }
  public void Delete(string path, int id) { }
  public T GetById<T>(string path, int id) { return default; }
  public T FindBy<T>(string path, Func<T, bool> predicate) { return default; }
  public List<T> GetAll<T>(string path) { return new List<T>(); }
  public int Count<T>(string path) { return 0; }
  public bool UserExists(string path, int id) { return false; }
}