using AOT.Models;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Windows.Documents;

namespace AOT
{
    public class DatabaseService
    {
        private readonly IMongoCollection<Project> _projects;
        private readonly IMongoCollection<Leader> _projectleaders;
        private readonly IMongoCollection<Department> _departments;
        private readonly IMongoCollection<ProjectType> _projectsType;
        private readonly IMongoCollection<Portfolio> _portfolios;
        private readonly IMongoCollection<User> _users;

        public DatabaseService()
        {
            var client = new MongoClient("mongodb://localhost:27017"); // or your Mongo URI
            var database = client.GetDatabase("AOT");
            _projects = database.GetCollection<Project>("Projects");
            _projectleaders = database.GetCollection<Leader>("Projektleiter");
            _departments = database.GetCollection<Department>("Abteilungen");
            _projectsType = database.GetCollection<ProjectType>("Projektart");
            _portfolios = database.GetCollection<Portfolio>("Portfolio");
            _users = database.GetCollection<User>("User");
        }

        public List<Project> Search(IMongoCollection<Project> collection, string budgetMin, string budgetMax, string name, bool isPflicht, string leader, string department, string type)
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

            var filterBuilder = Builders<Project>.Filter;
            var filter = FilterDefinition<Project>.Empty;

            filter = Builders<Project>.Filter.Gt(p => p.Budget, budgetMinValue) &
                         Builders<Project>.Filter.Lt(p => p.Budget, budgetMaxValue) &
                         Builders<Project>.Filter.Regex("name", new BsonRegularExpression(name, "i"));

            if (isPflicht)
                filter &= filterBuilder.Eq(x => x.Pflicht, pflicht);
            if (!string.IsNullOrWhiteSpace(leader))
                filter &= filterBuilder.Eq(x => x.Leader, leader);

            if (!string.IsNullOrWhiteSpace(department))
                filter &= filterBuilder.Eq(x => x.Department, department);

            if (!string.IsNullOrWhiteSpace(type))
                filter &= filterBuilder.Eq(x => x.Type, type);

            return collection.Find(filter).SortByDescending(p => p.Pflicht).ThenByDescending(k => k.KPI).ToList();
        }

        public List<Project> SearchActiveProject(string budgetMin, string budgetMax, string name, bool isPflicht, string leader, string department, string type)
        {
            return Search(_projects, budgetMin, budgetMax, name, isPflicht, leader, department, type);
        }

        public async Task<List<ProjectType>> GetProjectTypesAsync()
        {
            return await _projectsType.Find(_ => true).ToListAsync();
        }

        public async Task<List<Portfolio>> GetPortfoliosAsync()
        {
            return await _portfolios.Find(_ => true).ToListAsync();
        }

        public async Task<List<Leader>> GetLeadersAsync()
        {
            return await _projectleaders.Find(FilterDefinition<Leader>.Empty).ToListAsync();
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _departments.Find(_ => true).ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            return await _departments.Find(a => a.DepartmentId == departmentId).FirstOrDefaultAsync();
        }

        public async Task<Leader> GetLeaderByDepartmentId(int departmentId)
        {
            return await _projectleaders.Find(a => a.DepartmentId == departmentId).FirstOrDefaultAsync();
        }

        public async void UpdateProjectStatus(Project project, string newStatus)
        {
            var filter = Builders<Project>.Filter.Eq(p => p.Id, project.Id);
            var update = Builders<Project>.Update.Set(p => p.Status, newStatus);

            var result = await _projects.UpdateOneAsync(filter, update);
        }

        public async void MoveToDone(Project project)
        {
            var document = await _projects.Find(a => a.Id == project.Id).FirstOrDefaultAsync();
            if (document == null)
            {
                return;
            }
            await _projects.DeleteOneAsync(a => a.Id == project.Id);
        }

        public async void MoveToFail(Project project)
        {
            var document = await _projects.Find(a => a.Id == project.Id).FirstOrDefaultAsync();
            if (document == null)
            {
                return;
            }
            await _projects.DeleteOneAsync(a => a.Id == project.Id);
        }



        public List<Project> GetAllProjects()
        {
            return _projects.Find(FilterDefinition<Project>.Empty).SortByDescending(p => p.Pflicht).ThenByDescending(k => k.KPI).ToList();
        }


        public int CountActiveProjects()
        {
            return (int)_projects.CountDocuments(FilterDefinition<Project>.Empty);
        }

        public int CountActiveProjectsByPortfolio(string portfolioName)
        {
            return (int)_projects.CountDocuments(p => p.PortfolioName == portfolioName);
        }

        public async Task<User> GetUserByUserName(string usernanme)

        {
            return await _users.Find(a => a.username == usernanme).FirstOrDefaultAsync();
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
