using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKSNotifier.Model
{
    internal class EntityVacation: IEntity 
    {
        public string Id { get; set; }
        public string Fio { get; set; }
        public string TabNum { get; set; }
        public string Login { get; set; }
        public string Post { get; set; }
        public string Department { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int Days { get; set; }
        public string TypeName { get; set; }

        public string GetUnique()
        {
            return string.Format("{0}_{1}_{2}_{3}", Id, TabNum, DateBegin, DateEnd);
        }

        public string TypeEntity()
        {
            return "VACATION";
        }
    }
}
