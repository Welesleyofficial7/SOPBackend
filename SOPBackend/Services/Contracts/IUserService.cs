namespace SOPBackend.Services;

public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User GetUserById(Guid id);
    User? CreateUser(User newUser);
    User UpdateUser(Guid id, User updatedUser);
    bool DeleteUser(Guid id);
}