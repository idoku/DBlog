using AopAlliance.Intercept;
using DBlog.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBlog.Core.Intercept
{
    public class LoggingAroundAdvice : IMethodInterceptor
    {
        public LoggingAroundAdvice()
        {
        }

        static long ID = 0;
        public object Invoke(IMethodInvocation invocation)
        {
            long nID = Interlocked.Increment(ref ID);//设置调用标识

            object returnValue = null;


            LogHelper.Info(
                String.Format("Invocation ID {0}: start \"{1}, {2}\"",
                nID,
                invocation.Method.Name,
                invocation.TargetType.FullName)
               );

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                returnValue = invocation.Proceed();
                sw.Stop();//执行成功，记录调用时长
                LogHelper.Info(
                    String.Format("Invocation ID {0}: finish \"{1}, {2}\", elapsed {3} milliseconds",
                    nID,
                    invocation.Method.Name,
                    invocation.TargetType.FullName,
                    sw.ElapsedMilliseconds.ToString("#,##0"))
                    );
            }
            catch (Exception ex)
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
                LogHelper.Info(
                    String.Format("Invocation ID {0}: break \"{1}, {2}\", elapsed {3} milliseconds, Message:{4}",
                    nID,
                    invocation.Method.Name,
                    invocation.TargetType.FullName,
                    sw.ElapsedMilliseconds.ToString("#,##0"),
                    ex.Message)
                    );
                LogHelper.Error(
                    String.Format("Invocation ID {0}: break \"{1}, {2}\", Message:{3}",
                    nID,
                    invocation.Method.Name,
                    invocation.TargetType.FullName,
                    ex)
                    );
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }

            if (returnValue == null)
            {
                if (!IsNullableType(invocation.Method.ReturnType))
                {
                    switch (invocation.Method.ReturnType.FullName)
                    {
                        case "System.Boolean":
                            returnValue = false;
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.UInt16":
                        case "System.UInt32":
                        case "System.UInt64":
                        case "System.Double":
                        case "System.Single":
                            returnValue = 0;
                            break;
                        case "System.Decimal":
                            returnValue = 0m;
                            break;
                        case "System.DateTime":
                            returnValue = DateTime.MinValue;
                            break;
                    }
                }
            }

            return returnValue;
        }

        bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType &&
                theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}
