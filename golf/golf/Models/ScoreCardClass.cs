using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace golf.Models
{
    public class ScoreCardClass : Hole
    {
        public int CompetetionGolfer_ID { get; set; }
  
        
        public int playerStrokes { get; set; }
        public int addedStrokes { get; set; }
        public int net { get; set; }
        public int points { get; set; }

        public void calcPoints()
        {
   
            net = playerStrokes - addedStrokes;
            
          
            int hcpStrokes = par + addedStrokes;
            if(hcpStrokes == playerStrokes)
            {
                points = 2;
            }
            else if(playerStrokes < hcpStrokes)
            {
                points = 2 + (1 * (hcpStrokes - playerStrokes)); 
            }
            else if(playerStrokes > hcpStrokes)
            {
                int p = 2 - (1*(playerStrokes - hcpStrokes));
                if(p < 0)
                {
                    points=0;
                }
                else{
                    points = p;
                }
                 
            }

        }
    }


}