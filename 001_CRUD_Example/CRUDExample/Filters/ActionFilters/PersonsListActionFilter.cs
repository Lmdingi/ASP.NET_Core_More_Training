using System;
using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters;

public class PersonsListActionFilter : IActionFilter
{
    private readonly ILogger<PersonsListActionFilter> _logger;

    public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");

        PersonsController personsController = (PersonsController)context.Controller;

        IDictionary<string, object?>? parameters =
        (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

        if (parameters != null)
        {
            if (parameters.ContainsKey("searchBy"))
            {
                personsController.ViewData["CurrentSearchBy"] = 
                Convert.ToString(context.HttpContext.Items["searchBy"]);
            }

            if (parameters.ContainsKey("searchString"))
            {
                personsController.ViewData["CurrentSearchString"] = 
                Convert.ToString(context.HttpContext.Items["searchString"]);
            }

            if (parameters.ContainsKey("sortBy"))
            {
                personsController.ViewData["CurrentSortBy"] = 
                Convert.ToString(context.HttpContext.Items["sortBy"]);
            }

            if (parameters.ContainsKey("sortOrder"))
            {
                personsController.ViewData["CurrentSortOrder"] = 
                Convert.ToString(context.HttpContext.Items["sortOrder"]);
            }
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items["arguments"] = context.ActionArguments;
        _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");

        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

            if (!string.IsNullOrEmpty(searchBy))
            {
                var searchByOptions = new List<string>()
                {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryID),
                    nameof(PersonResponse.Address),
                };

                if (searchByOptions.Any(temp => temp == searchBy) == false)
                {
                    _logger.LogInformation($"searchBy actual value {searchBy}", searchBy);
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    _logger.LogInformation($"searchBy updated value {searchBy}", searchBy);
                }
            }
        }
    }
}
