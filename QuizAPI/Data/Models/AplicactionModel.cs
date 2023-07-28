using System;
using System.Collections.Generic;

namespace API.Data.Models
{
    public partial class AplicactionModel
    {
        public AplicactionModel()
        {
            InverseIdPadreNavigation = new HashSet<AplicactionModel>();
        }

        public virtual AplicactionModel IdPadreNavigation { get; set; }
        public virtual ICollection<AplicactionModel> InverseIdPadreNavigation { get; set; }
    }
}
