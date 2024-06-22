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

namespace TextBoard.ApplicationServices.Implementation;

public class BoardsService : IBoardsService
{
    TextBoardDbContext _context;
    IMapper _mapper;

    public BoardsService(TextBoardDbContext context)
    {
        _context = context;
        _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Board, BoardVM>()).CreateMapper();
    }


    public async Task<ListBoardsResponse> ListBoardsAsync(ListBoardsRequest request)
    {
        var list = await _context.Boards.Where(x => !x.Deleted && !x.Hidden).Select(x => _mapper.Map<BoardVM>(x)).ToListAsync();

        if(request.PageSize < 0)
            return new(list);
        return new(list.Skip(request.Page * request.PageSize).Take(request.PageSize).ToList(), request.Page, request.PageSize);
    }

    public async Task<ListUserBoardsResponse> ListUserBoardsAsync(ListUserBoardsRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && !x.Deleted);
        if(user == null)
            return new(request.UserId, BusinessStatusCodeEnum.NotFound, "User not found");

        var list = await _context.Boards.Include(x => x.Messages).Where(x => !x.Deleted && x.Messages.FirstOrDefault(m => m.User == user) != null).Select(x => _mapper.Map<BoardVM>(x)).ToListAsync();

        if(request.PageSize < 0)
            return new(request.UserId, list);
        return new(request.UserId, list.Skip(request.Page * request.PageSize).Take(request.PageSize).ToList(), request.Page, request.PageSize);
    }


    public async Task<GetBoardResponse> GetBoardByNameAsync(GetBoardByNameRequest request)
    {
        var board = await _context.Boards.Where(x => !x.Deleted).SingleOrDefaultAsync(x => x.Name == request.Name);
        return new(_mapper.Map<BoardVM>(board));
    }

    public async Task<GetBoardResponse> GetBoardByIdAsync(GetBoardByIdRequest request)
    {
        var board = await _context.Boards.Where(x => !x.Deleted).SingleOrDefaultAsync(x => x.Id == request.Id);
        return new(_mapper.Map<BoardVM>(board));
    }


    public async Task<CreateBoardResponse> CreateBoardAsync(CreateBoardRequest request)
    {
        if(request.Name.Length < 1)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be at least 1 characters");
        if(request.Name.Length > 128)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be no more than 128 characters");

        Board entry = new () {
            Name = request.Name,
            Hidden = request.Hidden
        };
        _context.Add(entry);
        await _context.SaveChangesAsync();
        return new(entry.Id);
    }


    public async Task<UpdateBoardResponse> UpdateBoardAsync(UpdateBoardRequest request)
    {
        if(request.Name != null && request.Name.Length < 1)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be at least 1 character");
        if(request.Name != null && request.Name.Length > 128)
            return new(BusinessStatusCodeEnum.InvalidInput, "Name must be no more than 128 characters");

        var board = await _context.Boards.SingleOrDefaultAsync(x => x.Id == request.Id);

        if(board == null || board.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found!");

        if(request.Name != null)
            board.Name = request.Name;
        if(request.Hidden != null)
            board.Hidden = (bool) request.Hidden;
        if(request.Archived != null)
            board.Archived = (bool) request.Archived;

        await _context.SaveChangesAsync();

        return new(board.Id);
    }

    public async Task<ArchiveBoardResponse> ArchiveBoardAsync(ArchiveBoardRequest request)
    {
        var board = await _context.Boards.SingleOrDefaultAsync(x => x.Id == request.Id);

        if(board == null || board.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found!");

        if(board.Archived)
            return new(BusinessStatusCodeEnum.InvalidInput, "Board already archived!");

        board.Archived = true;

        await _context.SaveChangesAsync();

        return new(board.Id);
    }


    public async Task<DeleteBoardResponse> DeleteBoardAsync(DeleteBoardRequest request)
    {
        var board = await _context.Boards.SingleOrDefaultAsync(x => x.Id == request.Id);
        if(board == null)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found.");
        if(board.Deleted)
            return new(BusinessStatusCodeEnum.NotFound, "Board not found");

        board.Deleted = true;
        await _context.SaveChangesAsync();

        return new(BusinessStatusCodeEnum.Success, "Board deleted.");
    }

}

