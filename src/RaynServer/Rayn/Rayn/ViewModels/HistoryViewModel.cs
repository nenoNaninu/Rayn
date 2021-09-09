using System.Collections.Generic;
using Rayn.Services.Models;

namespace Rayn.ViewModels
{
    public class HistoryViewModel
    {
       public  IEnumerable<History> Histories { get; }

       public HistoryViewModel(IEnumerable<History> histories)
       {
           Histories = histories;
       }
    }
}