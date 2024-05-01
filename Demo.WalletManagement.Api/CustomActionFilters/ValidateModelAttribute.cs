using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.WalletManagement.Api.CustomActionFilters;

public class ValidateModelAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (context.ModelState.IsValid == false) 
        {
            context.Result = new BadRequestResult();

        }
    }
}