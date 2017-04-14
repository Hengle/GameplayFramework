using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Assets.DB
{


    partial class MikuData
    {
        partial class ModelListDataTable
        {
            public ModelListRow FindByName(string name)
            {
                var sdf = from ModelListRow item in Rows
                          where item.Name == name
                          select item;

                return sdf.FirstOrDefault();
            }
        }

        partial class DataInfoTemplateDataTable
        {
        }

        partial class CharacterTemplateDataTable
        {
        }
    }
}

namespace Assets.DB.MikuDataTableAdapters {
    
    
    public partial class ModelListTableAdapter {
    }
}
