using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TextBoard.ApplicationServices.Interfaces;
using TextBoard.ApplicationServices.Messaging;
using TextBoard.ApplicationServices.Messaging.Requests;
using TextBoard.ApplicationServices.Messaging.Responses;
using TextBoard.ApplicationServices.Messaging.ViewModels;
using TextBoard.Data.Contexts;
using TextBoard.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace TextBoard.ApplicationServices.Interfaces;

public class MessagesService : IMessagesService
{
    TextBoardDbContext _context;
    IMapper _mapper;

    public MessagesService(TextBoardDbContext context)
    {
        _context = context;
        _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Message, MessageVM>()).CreateMapper();
    }

    // List messages
    public async Task<ListBoardMessagesResponse> ListBoardMessagesAsync(ListBoardMessagesRequest request)
    {
        var board = await _context.Boards.Where(x => x.Id == request.BoardId).FirstOrDefaultAsync();
        if(board == null || board.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found");

        var list = await _context.Messages.Where(x => x.Board.Id == request.BoardId).Select(x => _mapper.Map<MessageVM>(x)).ToListAsync();

        if(request.PageSize < 0)
            return new(request.BoardId, list);
        return new(request.BoardId, list.Skip(request.Page * request.PageSize).Take(request.PageSize).ToList(), request.Page, request.PageSize);
    }


    // Get messages
    public async Task<GetMessageResponse> GetMessageByIdAsync(GetMessageByIdRequest request)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(message == null)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found");
        return new(_mapper.Map<MessageVM>(message));
    }


    // Create message
    public async Task<CreateMessageResponse> CreateMessageAsync(CreateMessageRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if(user == null)
            return new(BusinessStatusCodeEnum.NotFound, "User Not Found");

        var board = await _context.Boards.FirstOrDefaultAsync(x => x.Id == request.BoardId);
        if(board == null)
            return new(BusinessStatusCodeEnum.NotFound, "Board Not Found");

        if(request.Content.Length < 1 || request.Content.Length > 2048)
            return new(BusinessStatusCodeEnum.InvalidInput, "Content needs to be within 1 and 2048 bytes.");

        Message message = new Message(){
            Board = board,
            User = user,
            Content = request.Content
        };

        _context.Add(message);
        await _context.SaveChangesAsync();

        return new(message.Id);
    }


    // Modify message
    public async Task<ReplaceMessageResponse> ReplaceMessageAsync(ReplaceMessageRequest request)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == request.OriginalId);
        if(message == null)
            return new(BusinessStatusCodeEnum.NotFound, "Message Not Found");

        if(request.Content.Length < 1 || request.Content.Length > 2048)
            return new(BusinessStatusCodeEnum.InvalidInput, "Content needs to be within 1 and 2048 bytes.");

        var user = message.User;
        var board = message.Board;

        Message new_message = new Message(){
            Board = board,
            User = user,
            Content = request.Content,
            OverrideMessage = message
        };

        _context.Add(new_message);
        await _context.SaveChangesAsync();

        return new(new_message.Id);
    }


    // Delete message
    public async Task<DeleteMessageResponse> DeleteMessageAsync(DeleteMessageRequest request)
    {
        if(request.Reason.Length < 1 || request.Reason.Length > 2048)
            return new(BusinessStatusCodeEnum.InvalidInput, "Reason needs to be within 1 and 2048 bytes.");

        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == request.Id);
        if(message == null)
            return new(BusinessStatusCodeEnum.NotFound, "Message Not Found");

        do {
            var child = await _context.Messages.FirstOrDefaultAsync(x => x.OverrideMessage == message);
            if(child == null)
                break;
            if(child.DeleteEvent)
                return new(BusinessStatusCodeEnum.NotFound, "Already marked as deleted");
            message = child;
        } while(true);

        var user = message.User;
        var board = message.Board;

        Message new_message = new Message(){
            Board = board,
            User = user,
            Content = request.Reason,
            OverrideMessage = message,
            DeleteEvent = true
        };

        _context.Add(new_message);
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Delete event created.");
    }

}

