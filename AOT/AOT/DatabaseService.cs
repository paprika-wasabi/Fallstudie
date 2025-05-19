using AOT.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AOT
{
    public class DatabaseService
    {
        private readonly IMongoCollection<Project> _projects;
        private readonly IMongoCollection<Project> _finishedProjects;
        private readonly IMongoCollection<Project> _failedProjects;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017"); // or your Mongo URI
            var database = client.GetDatabase("AOT");
            _projects = database.GetCollection<Project>("Projects");
            _finishedProjects = database.GetCollection<Project>("Finished_Projects");
            _failedProjects = database.GetCollection<Project>("Failed_Projects");
        }
        public async void MoveToDone(Project project)
        {
            var document = await _projects.Find(a => a.Id == project.Id).FirstOrDefaultAsync();
            if (document == null)
            {
                return;
            }
            await _finishedProjects.InsertOneAsync(document);
            await _projects.DeleteOneAsync(a => a.Id == project.Id);
        }

        public async void MoveToFail(Project project)
        {
            var document = await _projects.Find(a => a.Id == project.Id).FirstOrDefaultAsync();
            if (document == null)
            {
                return;
            }
            await _failedProjects.InsertOneAsync(document);
            await _projects.DeleteOneAsync(a => a.Id == project.Id);
        }

        public List<Project> GetAllProjects()
        {
            return _projects.Find(FilterDefinition<Project>.Empty).ToList();
        }

        public List<Project> GetAllCompletedProjects()
        {
            return _finishedProjects.Find(FilterDefinition<Project>.Empty).ToList();
        }

        public List<Project> GetAllFailedProjects()
        {
            return _failedProjects.Find(FilterDefinition<Project>.Empty).ToList();
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
