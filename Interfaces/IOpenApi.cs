using PayementMVC.Models;

namespace PayementMVC.Interfaces
{
    public interface IOpenApi
    {
        void Create(OpenApiModel model);
        List<OpenApiModel> GetAll();
        OpenApiModel GetById(int id);

    }
}
