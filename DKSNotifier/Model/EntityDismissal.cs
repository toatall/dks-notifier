using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    internal class EntityDismissal : IEntity
    {
        public string Id { get; set; }
        public string TabNumber { get; set; }
        public string Login { get; set; }
        public string Fio { get; set; }
        public string Post { get; set; }
        public string OrgName { get; set; }
        public string DepIndex { get; set; }
        public string DepName { get; set; }
        public DateTime DismissalDate { get; set; }
        public string DismissalDescription { get; set; }

        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}", Id, TabNumber, DismissalDate);
        }

        public string TypeEntity()
        {
            return "DISMISSAL";
        }

    }
}
