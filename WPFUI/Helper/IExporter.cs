using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.Helper
{
    public interface IExporter
    {
        void AddColumn(string value);
        void AddLineBreak();
        string Export(string exportPath);
    }
}
