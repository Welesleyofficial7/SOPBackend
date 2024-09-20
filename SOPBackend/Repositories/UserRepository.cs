using SOPBackend.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;


namespace SOPBackend.Repositories;

public class UserRepository : IUserRepository, IDisposable
{

    private ApplicationContext context;
    
    private bool _disposed = false;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<User> GetUsers()
    {
        return context.Users.ToList();
    }

    public User GetUserByID(Guid userId)
    {
        return context.Users.Find(userId);
    }

    public User InsertUser(User user)
    {
        var entry = context.Users.Add(user); 
        context.SaveChanges(); 
        return entry.Entity;
    }

    public bool DeleteUser(User user)
    {

        context.Users.Remove(user);
        
        context.SaveChanges();

        return true; 
    }


    public User UpdateUser(User user)
    {
        
        context.Users.Update(user);
        context.SaveChanges();
        return user;
    }


    public void Save()
    {
        context.SaveChanges();
    }
}