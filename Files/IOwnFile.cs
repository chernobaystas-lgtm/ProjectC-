using System;

namespace OwnProject;

public interface IFileRepository
{
  void Save<T>(string path, T data); // Сохранить файл и выводит успешно или нет
  T Load<T>(string path); // Достает информацию из файла
  void Update<T>(string path, int id, T data); // Если нужно обновить информацию об пользователе
  void Add<T>(string path, T data); // Добавить информацию об пользователе
  void Delete<T>(string path, int id); // Удалить информацию об пользователе
  public void DeleteUserInfo(string path, int id);
  bool Exists(string path); // есть ли файл и в каком он состоянии
  bool UserExists(string path, int id); // существует ли пользователь
  T GetById<T>(string path, int id); // найти информацию по айди

  // Пошук
  T FindBy<T>(string path, Func<T, bool> predicate);
  List<T> GetAll<T>(string path);
  int Count<T>(string path);

}





