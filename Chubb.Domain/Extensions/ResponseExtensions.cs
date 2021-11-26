using Chubb.Domain.Common;
using Chubb.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Domain.Extensions
{
    public static class ResponseServicesExtensions
    {
        public static void AddException(this ResponseServices response, Exception ex)
        {
            response.IsSuccess = false;
            response.IsException = true;
            response.Message = string.Format(ExceptionMessage.ErrorShowUser, DateTime.Now.ToString(), new StackTrace().GetFrame(1).GetMethod().Name, ex.Message);
            response.MessageException = string.Format(ExceptionMessage.ErrorException, DateTime.Now.ToString(), ex.StackTrace, ex.GetOriginalException().Message);
        }
    }
}
