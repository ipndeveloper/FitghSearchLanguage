using System;
using System.Collections.Generic;
using System.Text;
using Tranzact.Search.Models;

namespace Tranzact.Search.Managers
{
    public interface IFigthManager<TResponse> : IDisposable where TResponse :   FigthResponse
    {
    }
}
