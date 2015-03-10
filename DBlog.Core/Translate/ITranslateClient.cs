using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBlog.Core.Translate
{
    public interface ITranslateClient
    {
        T Execute<T>(ITransRequest<T> request) where T : TransResponse;
    }
}
