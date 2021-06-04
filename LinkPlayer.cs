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

        public override void PreUpdate()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            Player player = Main.LocalPlayer;

            var file = TMumbleLink.instance.OpenLinkFile();


            if (lm.uiVersion != 2)
            {
                StringBuilder nameSB = new StringBuilder("TMumbleLink", 256);
                StringBuilder descSB = new StringBuilder("TMumbleLink adds Positional Audio support for Terraria.", 2048);

                lm.fAvatarPosition = new float[3];
                lm.fAvatarTop = new float[3];
                lm.fAvatarFront = new float[3];

                lm.fCameraPosition = new float[3];
                lm.fCameraTop = new float[3];
                lm.fCameraFront = new float[3];

                lm.name = nameSB.ToString();
                lm.description = descSB.ToString();
                lm.uiVersion = 2;
            }

            lm.uiTick++;

            lm.fAvatarPosition[0] = (player.position.X / 16f) * 0.6096f;
            lm.fAvatarPosition[1] = -(player.position.Y / 16f) * 0.6096f;
            lm.fAvatarPosition[2] = 0f;

            lm.fAvatarTop[0] = 0f;
            lm.fAvatarTop[1] = 1f;
            lm.fAvatarTop[2] = 0f;

            lm.fAvatarFront[0] = 0f;
            lm.fAvatarFront[1] = 0f;
            lm.fAvatarFront[2] = 1f;

            lm.fCameraPosition[0] = (player.position.X / 16f) * 0.6096f;
            lm.fCameraPosition[1] = -(player.position.Y / 16f) * 0.6096f;
            lm.fCameraPosition[2] = 0f;

            lm.fCameraTop[0] = 0f;
            lm.fCameraTop[1] = 1f;
            lm.fCameraTop[2] = 0f;

            lm.fCameraFront[0] = 0f;
            lm.fCameraFront[1] = 0f;
            lm.fCameraFront[2] = 1f;

            if (file != null)
                file.Write(lm);
        }

        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            if (player == Main.LocalPlayer)
            {
                StringBuilder idSB = new StringBuilder(player.name, 256);
                lm.identity = idSB.ToString();
                lm.context = new byte[256];

                lm.fAvatarPosition = new float[3];
                lm.fAvatarTop = new float[3];
                lm.fAvatarFront = new float[3];

                lm.fCameraPosition = new float[3];
                lm.fCameraTop = new float[3];
                lm.fCameraFront = new float[3];

                Array.Copy(Encoding.UTF8.GetBytes("tmumblelink"), lm.context, 11);
                lm.context_len = 11;
            }
        }

        private void debugPrint()
        {
            TMumbleLink.instance.Logger.Info("Version: " + lm.uiVersion);
            TMumbleLink.instance.Logger.Info("Ticks: " + lm.uiTick);
            TMumbleLink.instance.Logger.Info("APosX: " + lm.fAvatarPosition[0]);
            TMumbleLink.instance.Logger.Info("APosY: " + lm.fAvatarPosition[1]);
            TMumbleLink.instance.Logger.Info("APosZ: " + lm.fAvatarPosition[2]);
            TMumbleLink.instance.Logger.Info("ATopX: " + lm.fAvatarTop[0]);
            TMumbleLink.instance.Logger.Info("ATopY: " + lm.fAvatarTop[1]);
            TMumbleLink.instance.Logger.Info("ATopZ: " + lm.fAvatarTop[2]);
            TMumbleLink.instance.Logger.Info("AFrontX: " + lm.fAvatarFront[0]);
            TMumbleLink.instance.Logger.Info("AFrontY: " + lm.fAvatarFront[1]);
            TMumbleLink.instance.Logger.Info("AFrontZ: " + lm.fAvatarFront[2]);
            TMumbleLink.instance.Logger.Info("CPosX: " + lm.fCameraPosition[0]);
            TMumbleLink.instance.Logger.Info("CPosY: " + lm.fCameraPosition[1]);
            TMumbleLink.instance.Logger.Info("CPosZ: " + lm.fCameraPosition[2]);
            TMumbleLink.instance.Logger.Info("CTopX: " + lm.fCameraTop[0]);
            TMumbleLink.instance.Logger.Info("CTopY: " + lm.fCameraTop[1]);
            TMumbleLink.instance.Logger.Info("CTopZ: " + lm.fCameraTop[2]);
            TMumbleLink.instance.Logger.Info("CFrontX: " + lm.fCameraFront[0]);
            TMumbleLink.instance.Logger.Info("CFrontY: " + lm.fCameraFront[1]);
            TMumbleLink.instance.Logger.Info("CFrontZ: " + lm.fCameraFront[2]);
            TMumbleLink.instance.Logger.Info("Name: " + lm.name);
            TMumbleLink.instance.Logger.Info("Identity: " + lm.identity);
            TMumbleLink.instance.Logger.Info("Context: " + lm.context);
            TMumbleLink.instance.Logger.Info("Context Length: " + lm.context_len);
            TMumbleLink.instance.Logger.Info("Description: " + lm.description);
        }
    }
}
