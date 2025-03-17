using Microsoft.EntityFrameworkCore;

namespace TemplateRabbitMQApi.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}