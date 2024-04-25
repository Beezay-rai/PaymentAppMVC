using Dapper;
using PayementMVC.Interfaces;
using PayementMVC.Models;
using PaymentApp.Areas.Admin;

namespace PayementMVC.Repository
{
    public class OpenApiRepository : IOpenApi
    {
        DatabaseUtilities _con;

        public OpenApiRepository(DatabaseUtilities db)
        {
            _con = db;
        }

        public void Create(OpenApiModel model)
        {
            using (var con = _con.GetConnection())
            {
                _con.OpenConnection();
                var query = "Insert Into OpenApi(Name,Version,Title,Description,Visibility) VALUES(@Name,@Version,@Title,@Description,@Visibility)";


                var response = con.Execute(query, model);
                Console.WriteLine($"{response} rows affected  !");
                _con.CloseConnection();

            }
        }

        public List<OpenApiModel> GetAll()
        {
            using (var con = _con.GetConnection())
            {
                _con.OpenConnection();
                var list = con.Query<OpenApiModel>("Select * from OpenApi").ToList();

                _con.CloseConnection();
                return list;

            }
        }

        public OpenApiModel GetById(int id)
        {
            using (var con = _con.GetConnection())
            {
                _con.OpenConnection();
                var data = con.Query<OpenApiModel>($"Select * from OpenApi where id ={id}").FirstOrDefault() ?? new OpenApiModel();

                _con.CloseConnection();
                return data;

            }
        }
    }
}
