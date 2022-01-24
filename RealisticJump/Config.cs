using Rocket.API;

namespace RealisticJump
{
    public class Config : IRocketPluginConfiguration
    {
        public double JumpCooldown;
        public void LoadDefaults()
        {
            JumpCooldown = 1;
        }
    }
}
