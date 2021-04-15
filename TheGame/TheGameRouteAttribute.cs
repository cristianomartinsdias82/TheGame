using Microsoft.AspNetCore.Mvc;

namespace TheGame
{
    public class TheGameRouteAttribute : RouteAttribute
    {
        public TheGameRouteAttribute(string template) : base($"/api/v1/{template}") { }
    }
}
