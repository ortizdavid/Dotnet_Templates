using Microsoft.EntityFrameworkCore;

namespace TemplateRedisApi.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}