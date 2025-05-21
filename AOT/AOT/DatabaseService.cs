using AOT.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Documents;

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

        public List<Project> Search(string budgetMin, string budgetMax, string name, bool isPflicht)
        {
            decimal budgetMinValue;
            decimal budgetMaxValue;

            if (string.IsNullOrWhiteSpace(budgetMin))
            {
                budgetMinValue = decimal.MinValue;
            }
            else
            {
                budgetMinValue = decimal.Parse(budgetMin);
            }

            if (string.IsNullOrWhiteSpace(budgetMax))
            {
                budgetMaxValue = decimal.MaxValue;
            }
            else
            {
                budgetMaxValue = decimal.Parse(budgetMax);
            }

            var pflicht = "";
            if (isPflicht)
            {
                pflicht = "Ja";
            }

            var filter = Builders<Project>.Filter.Gt(p => p.Budget, budgetMinValue) &
                         Builders<Project>.Filter.Lt(p => p.Budget, budgetMaxValue) &
                         Builders<Project>.Filter.Regex("name", new BsonRegularExpression(name, "i")) &
                         Builders<Project>.Filter.Eq(x => x.Pflicht, pflicht);

            return _projects.Find(filter).ToList();
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
            return _projects.Find(FilterDefinition<Project>.Empty).SortByDescending(p => p.Pflicht).ThenByDescending(k => k.KPI).ToList();
        }

        public List<Project> GetAllCompletedProjects()
        {
            return _finishedProjects.Find(FilterDefinition<Project>.Empty).SortByDescending(p => p.Pflicht).ThenByDescending(k => k.KPI).ToList();
        }

        public List<Project> GetAllFailedProjects()
        {
            return _failedProjects.Find(FilterDefinition<Project>.Empty).SortByDescending(p => p.Pflicht).ThenByDescending(k => k.KPI).ToList();
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
