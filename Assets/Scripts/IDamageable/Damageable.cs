using System.Collections;
using System.Collections.Generic;


public class Damageable : IDamageable
{
    private float health;
    private List<IDamageObserver> observers = new List<IDamageObserver>();

    public Damageable(float initialHealth)
    {
        health = initialHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        NotifyObservers();
    }

    public void AddObserver(IDamageObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IDamageObserver observer)
    {
        observers.Remove(observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnDamageTaken(health);
        }
    }
}
