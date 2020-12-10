using UnityEngine;

namespace Mirror.Examples.NetworkRoom
{
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int index;

        [SyncVar]
        public uint score;

        public override void OnStartServer()
        {
            index = connectionToClient.connectionId;
            
        }
        
      

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"P{index}: {score.ToString("0000000")}");
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
           if(Input.GetKeyDown(KeyCode.M))
            {
                CmdAddScore();
            }
        }

        [Command]
       public void CmdAddScore()
        {
            score += 10;
        }
    }
}
