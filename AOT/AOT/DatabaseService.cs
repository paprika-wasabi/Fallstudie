using AOT.Models;
using MongoDB.Driver;

namespace AOT
{
    public class DatabaseService
    {
        private readonly IMongoCollection<Project> _projects;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017"); // or your Mongo URI
            var database = client.GetDatabase("AOT");
            _projects = database.GetCollection<Project>("Projects");
        }

        public List<Project> GetAllUsers()
        {
            return [];
        }

        public bool AddNewProject(Project newProject)
        {
            try
            {
                _projects.InsertOne(newProject);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }
    }
}
