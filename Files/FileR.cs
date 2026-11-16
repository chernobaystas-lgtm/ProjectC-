using System;

namespace OwnProject;

public static class FileR
{
  private static IFileRepository _json = new JsonFileRepository();
  private static IFileRepository _xml = new XmlFileRepository();

  public static IFileRepository Json => _json;
  public static IFileRepository Xml => _xml;
}
