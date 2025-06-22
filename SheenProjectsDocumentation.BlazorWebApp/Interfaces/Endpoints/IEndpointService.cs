using ProjectsDocumentation.BlazorWebApp.Dtos.Endpoints;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpoint;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.AddEndpointVersion;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpoint;
using ProjectsDocumentation.BlazorWebApp.Requests.Endpoints.UpdateEndpointVersion;

namespace ProjectsDocumentation.BlazorWebApp.Interfaces.Endpoints
{
    public interface IEndpointService
    {
        Task<List<EndpointDto>> GetEndpointForSpecificModel(int modelId);
        Task<EndpointForUpdateDto> GetEndpointForUpdateByIdAsync(int endpointId);
        Task<EndpointVersionDto> GetEndpointVersionDetails(int endpointVersionId);

        Task<int> AddEndpointAsync(int modelId, AddEndpointRequest request);
        Task<int> UpdateEndpointAsync(int modelId, UpdateEndpointRequest updateEndpointRequest);

        Task<int> AddEndpointVersionAsync(int endpointId, AddEndpointVersionRequest request);
        Task<int> UpdateEndpointVersionAsync(int endpointId, UpdateEndpointVersionRequest request);
    }
}
