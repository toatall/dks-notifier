using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    internal class EntityMoving: IEntity
    {       
        public string Id { get; set; }
        public string Fio { get; set; }
        public string TabNum { get; set; }
        public string Login { get; set; }
        public string OrgName { get; set; }
        public string DepNameOld { get; set; }
        public string DepNameNew { get; set; }
        public string PostOld { get; set; }
        public string PostNew { get; set; }
        public DateTime Date { get; set; }

        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}_{3}", Id, TabNum, PostNew, Date);
        }

        public string TypeEntity()
        {
            return "MOVING";
        }
    }
}
