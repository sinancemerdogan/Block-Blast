public interface IFallable 
{
   bool IsFalling { get; set; }
   void Fall(int targetY);
}
