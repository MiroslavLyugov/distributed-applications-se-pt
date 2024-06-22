namespace TextBoard.ApplicationServices.Messaging.Requests;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Password { get; set; }

    public CreateUserRequest(string name, string password)
    {
        Name = name;
        Password = password;
    }
}
