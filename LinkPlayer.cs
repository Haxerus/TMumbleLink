using Microsoft.Xna.Framework;
using System;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TMumbleLink
{
    /*
     * ModPlayer object that implements the logic required to interact with the Mumble Link API
     */
    class LinkPlayer : ModPlayer
    {
        private LinkedMem lm;
        private bool initComplete = false;

        public override void PreUpdate()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            Player player = Main.LocalPlayer;

            var file = TMumbleLink.instance.OpenLinkFile();

            /*
             * Initialize static data needed for the link file
             * TODO: Find out how to make this not get called everytime any player joins
             */
            if (!initComplete)
            {
                lm.context = new byte[256];
                Array.Copy(Encoding.UTF8.GetBytes("tmumblelink"), lm.context, 11);
                lm.context_len = 11;

                lm.identity = player.name;

                lm.name = "Terraria";
                lm.description = "TMumbleLink adds Positional Audio support for Terraria.";
                lm.uiVersion = 2;

                initComplete = true;
            }

            /*
             * Update the linked file
             */

            float[] pos = { (player.position.X / 16f) * 0.6096f, -(player.position.Y / 16f) * 0.6096f, 0f };    // Pixel position gets converted to "meters"
            float[] top = { 0f, 1f, 0f };
            float[] front = { 0f, 0f, 1f };

            lm.uiTick++;

            lm.fAvatarPosition = pos;
            lm.fAvatarTop = top;
            lm.fAvatarFront = front;

            lm.fCameraPosition = pos;
            lm.fCameraTop = top;
            lm.fCameraFront = front;

            if (file != null && initComplete)
                file.Write(lm);
        }

        public override void OnEnterWorld(Player player)
        {
			if (Main.netMode != NetmodeID.MultiplayerClient)
                return;
			
            if (player.whoAmI == Main.myPlayer)
            {
                Main.NewText("Mumble Linked!", new Color(255, 240, 20));
            }
        }
    }
}
