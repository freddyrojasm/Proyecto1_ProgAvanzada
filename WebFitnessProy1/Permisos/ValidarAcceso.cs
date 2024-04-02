using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFitnessProy1.Permisos
{
    public class ValidarAcceso : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.Session["usuario"] == null)

            {
                filterContext.Result = new RedirectResult("~/Login/Acceder");
            }

            base.OnActionExecuting(filterContext);
        }

    }
}