namespace ProjectsDocumentation.BlazorWebApp.Enums
{
    public enum LookupTitleEnum
    {
        ModelType = 1,
        Method,
        RequestType,
        StatusCode
    }

    public enum ModelType
    {
        Web = 1,
        Mobile = 2,
    }

    public enum Method
    {
        GET = 10,
        POST = 11,
        PUT = 12,
        DELETE = 13,
        PATCH = 14,
    }

    public enum RequestType
    {
        Rest = 20,
        Soap = 21,
        GraphQl = 22,
    }

    public enum StatusCode
    {
        OK200 = 30,
        InternalServerError500 = 31,
        BadRequest400 = 32,
        Unauthorized401 = 33,
        Forbidden403 = 34,
        NotFound404 = 35,
        Conflict409 = 36,
        Created201 = 37,
        NoContent204 = 38,
    }
}
