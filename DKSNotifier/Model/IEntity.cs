using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    internal interface IEntity
    {
        string GetUnique();

        string TypeEntity();

    }
}
