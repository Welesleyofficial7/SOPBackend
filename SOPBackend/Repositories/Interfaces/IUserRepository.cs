namespace SOPBackend.Repositories.Interfaces;

public interface IUserRepository : IDisposable
{
    IEnumerable<User> GetUsers();
    User GetUserByID(Guid userId);
    User InsertUser(User user);
    bool DeleteUser(User user);
    User UpdateUser(User user);
    void Save();
}