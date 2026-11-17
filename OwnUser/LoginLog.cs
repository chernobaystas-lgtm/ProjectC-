using System;

namespace OwnProject;


public class LoginLog
{
    public DateTime Time { get; set; } = DateTime.Now;

    public string Ip { get; set; } = string.Empty;

    public bool Success { get; set; }

    public LoginLog() { }

    public LoginLog(string ip, bool success)
    {
        Ip = ip;
        Success = success;
        Time = DateTime.Now;
    }

    public override string ToString()
    {
        string status = Success ? "Successful" : "Failed";

        return $"{Time:G} | {Ip} | {status}";
    }
}
