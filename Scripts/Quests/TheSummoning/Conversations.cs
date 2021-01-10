using Server.Mobiles;

namespace Server.Engines.Quests.Doom
{
    public class AcceptConversation : QuestConversation
    {
        public override object Message =>
                /* You have accepted Victoria's help.  She requires 1000 Daemon
* bones to summon the devourer.<BR><BR>
* 
* You may hand Victoria the bones as you collect them and she
* will keep count of how many you have brought her.<BR><BR>
* 
* Daemon bones can be collected via various means throughout
* Dungeon Doom.<BR><BR>
* 
* Good luck.
*/
                1050027;
        public override void OnRead()
        {
            System.AddObjective(new CollectBonesObjective());
        }
    }

    public class VanquishDaemonConversation : QuestConversation
    {
        public override object Message =>
                /* Well done brave soul.   I shall summon the beast to the circle
* of stones just South-East of here.  Take great care - the beast
* takes many forms.  Now hurry...
*/
                1050021;
        public override void OnRead()
        {
            System.From.SendMessage("Internal error: unable to find Victoria. Quest unable to continue.");
            System.Cancel();
        }
    }
}
