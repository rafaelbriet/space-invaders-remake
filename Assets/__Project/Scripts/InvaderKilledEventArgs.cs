using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvadersRemake
{
    public class InvaderKilledEventArgs
    {
        public InvaderKilledEventArgs(Invader invader)
        {
            Invader = invader;
        }

        public Invader Invader { get; set; }
    }
}
