using System;
using System.Collections.Generic;
using System.Linq;

namespace SOPBackend.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationContext _context;

        public MenuItemService(ApplicationContext context)
        {
            _context = context;
        }
        
        public IEnumerable<MenuItem>? GetAllMenuItems()
        {
            return _context.MenuItems.ToList();
        }
        
        public MenuItem? GetMenuItemById(Guid id)
        {
            var menuItem = _context.MenuItems.Find(id);
            if (menuItem == null)
            {
                throw new Exception("Menu item not found");
            }
            return menuItem;
        }
        
        public MenuItem? CreateMenuItem(MenuItem newMenuItem)
        {
            var check = _context.MenuItems.Find(newMenuItem.Id);

            if (check == null)
            {
                var entry = _context.MenuItems.Add(newMenuItem);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null;
        }
        
        public MenuItem? UpdateMenuItem(Guid id, MenuItem updatedMenuItem)
        {
            var menuItem = _context.MenuItems.Find(id);

            if (menuItem == null)
            {
                throw new Exception("Menu item not found");
            }
            
            menuItem.Name = updatedMenuItem.Name;
            menuItem.Price = updatedMenuItem.Price;
            menuItem.Description = updatedMenuItem.Description;
            menuItem.Category = updatedMenuItem.Category;

            _context.MenuItems.Update(menuItem);
            _context.SaveChanges();

            return menuItem;
        }
        
        public bool DeleteMenuItem(Guid id)
        {
            var menuItem = _context.MenuItems.Find(id);

            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
