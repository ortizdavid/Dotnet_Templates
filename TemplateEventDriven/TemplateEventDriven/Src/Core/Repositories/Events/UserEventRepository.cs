using TemplateEventDriven.Core.Models;
using TemplateEventDriven.Core.Models.Events;

namespace TemplateEventDriven.Core.Repositories.Events;

public class UserEventRepository : RepositoryBase<UserEvent>
{
    private readonly AppDbContext _context;

    public UserEventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
}