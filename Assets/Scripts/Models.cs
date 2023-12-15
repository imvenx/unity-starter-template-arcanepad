using ArcanepadSDK.Models;

public class TakeDamageEvent : ArcaneBaseEvent
{
    public int damage;
    public TakeDamageEvent(int damage) : base("TakeDamage")
    {
        this.damage = damage;
    }
}