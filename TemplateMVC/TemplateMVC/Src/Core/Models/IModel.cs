using Microsoft.EntityFrameworkCore;

namespace TemplateMVC.Core.Models;

public interface IModel
{
    static  abstract void ConfigureModel(ModelBuilder modelBuilder);
}