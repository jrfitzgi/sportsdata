using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public enum HtmlBlobType
    {
        None = 0,
        NhlRoster,
        NhlGame,
        NhlEvents,
        NhlFaceOffs,
        NhlShots,
        NhlHomeToi,
        NhlVisitorToi,
        NhlShootout
    }
}
