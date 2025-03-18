using Microsoft.EntityFrameworkCore;

namespace TemplateApi.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}