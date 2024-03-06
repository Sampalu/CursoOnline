using CursoOnline.Dominio.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CursoOnline.Web.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            bool isAjaxCall = context.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
            
            if (isAjaxCall)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = context.Exception is ExceptionDeDominio ? 502 : 500;
                context.Result = context.Exception is ExceptionDeDominio dominio ? 
                    new JsonResult(dominio.MensagensDeErro) : 
                    new JsonResult("An error ocurred");
                context.ExceptionHandled = true;
            }

            base.OnException(context);  
        }
    }
}
