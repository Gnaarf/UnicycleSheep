using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicycleSheep
{
    class Controller
    {
        public PlayerCharacter character { get; private set; }

        public float rotation { get; protected set; }
        public float wantsToBalance { get; protected set; }
        public bool jump { get; protected set; }
        public bool isLoadingJump { get; protected set; }

        public Controller(PlayerCharacter _playerCharacter)
        {
            rotation = 0;
            wantsToBalance = 0;
            jump = false;
            isLoadingJump = false;
            
            character = _playerCharacter;
        }

        public void update()
        {
            //discard any actions when dead
            if (character.isDead) return;

            process();

            character.accelerate = rotation;
            character.rotate = wantsToBalance;
            character.jump = jump;
            character.isLoadingJump = isLoadingJump;
        }

        virtual protected void process(){}

    }
}
