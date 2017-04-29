using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sadjad_AI_CSharpClient_2017
{
    class Ai
    {
        public String ServerAdr = "http://keeg.ir:8898/game";
        public Int32 Token = 123;

        public void doTurn(ref Int32[] touch, ABlast.Board Board)
        {
            Random rnd = new Random();

            touch[0] = rnd.Next(0, 9);
            touch[1] = rnd.Next(0, 9);
        }
    }
}
