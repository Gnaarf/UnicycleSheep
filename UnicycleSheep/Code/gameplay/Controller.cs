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

        protected float rotation = 0;
        protected float wantsToBalance = 0;
        protected bool jump = false;
        protected bool isLoadingJump = false;

        public Controller(PlayerCharacter _playerCharacter)
        {
            character = _playerCharacter;
        }

        public void update()
        {
            //discard any actions when dead
            if (character.isDead) return;

            process();

            character.rotation = rotation;
            character.wantsToBalance = wantsToBalance;
            character.jump = jump;
            character.isLoadingJump = isLoadingJump;
        }

        virtual protected void process(){}

    }
}
