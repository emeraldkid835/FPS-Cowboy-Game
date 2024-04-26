public interface IDamage // This is my IDamage interface, any monobehaviour with an IDamage aspect can use this method to take damage
{
    public enum DamageType
    {
        Sharp,
        Fire
    }
    void TakeDamage(float damage, DamageType damagetype);
}

public interface IInteract
{
    public bool validToReinteract();
    public string contextText();
    
    void Interaction();
}