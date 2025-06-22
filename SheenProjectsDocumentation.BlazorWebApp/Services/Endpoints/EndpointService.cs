using Microsoft.EntityFrameworkCore;
using ProjectsDocumentation.BlazorWebApp.DbContext;
using ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints;
using ProjectsDocumentation.BlazorWebApp.Dtos.Projects.Details;
using ProjectsDocumentation.BlazorWebApp.Entities;
using ProjectsDocumentation.BlazorWebApp.Enums;
using ProjectsDocumentation.BlazorWebApp.Interfaces.Endpoints;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpoint;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpointVersion;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpoint;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpointVersion;
using System.Text;
using System.Text.Json;
using Endpoint = ProjectsDocumentation.BlazorWebApp.Entities.Endpoint;

namespace ProjectsDocumentation.BlazorWebApp.Services.Endpoints
{
    public class EndpointService : IEndpointService
    {
        private readonly DataBaseContext _baseContext;

        public EndpointService(DataBaseContext baseContext)
        {
            _baseContext = baseContext;
        }

        public async Task<List<EndpointDto>> GetEndpointForSpecificModel(int modelId)
        {
            IQueryable<EndpointDto> endpointDtos = from endpoint in _baseContext.Endpoints.Include(e => e.EndpointVersions)
                                                   where endpoint.ProjectModelId == modelId
                                                   select new EndpointDto
                                                   {
                                                       Description = endpoint.Description,
                                                       Id = endpoint.Id,
                                                       Method = (Method)endpoint.MethodId,
                                                       Name = endpoint.Name,
                                                       URL = endpoint.EndpointUrl,
                                                       EndpointVersionDtos = endpoint.EndpointVersions.Select(ev => new EndpointVersionDto
                                                       {
                                                           Id = ev.Id,
                                                           Version = ev.Version,
                                                           Description = ev.Description,
                                                       }).ToList()
                                                   };
            return await endpointDtos.ToListAsync();
        }

        public async Task<EndpointForUpdateDto> GetEndpointForUpdateByIdAsync(int endpointId)
        {
            Endpoint endpoint = await _baseContext.Endpoints.Include(e => e.EndpointVersions).FirstOrDefaultAsync(e => e.Id == endpointId);
            if (endpoint == null)
            {
                throw new ArgumentException($"Endpoint with ID {endpointId} does not exist.");
            }

            EndpointForUpdateDto endpointDto = new EndpointForUpdateDto
            {
                Id = endpoint.Id,
                Description = endpoint.Description,
                Method = (Method)endpoint.MethodId,
                Name = endpoint.Name,
                URL = endpoint.EndpointUrl
            };
            return endpointDto;
        }

        public async Task<EndpointVersionDto> GetEndpointVersionDetails(int endpointVersionId)
        {
            IQueryable<EndpointVersionDto> endpointVersionDtos = from endpointVersion in _baseContext.EndpointVersions
                                                                               where endpointVersion.Id == endpointVersionId
                                                                               select new EndpointVersionDto
                                                                               {
                                                                                   Id = endpointVersionId,
                                                                                   Description = endpointVersion.Description,
                                                                                   Version = endpointVersion.Version,
                                                                                   ReflectionTables = (from endpointReflection in _baseContext.EndpointReflections
                                                                                                       join projectTable in _baseContext.projectTables
                                                                                                       on endpointReflection.ProjectTableId equals projectTable.Id
                                                                                                       where endpointReflection.EndpointVersionId == endpointVersion.Id
                                                                                                       select new ReflectionTableDto
                                                                                                       {
                                                                                                           Id = endpointReflection.Id,
                                                                                                           TableName = projectTable.TableName,
                                                                                                       }).ToList(),
                                                                                   EndpointRequestDtos = (from endpointRequest in _baseContext.EndpointRequests
                                                                                                          where endpointRequest.EndpointVersionId == endpointVersion.Id
                                                                                                          select new EndpointRequestDto
                                                                                                          {
                                                                                                              Id = endpointRequest.Id,
                                                                                                              Body = endpointRequest.Body,
                                                                                                              Query = endpointRequest.QueryParameters,
                                                                                                              Header = endpointRequest.Headers,
                                                                                                          }).ToList(),
                                                                                   EndpointResponseDtos = (from endpointResponse in _baseContext.EndpointResponses
                                                                                                           where endpointResponse.EndpointVersionId == endpointVersion.Id
                                                                                                           select new EndpointResponseDto
                                                                                                           {
                                                                                                               Id = endpointResponse.Id,
                                                                                                               Response = endpointResponse.EndpointResponseData,
                                                                                                               StatusCode = (StatusCode)endpointResponse.StatusCodeId
                                                                                                           }).ToList()
                                                                               };
            return await endpointVersionDtos.FirstOrDefaultAsync();
        }

        public async Task<int> AddEndpointAsync(int modelId, AddEndpointRequest request)
        {
            // Validate input: required fields
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Endpoint name cannot be null or empty.");

            if (!Enum.IsDefined(typeof(Method), request.Method))
                throw new ArgumentException("Invalid HTTP method provided.");

            if (string.IsNullOrWhiteSpace(request.EndpointUrl))
                throw new ArgumentException("Endpoint URL cannot be null or empty.");

            if (request.EndpointUrl.Length > 500)
                throw new ArgumentException("Endpoint URL must not exceed 500 characters.");

            if (request.Name.Length > 255)
                throw new ArgumentException("Endpoint name must not exceed 255 characters.");

            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Length > 1000)
                throw new ArgumentException("Endpoint description must not exceed 1000 characters.");

            // Validate: Project Model existence
            var projectModel = await _baseContext.ProjectModels
                .Include(pm => pm.Endpoints)
                .FirstOrDefaultAsync(pm => pm.Id == modelId);

            if (projectModel == null)
                throw new ArgumentException($"Project model with ID {modelId} does not exist.");

            // Validate: Method exists in lookupvalues table
            bool methodExists = await _baseContext.LookupValues
                .AnyAsync(lv => lv.Id == (int)request.Method);
            if (!methodExists)
                throw new ArgumentException($"HTTP method with ID {(int)request.Method} does not exist in the lookup values.");

            // Check: Duplicate endpoint
            var endpointExistInDb = await _baseContext.Endpoints.FirstOrDefaultAsync(e =>
                e.EndpointUrl == request.EndpointUrl &&
                e.MethodId == (int)request.Method &&
                e.ProjectModelId == modelId);

            if (endpointExistInDb != null)
                throw new ArgumentException($"An endpoint with the same URL and method already exists in this project model. Endpoint URL: {request.EndpointUrl}, Method: {request.Method}.");

            // Add endpoint
            try
            {
                projectModel.Endpoints ??= new List<Endpoint>();

                var endpoint = new Endpoint
                {
                    Description = request.Description ?? string.Empty,
                    MethodId = (int)request.Method,
                    EndpointUrl = request.EndpointUrl,
                    Name = request.Name,
                };

                projectModel.Endpoints.Add(endpoint);
                _baseContext.ProjectModels.Update(projectModel);
                await _baseContext.SaveChangesAsync();

                return endpoint.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the endpoint: {ex.Message}", ex);
            }
        }

        public async Task<int> UpdateEndpointAsync(int modelId, UpdateEndpointRequest updateEndpointRequest)
        {
            // Validate input
            if (updateEndpointRequest.Id <= 0)
                throw new ArgumentException("Invalid endpoint ID.");

            if (string.IsNullOrWhiteSpace(updateEndpointRequest.EndpointUrl))
                throw new ArgumentException("Endpoint URL cannot be null or empty.");

            if (updateEndpointRequest.EndpointUrl.Length > 500)
                throw new ArgumentException("Endpoint URL must not exceed 500 characters.");

            if (string.IsNullOrWhiteSpace(updateEndpointRequest.Name))
                throw new ArgumentException("Endpoint name cannot be null or empty.");

            if (updateEndpointRequest.Name.Length > 255)
                throw new ArgumentException("Endpoint name must not exceed 255 characters.");

            if (!string.IsNullOrWhiteSpace(updateEndpointRequest.Description) &&
                updateEndpointRequest.Description.Length > 1000)
                throw new ArgumentException("Endpoint description must not exceed 1000 characters.");

            if (!Enum.IsDefined(typeof(Method), updateEndpointRequest.Method))
                throw new ArgumentException("Invalid HTTP method provided.");

            // Check if project model exists
            var projectModel = await _baseContext.ProjectModels
                .Include(pm => pm.Endpoints)
                .FirstOrDefaultAsync(pm => pm.Id == modelId);

            if (projectModel == null)
                throw new ArgumentException($"Project model with ID {modelId} does not exist.");

            // Check if method exists in lookupvalues
            bool methodExists = await _baseContext.LookupValues
                .AnyAsync(lv => lv.Id == (int)updateEndpointRequest.Method);
            if (!methodExists)
                throw new ArgumentException($"HTTP method with ID {(int)updateEndpointRequest.Method} does not exist in the lookup values.");

            // Find the endpoint to update
            var endpoint = await _baseContext.Endpoints
                .FirstOrDefaultAsync(e => e.Id == updateEndpointRequest.Id && e.ProjectModelId == modelId);

            if (endpoint == null)
                throw new ArgumentException($"Endpoint with ID {updateEndpointRequest.Id} does not exist in this project model.");

            // Check for duplicates (excluding the current endpoint being updated)
            bool duplicateExists = await _baseContext.Endpoints.AnyAsync(e =>
                e.ProjectModelId == modelId &&
                e.Id != updateEndpointRequest.Id &&
                e.EndpointUrl == updateEndpointRequest.EndpointUrl &&
                e.MethodId == (int)updateEndpointRequest.Method);

            if (duplicateExists)
                throw new ArgumentException("Another endpoint with the same URL and method already exists in this project model.");

            // Update the endpoint
            try
            {
                endpoint.EndpointUrl = updateEndpointRequest.EndpointUrl;
                endpoint.MethodId = (int)updateEndpointRequest.Method;
                endpoint.Name = updateEndpointRequest.Name;
                endpoint.Description = updateEndpointRequest.Description ?? string.Empty;

                _baseContext.Endpoints.Update(endpoint);
                await _baseContext.SaveChangesAsync();

                return endpoint.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the endpoint: {ex.Message}", ex);
            }
        }

        public async Task<int> AddEndpointVersionAsync(int endpointId, AddEndpointVersionRequest request)
        {
            // Validate EndpointId
            var endpoint = await _baseContext.Endpoints.FindAsync(endpointId);
            if (endpoint == null)
                throw new ArgumentException($"Endpoint with ID {endpointId} does not exist.");

            // Validate Version
            if (string.IsNullOrWhiteSpace(request.Version))
                throw new ArgumentException("Version is required.");

            if (request.Version.Length > 50)
                throw new ArgumentException("Version must not exceed 50 characters.");

            // Check if version already exists for this endpoint
            bool versionExists = await _baseContext.EndpointVersions
                .AnyAsync(ev => ev.EndpointId == endpointId && ev.Version == request.Version);

            if (versionExists)
                throw new ArgumentException($"Version '{request.Version}' already exists for this endpoint.");

            // Validate Description
            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Length > 1000)
                throw new ArgumentException("Description must not exceed 1000 characters.");

            // Validate Responses
            if (request.AddEnpointResponseRequests != null)
            {
                foreach (var response in request.AddEnpointResponseRequests)
                {
                    if (string.IsNullOrWhiteSpace(response.Response))
                        throw new ArgumentException("Response content is required for each response.");

                    bool statusExists = await _baseContext.LookupValues.AnyAsync(lv => lv.Id == (int)response.StatusCode);
                    if (!statusExists)
                        throw new ArgumentException($"Invalid status code: {(int)response.StatusCode}");
                }
            }

            // Validate Request JSON fields
            if (request.AddEnpointRequestRequest != null)
            {
                if (!IsValidJson(request.AddEnpointRequestRequest.Body))
                    throw new ArgumentException("Invalid JSON in Body.");

                if (!IsValidJson(request.AddEnpointRequestRequest.Header))
                    throw new ArgumentException("Invalid JSON in Header.");

                if (!IsValidJson(request.AddEnpointRequestRequest.QuerParameters))
                    throw new ArgumentException("Invalid JSON in Query Parameters.");
            }

            // Validate Reflection Tables
            if (request.ReflectionTableIds != null && request.ReflectionTableIds.Any())
            {
                var existingIds = await _baseContext.projectTables
                    .Where(pt => request.ReflectionTableIds.Contains(pt.Id))
                    .Select(pt => pt.Id)
                    .ToListAsync();

                var missing = request.ReflectionTableIds.Except(existingIds).ToList();
                if (missing.Any())
                    throw new ArgumentException($"Some reflection table IDs are invalid: {string.Join(", ", missing)}");
            }

            // Start transaction
            using var transaction = await _baseContext.Database.BeginTransactionAsync();

            try
            {
                // Create EndpointVersion
                var endpointVersion = new EndpointVersion
                {
                    EndpointId = endpointId,
                    Version = request.Version,
                    Description = request.Description
                };

                _baseContext.EndpointVersions.Add(endpointVersion);
                await _baseContext.SaveChangesAsync();

                int versionId = endpointVersion.Id;

                // Add Request
                if (request.AddEnpointRequestRequest != null)
                {
                    var endpointRequest = new EndpointRequest
                    {
                        EndpointVersionId = versionId,
                        Headers = request.AddEnpointRequestRequest.Header,
                        Body = request.AddEnpointRequestRequest.Body,
                        QueryParameters = request.AddEnpointRequestRequest.QuerParameters
                    };

                    _baseContext.EndpointRequests.Add(endpointRequest);
                }

                // Add Responses
                if (request.AddEnpointResponseRequests != null)
                {
                    foreach (var res in request.AddEnpointResponseRequests)
                    {
                        var endpointResponse = new EndpointResponse
                        {
                            EndpointVersionId = versionId,
                            StatusCodeId = (int)res.StatusCode,
                            EndpointResponseData = res.Response
                        };

                        _baseContext.EndpointResponses.Add(endpointResponse);
                    }
                }

                // Add Reflections
                if (request.ReflectionTableIds != null)
                {
                    foreach (var tableId in request.ReflectionTableIds)
                    {
                        var reflection = new EndpointReflection
                        {
                            EndpointVersionId = versionId,
                            ProjectTableId = tableId
                        };

                        _baseContext.EndpointReflections.Add(reflection);
                    }
                }

                await _baseContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return versionId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Failed to add endpoint version: {ex.Message}", ex);
            }
        }

        public async Task<int> UpdateEndpointVersionAsync(int endpointId, UpdateEndpointVersionRequest request)
        {
            // 1. Validate existence
            var endpoint = await _baseContext.Endpoints.FindAsync(endpointId);
            if (endpoint == null)
                throw new ArgumentException($"Endpoint with ID {endpointId} does not exist.");

            var endpointVersion = await _baseContext.EndpointVersions
                .FirstOrDefaultAsync(ev => ev.Id == request.Id && ev.EndpointId == endpointId);

            if (endpointVersion == null)
                throw new ArgumentException($"Endpoint version with ID {request.Id} does not exist for endpoint {endpointId}.");

            // 2. Validate version string
            if (string.IsNullOrWhiteSpace(request.Version))
                throw new ArgumentException("Version is required.");

            if (request.Version.Length > 50)
                throw new ArgumentException("Version must not exceed 50 characters.");

            // 3. Check uniqueness (excluding current)
            bool versionExists = await _baseContext.EndpointVersions.AnyAsync(ev =>
                ev.EndpointId == endpointId &&
                ev.Version == request.Version &&
                ev.Id != request.Id);

            if (versionExists)
                throw new ArgumentException($"Another version '{request.Version}' already exists for this endpoint.");

            // 4. Validate description
            if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Length > 1000)
                throw new ArgumentException("Description must not exceed 1000 characters.");

            // 5. Validate responses
            if (request.UpdateEnpointResponseRequests != null)
            {
                foreach (var response in request.UpdateEnpointResponseRequests)
                {
                    if (string.IsNullOrWhiteSpace(response.Response))
                        throw new ArgumentException("Response content is required.");

                    bool statusCodeExists = await _baseContext.LookupValues
                        .AnyAsync(lv => lv.Id == (int)response.StatusCode);

                    if (!statusCodeExists)
                        throw new ArgumentException($"Invalid status code: {(int)response.StatusCode}");
                }
            }

            // 6. Validate request JSON
            if (request.UpdateEnpointRequestRequest != null)
            {
                if (!IsValidJson(request.UpdateEnpointRequestRequest.Body))
                    throw new ArgumentException("Invalid JSON in Body.");
                if (!IsValidJson(request.UpdateEnpointRequestRequest.Header))
                    throw new ArgumentException("Invalid JSON in Header.");
                if (!IsValidJson(request.UpdateEnpointRequestRequest.QuerParameters))
                    throw new ArgumentException("Invalid JSON in Query Parameters.");
            }

            // 7. Validate reflection tables
            if (request.ReflectionTableIds != null && request.ReflectionTableIds.Any())
            {
                var existingTableIds = await _baseContext.projectTables
                    .Where(pt => request.ReflectionTableIds.Contains(pt.Id))
                    .Select(pt => pt.Id)
                    .ToListAsync();

                var invalidIds = request.ReflectionTableIds.Except(existingTableIds).ToList();
                if (invalidIds.Any())
                    throw new ArgumentException($"Invalid reflection table IDs: {string.Join(", ", invalidIds)}");
            }

            // 8. Perform the update in a transaction
            using var tx = await _baseContext.Database.BeginTransactionAsync();

            try
            {
                // Update version info
                endpointVersion.Version = request.Version;
                endpointVersion.Description = request.Description;
                _baseContext.EndpointVersions.Update(endpointVersion);

                // Delete existing request/response/reflection rows
                var oldRequest = await _baseContext.EndpointRequests
                    .FirstOrDefaultAsync(er => er.EndpointVersionId == request.Id);

                if (oldRequest != null)
                    _baseContext.EndpointRequests.Remove(oldRequest);

                var oldResponses = await _baseContext.EndpointResponses
                    .Where(r => r.EndpointVersionId == request.Id).ToListAsync();

                _baseContext.EndpointResponses.RemoveRange(oldResponses);

                var oldReflections = await _baseContext.EndpointReflections
                    .Where(r => r.EndpointVersionId == request.Id).ToListAsync();

                _baseContext.EndpointReflections.RemoveRange(oldReflections);

                await _baseContext.SaveChangesAsync();

                // Insert updated request
                if (request.UpdateEnpointRequestRequest != null)
                {
                    var newRequest = new EndpointRequest
                    {
                        EndpointVersionId = request.Id,
                        Headers = request.UpdateEnpointRequestRequest.Header,
                        Body = request.UpdateEnpointRequestRequest.Body,
                        QueryParameters = request.UpdateEnpointRequestRequest.QuerParameters
                    };
                    _baseContext.EndpointRequests.Add(newRequest);
                }

                // Insert updated responses
                if (request.UpdateEnpointResponseRequests != null)
                {
                    foreach (var response in request.UpdateEnpointResponseRequests)
                    {
                        var newResponse = new EndpointResponse
                        {
                            EndpointVersionId = request.Id,
                            StatusCodeId = (int)response.StatusCode,
                            EndpointResponseData = response.Response
                        };
                        _baseContext.EndpointResponses.Add(newResponse);
                    }
                }

                // Insert updated reflections
                if (request.ReflectionTableIds != null)
                {
                    foreach (var tableId in request.ReflectionTableIds)
                    {
                        var reflection = new EndpointReflection
                        {
                            EndpointVersionId = request.Id,
                            ProjectTableId = tableId
                        };
                        _baseContext.EndpointReflections.Add(reflection);
                    }
                }

                await _baseContext.SaveChangesAsync();
                await tx.CommitAsync();

                return request.Id;
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw new Exception($"Failed to update endpoint version: {ex.Message}", ex);
            }
        }

        bool IsValidJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            try
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                var reader = new System.Text.Json.Utf8JsonReader(bytes);
                System.Text.Json.JsonDocument.TryParseValue(ref reader, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}