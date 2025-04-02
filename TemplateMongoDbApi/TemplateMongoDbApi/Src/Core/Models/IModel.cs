using Microsoft.EntityFrameworkCore;

namespace TemplateMongoDbApi.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}