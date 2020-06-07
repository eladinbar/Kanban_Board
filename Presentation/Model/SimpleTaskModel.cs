using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class SimpleTaskModel : NotifiableModelObject
    {

        public string Title { get; set; }//???????
        public DateTime DueDate { get; set; }//???????????

        //properties of border and backgrounfd colors

        public SimpleTaskModel(BackendController controller, string title, DateTime dueDate) : base(controller)
        {
            this.Title = title;
            this.DueDate = dueDate;
        }
    }
}