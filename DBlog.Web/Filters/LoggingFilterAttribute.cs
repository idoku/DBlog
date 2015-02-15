using Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DBlog.Web.Filters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        #region Loggin
        protected static readonly Common.Logging.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private const string StopwatchKey = "LoggingStopWatch";
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var loggingWatch = Stopwatch.StartNew();
            filterContext.HttpContext.Items.Add(StopwatchKey, loggingWatch);
            var message = new StringBuilder();
            message.Append(string.Format("Executing controller {0}, action {1}",
            filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
            filterContext.ActionDescriptor.ActionName));
            log.Info(message);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Items[StopwatchKey] != null)
            {
                var loggingWatch = (Stopwatch)filterContext.HttpContext.Items[StopwatchKey];
                long timeSpent = loggingWatch.ElapsedMilliseconds;
                var message = new StringBuilder();
                message.Append(string.Format("Finished executing controller {0}, action {1} - time spent {2}",
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    filterContext.ActionDescriptor.ActionName,
                    timeSpent));
                log.Info(message);
                filterContext.HttpContext.Items.Remove(StopwatchKey);
            }
        }
    }
}