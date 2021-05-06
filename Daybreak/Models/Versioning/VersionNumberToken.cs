namespace Daybreak.Models.Versioning
{
    public sealed class VersionNumberToken : VersionToken
    {
        public int Number { get; }
        internal VersionNumberToken(int number)
        {
            this.Number = number;
        }

        protected override string Stringify()
        {
            return this.Number.ToString();
        }
    }
}
