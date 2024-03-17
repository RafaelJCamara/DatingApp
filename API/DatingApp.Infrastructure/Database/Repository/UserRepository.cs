using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Repository;
using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Database.Repository;

public class UserRepository : IUserRepository
{

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users.Include(x => x.Photos).ToListAsync();
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParamsDto userParams)
    {
        var query = _context.Users.AsQueryable();

        query = query.Where(u => u.UserName != userParams.CurrentUsername);
        query = query.Where(u => u.Gender == userParams.Gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive)
        };

        return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking()
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
            userParams.PageNumber, userParams.PageSize);
    }

    public async Task<MemberDto> GetMemberByUsernameAsync(string username)
    {
        return await _context.Users
            .Where(user => user.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<string> GetUserGender(string username)
    {
        return (await _context.Users.FirstOrDefaultAsync(x => x.UserName.Equals(username)))?.Gender;
    }
}