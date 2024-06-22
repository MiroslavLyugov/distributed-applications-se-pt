namespace TextBoard.ApplicationServices.Messaging.Requests;

public class AuthenticateUserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }

    public AuthenticateUserRequest(string name, string pwd)
    {
        Name = name;
        Password = pwd;
    }
}
