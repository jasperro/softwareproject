namespace SoftwareProject.Types
{
    public class Usersettings
    {
        public Usersettings(int simulatietijd)
        {
            Simtime = simulatietijd;
        }

        public int Simtime { get; set; }
    }
}