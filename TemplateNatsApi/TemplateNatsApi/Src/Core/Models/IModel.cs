using Microsoft.EntityFrameworkCore;

namespace TemplateNatsApi.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}