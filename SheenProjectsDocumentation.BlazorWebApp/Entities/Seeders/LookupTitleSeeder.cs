using ProjectsDocumentation.BlazorWebApp.DbContext;
using ProjectsDocumentation.BlazorWebApp.Enums;

namespace ProjectsDocumentation.BlazorWebApp.Entities.Seeders
{
    public class LookupTitleSeeder
    {
        private readonly DataBaseContext _dataBaseContext;
        public LookupTitleSeeder(DataBaseContext dataBaseContext)
        {
            _dataBaseContext = dataBaseContext;
        }
        public void SeedData()
        {
            List<LookupTitle> data = Data();
            try
            {
                _dataBaseContext.LookupTitles.AddRange(data);
                _dataBaseContext.SaveChanges();
            }
            catch
            {

            }
        }

        public List<LookupTitle> Data()
        {
            return [
                LookupTitle.CreateLookupTitleSeeder((int)LookupTitleEnum.ModelType , "Model Type", [
                    LookupValue.CreateLookupValueSeeder((int)ModelType.Web, "Web"),
                    LookupValue.CreateLookupValueSeeder((int)ModelType.Mobile, "Mobile")
                ]),

                LookupTitle.CreateLookupTitleSeeder((int)LookupTitleEnum.Method , "Method", [
                    LookupValue.CreateLookupValueSeeder((int)Method.GET, "GET"),
                    LookupValue.CreateLookupValueSeeder((int)Method.POST, "POST"),
                    LookupValue.CreateLookupValueSeeder((int)Method.PUT, "PUT"),
                    LookupValue.CreateLookupValueSeeder((int)Method.DELETE, "DELETE"),
                    LookupValue.CreateLookupValueSeeder((int)Method.PATCH, "PATCH"),
                ]),

                LookupTitle.CreateLookupTitleSeeder((int)LookupTitleEnum.RequestType , "Request Type", [
                    LookupValue.CreateLookupValueSeeder((int)RequestType.Rest, "Rest"),
                    LookupValue.CreateLookupValueSeeder((int)RequestType.Soap, "Soap"),
                    LookupValue.CreateLookupValueSeeder((int)RequestType.GraphQl, "GraphQl"),
                ]),

                LookupTitle.CreateLookupTitleSeeder((int)LookupTitleEnum.StatusCode , "Status Code", [
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.OK200, "OK200"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.InternalServerError500, "InternalServerError500"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.BadRequest400, "BadRequest400"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.Unauthorized401, "Unauthorized401"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.Forbidden403, "Forbidden403"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.NotFound404, "NotFound404"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.Conflict409, "Conflict409"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.Created201, "Created201"),
                    LookupValue.CreateLookupValueSeeder((int)StatusCode.NoContent204, "NoContent204"),
                ]),
            ];
        }
    }
}
