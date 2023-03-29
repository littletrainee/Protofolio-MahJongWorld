using System.Collections.Generic;

using MahJongWorld.DiceMahJong;

namespace MahJongWorld.MahJong
{
    internal class Wall
    {
        public string Name { get; set; }
        public List<Dice> Hand { get; set; }
    }
}
