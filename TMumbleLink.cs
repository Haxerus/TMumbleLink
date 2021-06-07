using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TMumbleLink
{
	public class TMumbleLink : Mod
	{
        internal static TMumbleLink instance;
        private MumbleLinkFile file;

        public override void Load()
        {
            instance = this;
        }

        public override void Unload()
        {
            instance = null;
        }

        /*
         * Free the Memory Mapped File when the player disconnects
         * Note: This doesn't get called if the server is closed before the player disconnects.
         * There is no work around due to tModLoader limitations.
         */
        public override void PreSaveAndQuit()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
                return;

            if (file != null)
                file.Dispose();

            file = null;
        }

        public MumbleLinkFile OpenLinkFile()
        {
            if (file == null)
            {
                file = new MumbleLinkFile();
            }

            return file;
        }
    }
}