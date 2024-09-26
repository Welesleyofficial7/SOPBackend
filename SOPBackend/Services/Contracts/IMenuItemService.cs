namespace SOPBackend.Services;

public interface IMenuItemService
{
    IEnumerable<MenuItem>? GetAllMenuItems();
    MenuItem? GetMenuItemById(Guid id);
    MenuItem? CreateMenuItem(MenuItem newMenuItem);
    MenuItem? UpdateMenuItem(Guid id, MenuItem updatedMenuItem);
    bool DeleteMenuItem(Guid id);
}