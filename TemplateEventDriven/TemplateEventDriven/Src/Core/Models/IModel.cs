using Microsoft.EntityFrameworkCore;

namespace TemplateEventDriven.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}