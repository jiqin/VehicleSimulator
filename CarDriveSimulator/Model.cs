using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDriveSimulator
{
    class Model
    {
        public enum EditMode
        {
            Edit = 0,
            Simulate = 1,
        }
        public EditMode CurrentEditMode { get; private set; }

        public Model()
        {

        }

        public EditMode UpdateEditMode()
        {
            if (CurrentEditMode == EditMode.Edit)
            {
                CurrentEditMode = EditMode.Simulate;
            }
            else
            {
                CurrentEditMode = EditMode.Edit;
            }
            return CurrentEditMode;
        }
    }
}
