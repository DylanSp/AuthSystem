namespace AuthSystem.Interfaces
{
    public interface ICryptoRng
    {
        byte[] GetRandomBytes(int numBytes);
    }
}
