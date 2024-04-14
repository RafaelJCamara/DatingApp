using DatingApp.Application.Dtos;
using DatingApp.Domain.Models;
using DatingApp.Application.Common.Models;

namespace DatingApp.Application.Interfaces.Repository;

public interface IMessageRepository
{
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessage(int id);
    Task<PagedList<MessageDto>> GetMessagesForUser(MessageParamsDto messageParams);
    Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnection(string connectionId);
    Task<Group> GetMessageGroup(string groupName);
    Task<Group> GetGroupForConnection(string connectionId);
}
