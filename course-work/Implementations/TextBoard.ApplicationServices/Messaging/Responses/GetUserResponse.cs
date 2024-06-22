using TextBoard.ApplicationServices.Messaging.ViewModels;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class GetUserResponse : ResponseBase
{
    public UserVM? User { get; set; }

    public GetUserResponse(UserVM? user)
        : base(user == null ? BusinessStatusCodeEnum.NotFound : BusinessStatusCodeEnum.Success,
               user == null ? "User not found!" : "User data fetched.")
    {
        User = user;
    }

    public GetUserResponse(BusinessStatusCodeEnum statusCode, string message)
        : base(statusCode, message)
    {

    }
}
