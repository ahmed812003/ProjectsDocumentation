using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using ProjectsDocumentation.BlazorWebApp.DbContext;
using ProjectsDocumentation.BlazorWebApp.Dtos.Dashboard;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.GetAll;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Update;
using ProjectsDocumentation.BlazorWebApp.Entities;
using ProjectsDocumentation.BlazorWebApp.Enums;
using ProjectsDocumentation.BlazorWebApp.Interfaces.Projects;
using ProjectsDocumentation.BlazorWebApp.Requests.Projects;

namespace ProjectsDocumentation.BlazorWebApp.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly DataBaseContext _baseContext;

        public ProjectService(DataBaseContext baseContext)
        {
            _baseContext = baseContext;
        }

        public async Task<DashboardDto> GetProjectListAsync()
        {
            IQueryable<ProjectListDto> query = from project in _baseContext.Projects.Include(p => p.ProjectModels).Include(p => p.ProjectTables)
                                               select new ProjectListDto
                                               {
                                                   Id = project.Id,
                                                   Name = project.ProjectName,
                                                   Description = project.Description,
                                                   TotalNumberOfModels = project.ProjectModels.Count,
                                                   TotalNumberOfTables = project.ProjectTables.Count
                                               };
            List<ProjectListDto> projects = await query.ToListAsync();

            foreach (ProjectListDto projectListDto in projects)
            {
                if (projectListDto.Description.Length > 200)
                {
                    projectListDto.Description = projectListDto.Description.Substring(0, 200) + "...";
                }
            }

            return new DashboardDto
            {
                TotalNumberOfProjects = projects.Count,
                TotalNumberOfModels = projects.Sum(p => p.TotalNumberOfModels),
                TotalNumberOfTables = projects.Sum(p => p.TotalNumberOfTables),
                ProjectListDtos = projects
            };
        }

        public async Task<ProjectDetailsDto> GetProjectDetailsAsync(int Id)
        {
            IQueryable<ProjectDetailsDto> query = from project in _baseContext.Projects.Include(p => p.ProjectTables).Include(p => p.ProjectModels)
                                                  .ThenInclude(pm => pm.Endpoints)
                                                  where project.Id == Id
                                                  select new ProjectDetailsDto
                                                  {
                                                      Id = project.Id,
                                                      ProjectName = project.ProjectName,
                                                      ProjectDescription = project.Description,
                                                      TotalNumberOfEndPoints = project.ProjectModels.Sum(pm => pm.Endpoints.Count),
                                                      TotalNumberOfModels = project.ProjectModels.Count,
                                                      TotalNumberOfTables = project.ProjectTables.Count,
                                                      ProjectTables = project.ProjectTables.Select(pt => new ProjectTablesDto
                                                      {
                                                          Id = pt.Id,
                                                          Name = pt.TableName
                                                      }).ToList(),
                                                      ProjectModelsDtos = project.ProjectModels.Select(pm => new ProjectModelsDetailsDto
                                                      {
                                                          Id = pm.Id,
                                                          ModelName = pm.ModelName,
                                                          ModelType = (ModelType)pm.ModelTypeId,
                                                      }).ToList()
                                                  };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProjectForUpdateDto> GetProjectByIdAsync(int id)
        {
            IQueryable<ProjectForUpdateDto> query = from project in _baseContext.Projects.Include(p => p.ProjectTables).Include(p => p.ProjectModels)
                                                    where project.Id == id
                                                    select new ProjectForUpdateDto
                                                    {
                                                        Id = project.Id,
                                                        ProjectName = project.ProjectName,
                                                        ProjectDescription = project.Description,
                                                        ProjectModelForUpdateDtos = project.ProjectModels.Select(model => new ProjectModelForUpdateDto
                                                        {
                                                            Id = model.Id,
                                                            ModelName = model.ModelName,
                                                            ModelType = (ModelType)model.ModelTypeId
                                                        }).ToList(),
                                                        ProjectTableForUpdateDtos = project.ProjectTables.Select(table => new ProjectTableForUpdateDto
                                                        {
                                                            Id = table.Id,
                                                            TableName = table.TableName
                                                        }).ToList()
                                                    };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<ProjectTablesDto>> GetProjectReflectionTables(int projectId)
        {
            IQueryable<ProjectTablesDto> query = from projectTable in _baseContext.projectTables
                                                 where projectTable.ProjectId == projectId
                                                 select new ProjectTablesDto
                                                 {
                                                     Id = projectTable.Id,
                                                     Name = projectTable.TableName
                                                 };
            return await query.ToListAsync();
        }

        public async Task<int> AddProjectAsync(UpsertProjectRequest request)
        {
            // Validate input request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Project request cannot be null.");
            }

            // Validate project name
            if (string.IsNullOrWhiteSpace(request.ProjectName))
            {
                throw new ArgumentException("Project name is required and cannot be empty or whitespace.", nameof(request.ProjectName));
            }

            if (request.ProjectName.Length > 255)
            {
                throw new ArgumentException("Project name cannot exceed 255 characters.", nameof(request.ProjectName));
            }

            // Validate description (assuming it's required based on your schema)
            if (string.IsNullOrWhiteSpace(request.ProjectDescription))
            {
                throw new ArgumentException("Project description is required.", nameof(request.ProjectDescription));
            }

            // Validate models
            if (request.ProjectModelsRequest == null)
            {
                throw new ArgumentException("Project models collection cannot be null.", nameof(request.ProjectModelsRequest));
            }

            if (!request.ProjectModelsRequest.Any())
            {
                throw new ArgumentException("At least one model is required.", nameof(request.ProjectModelsRequest));
            }

            foreach (var model in request.ProjectModelsRequest)
            {
                if (string.IsNullOrWhiteSpace(model.ModelName))
                {
                    throw new ArgumentException("Model name is required for all models.", nameof(request.ProjectModelsRequest));
                }

                if (model.ModelName.Length > 255)
                {
                    throw new ArgumentException($"Model name '{model.ModelName}' cannot exceed 255 characters.", nameof(request.ProjectModelsRequest));
                }

                if (model.ModelType == default)
                {
                    throw new ArgumentException($"Model type is required for model '{model.ModelName}'.", nameof(request.ProjectModelsRequest));
                }
            }

            // Validate tables
            if (request.ProjectTablesRequest == null)
            {
                throw new ArgumentException("Project tables collection cannot be null.", nameof(request.ProjectTablesRequest));
            }

            if (!request.ProjectTablesRequest.Any())
            {
                throw new ArgumentException("At least one table is required.", nameof(request.ProjectTablesRequest));
            }

            foreach (var table in request.ProjectTablesRequest)
            {
                if (string.IsNullOrWhiteSpace(table.TableName))
                {
                    throw new ArgumentException("Table name is required for all tables.", nameof(request.ProjectTablesRequest));
                }

                if (table.TableName.Length > 255)
                {
                    throw new ArgumentException($"Table name '{table.TableName}' cannot exceed 255 characters.", nameof(request.ProjectTablesRequest));
                }
            }

            // Check for duplicate project name
            bool projectExists = await _baseContext.Projects.AnyAsync(p => p.ProjectName == request.ProjectName);
            if (projectExists)
            {
                throw new InvalidOperationException($"A project with the name '{request.ProjectName}' already exists.");
            }

            // Check for duplicate model names within the request
            var duplicateModels = request.ProjectModelsRequest
                .GroupBy(m => new { m.ModelName, m.ModelType })
                .Where(g => g.Count() > 1)
                .Select(g => g.Key.ModelName)
                .ToList();

            if (duplicateModels.Any())
            {
                throw new ArgumentException($"Duplicate model names with the same type found: {string.Join(", ", duplicateModels)}", nameof(request.ProjectModelsRequest));
            }

            // Check for duplicate table names within the request
            var duplicateTables = request.ProjectTablesRequest
                .GroupBy(t => t.TableName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateTables.Any())
            {
                throw new ArgumentException($"Duplicate table names found: {string.Join(", ", duplicateTables)}", nameof(request.ProjectTablesRequest));
            }

            // Create the project entity
            Project project = new Project
            {
                ProjectName = request.ProjectName.Trim(),
                Description = request.ProjectDescription.Trim(),
                ProjectModels = request.ProjectModelsRequest.Select(model => new ProjectModel
                {
                    ModelName = model.ModelName.Trim(),
                    ModelTypeId = (int)model.ModelType,
                }).ToList(),
                ProjectTables = request.ProjectTablesRequest.Select(table => new ProjectTable
                {
                    TableName = table.TableName.Trim(),
                }).ToList()
            };

            try
            {
                await _baseContext.Projects.AddAsync(project);
                await _baseContext.SaveChangesAsync();
                return project.Id;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle specific database exceptions
                if (dbEx.InnerException is MySqlException mySqlEx)
                {
                    switch (mySqlEx.Number)
                    {
                        case 1062: // MySQL duplicate key error
                            throw new InvalidOperationException("A duplicate project, model, or table was detected.", dbEx);
                        case 1452: // MySQL foreign key constraint fails
                            throw new InvalidOperationException("Invalid model type reference.", dbEx);
                    }
                }

                throw new InvalidOperationException("An error occurred while saving the project to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while adding the project.", ex);
            }
        }

        public async Task<int> UpdateProjectAsync(int id, UpsertProjectRequest request)
        {
            // Validate ID
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Project ID must be a positive number.");
            }

            // Validate input request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Project request cannot be null.");
            }

            // Validate project name
            if (string.IsNullOrWhiteSpace(request.ProjectName))
            {
                throw new ArgumentException("Project name is required and cannot be empty or whitespace.", nameof(request.ProjectName));
            }

            if (request.ProjectName.Length > 255)
            {
                throw new ArgumentException("Project name cannot exceed 255 characters.", nameof(request.ProjectName));
            }

            // Validate description
            if (string.IsNullOrWhiteSpace(request.ProjectDescription))
            {
                throw new ArgumentException("Project description is required.", nameof(request.ProjectDescription));
            }

            // Validate models
            if (request.ProjectModelsRequest == null)
            {
                throw new ArgumentException("Project models collection cannot be null.", nameof(request.ProjectModelsRequest));
            }

            if (!request.ProjectModelsRequest.Any())
            {
                throw new ArgumentException("At least one model is required.", nameof(request.ProjectModelsRequest));
            }

            foreach (var model in request.ProjectModelsRequest)
            {
                if (string.IsNullOrWhiteSpace(model.ModelName))
                {
                    throw new ArgumentException("Model name is required for all models.", nameof(request.ProjectModelsRequest));
                }

                if (model.ModelName.Length > 255)
                {
                    throw new ArgumentException($"Model name '{model.ModelName}' cannot exceed 255 characters.", nameof(request.ProjectModelsRequest));
                }

                if (model.ModelType == default)
                {
                    throw new ArgumentException($"Model type is required for model '{model.ModelName}'.", nameof(request.ProjectModelsRequest));
                }
            }

            // Validate tables
            if (request.ProjectTablesRequest == null)
            {
                throw new ArgumentException("Project tables collection cannot be null.", nameof(request.ProjectTablesRequest));
            }

            if (!request.ProjectTablesRequest.Any())
            {
                throw new ArgumentException("At least one table is required.", nameof(request.ProjectTablesRequest));
            }

            foreach (var table in request.ProjectTablesRequest)
            {
                if (string.IsNullOrWhiteSpace(table.TableName))
                {
                    throw new ArgumentException("Table name is required for all tables.", nameof(request.ProjectTablesRequest));
                }

                if (table.TableName.Length > 255)
                {
                    throw new ArgumentException($"Table name '{table.TableName}' cannot exceed 255 characters.", nameof(request.ProjectTablesRequest));
                }
            }

            // Check for duplicate project name (excluding current project)
            bool projectNameExists = await _baseContext.Projects
                .AnyAsync(p => p.ProjectName == request.ProjectName && p.Id != id);

            if (projectNameExists)
            {
                throw new InvalidOperationException($"A project with the name '{request.ProjectName}' already exists.");
            }

            // Check for duplicate model names within the request
            var duplicateModels = request.ProjectModelsRequest
                .GroupBy(m => new { m.ModelName, m.ModelType })
                .Where(g => g.Count() > 1)
                .Select(g => g.Key.ModelName)
                .ToList();

            if (duplicateModels.Any())
            {
                throw new ArgumentException($"Duplicate model names with the same type found: {string.Join(", ", duplicateModels)}", nameof(request.ProjectModelsRequest));
            }

            // Check for duplicate table names within the request
            var duplicateTables = request.ProjectTablesRequest
                .GroupBy(t => t.TableName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateTables.Any())
            {
                throw new ArgumentException($"Duplicate table names found: {string.Join(", ", duplicateTables)}", nameof(request.ProjectTablesRequest));
            }

            // Load existing project with tracking
            var project = await _baseContext.Projects
                .Include(p => p.ProjectModels)
                .Include(p => p.ProjectTables)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} was not found.");
            }

            // Update project properties
            project.ProjectName = request.ProjectName.Trim();
            project.Description = request.ProjectDescription.Trim();

            // Handle models update more efficiently
            var existingModels = project.ProjectModels.ToList();
            var requestModels = request.ProjectModelsRequest.ToList();

            // Remove models not in the request
            var modelsToRemove = existingModels
                .Where(em => !requestModels.Any(rm => rm.Id == em.Id))
                .ToList();

            foreach (var model in modelsToRemove)
            {
                project.ProjectModels.Remove(model);
            }

            // Update existing models and add new ones
            foreach (var modelRequest in requestModels)
            {
                var existingModel = existingModels.FirstOrDefault(em => em.Id == modelRequest.Id);

                if (existingModel != null)
                {
                    // Update existing model
                    existingModel.ModelName = modelRequest.ModelName.Trim();
                    existingModel.ModelTypeId = (int)modelRequest.ModelType;
                }
                else
                {
                    // Add new model
                    project.ProjectModels.Add(new ProjectModel
                    {
                        ModelName = modelRequest.ModelName.Trim(),
                        ModelTypeId = (int)modelRequest.ModelType
                    });
                }
            }

            // Handle tables update more efficiently
            var existingTables = project.ProjectTables.ToList();
            var requestTables = request.ProjectTablesRequest.ToList();

            // Remove tables not in the request
            var tablesToRemove = existingTables
                .Where(et => !requestTables.Any(rt => rt.Id == et.Id))
                .ToList();

            foreach (var table in tablesToRemove)
            {
                project.ProjectTables.Remove(table);
            }

            // Update existing tables and add new ones
            foreach (var tableRequest in requestTables)
            {
                var existingTable = existingTables.FirstOrDefault(et => et.Id == tableRequest.Id);

                if (existingTable != null)
                {
                    // Update existing table
                    existingTable.TableName = tableRequest.TableName.Trim();
                }
                else
                {
                    // Add new table
                    project.ProjectTables.Add(new ProjectTable
                    {
                        TableName = tableRequest.TableName.Trim()
                    });
                }
            }

            try
            {
                await _baseContext.SaveChangesAsync();
                return project.Id;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle specific database exceptions
                if (dbEx.InnerException is MySqlException mySqlEx)
                {
                    switch (mySqlEx.Number)
                    {
                        case 1062: // MySQL duplicate key error
                            throw new InvalidOperationException("A duplicate project, model, or table was detected.", dbEx);
                        case 1452: // MySQL foreign key constraint fails
                            throw new InvalidOperationException("Invalid model type reference.", dbEx);
                    }
                }

                throw new InvalidOperationException("An error occurred while updating the project in the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred while updating the project.", ex);
            }
        }

    }
}