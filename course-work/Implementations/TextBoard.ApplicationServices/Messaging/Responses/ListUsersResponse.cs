using TextBoard.ApplicationServices.Messaging.ViewModels;
using System.Collections.Generic;

namespace TextBoard.ApplicationServices.Messaging.Responses;

public class ListUsersResponse : ResponseBase
{
    public ICollection<UserVM> Users { get; set; }
    int Page { get; set;} = 0;
    int PageSize { get; set; } = -1;

    public ListUsersResponse(ICollection<UserVM> users)
        : base(BusinessStatusCodeEnum.Success, "User list fetched.")
    {
        Users = users;
    }

    public ListUsersResponse(ICollection<UserVM> users, int page, int size)
        : base(BusinessStatusCodeEnum.Success, "User list fetched.")
    {
        Users = users;
        Page = page;
        PageSize = size;
    }

    public ListUsersResponse(BusinessStatusCodeEnum status, string message)
        : base(status, message)
    {
        Users = new List<UserVM>();
    }
}
